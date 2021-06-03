using System;
using System.Linq;
using Steamworks;
using UnityEngine;

namespace Mirror.FizzySteam
{
    public class NextServer : NextCommon, IServer
    {
        private Callback<SteamNetConnectionStatusChangedCallback_t> c_onConnectionChange;

        private readonly BidirectionalDictionary<HSteamNetConnection, int> connToMirrorID;

        private HSteamListenSocket listenSocket;
        private readonly int maxConnections;
        private int nextConnectionID;
        private readonly BidirectionalDictionary<CSteamID, int> steamIDToMirrorID;

        private NextServer(int maxConnections)
        {
            this.maxConnections = maxConnections;
            connToMirrorID = new BidirectionalDictionary<HSteamNetConnection, int>();
            steamIDToMirrorID = new BidirectionalDictionary<CSteamID, int>();
            nextConnectionID = 1;
            c_onConnectionChange =
                Callback<SteamNetConnectionStatusChangedCallback_t>.Create(OnConnectionStatusChanged);
        }

        public bool Disconnect(int connectionId)
        {
            if (connToMirrorID.TryGetValue(connectionId, out var conn))
            {
                Debug.Log($"Connection id {connectionId} disconnected.");
                SteamNetworkingSockets.CloseConnection(conn, 0, "Disconnected by server", false);
                steamIDToMirrorID.Remove(connectionId);
                connToMirrorID.Remove(connectionId);
                OnDisconnected(connectionId);
                return true;
            }

            Debug.LogWarning("Trying to disconnect unknown connection id: " + connectionId);
            return false;
        }

        public void FlushData()
        {
            foreach (var conn in connToMirrorID.FirstTypes) SteamNetworkingSockets.FlushMessagesOnConnection(conn);
        }

        public void ReceiveData()
        {
            foreach (var conn in connToMirrorID.FirstTypes.ToList())
                if (connToMirrorID.TryGetValue(conn, out var connId))
                {
                    var ptrs = new IntPtr[MAX_MESSAGES];
                    int messageCount;

                    if ((messageCount = SteamNetworkingSockets.ReceiveMessagesOnConnection(conn, ptrs, MAX_MESSAGES)) >
                        0)
                        for (var i = 0; i < messageCount; i++)
                        {
                            (var data, var ch) = ProcessMessage(ptrs[i]);
                            OnReceivedData(connId, data, ch);
                        }
                }
        }

        public void Send(int connectionId, byte[] data, int channelId)
        {
            if (connToMirrorID.TryGetValue(connectionId, out var conn))
            {
                var res = SendSocket(conn, data, channelId);

                if (res == EResult.k_EResultNoConnection || res == EResult.k_EResultInvalidParam)
                {
                    Debug.Log($"Connection to {connectionId} was lost.");
                    InternalDisconnect(connectionId, conn);
                }
                else if (res != EResult.k_EResultOK)
                {
                    Debug.LogError($"Could not send: {res.ToString()}");
                }
            }
            else
            {
                Debug.LogError("Trying to send on unknown connection: " + connectionId);
                OnReceivedError.Invoke(connectionId, new Exception("ERROR Unknown Connection"));
            }
        }

        public string ServerGetClientAddress(int connectionId)
        {
            if (steamIDToMirrorID.TryGetValue(connectionId, out var steamId)) return steamId.ToString();

            Debug.LogError("Trying to get info on unknown connection: " + connectionId);
            OnReceivedError.Invoke(connectionId, new Exception("ERROR Unknown Connection"));
            return string.Empty;
        }

        public void Shutdown()
        {
            SteamNetworkingSockets.CloseListenSocket(listenSocket);

            if (c_onConnectionChange != null)
            {
                c_onConnectionChange.Dispose();
                c_onConnectionChange = null;
            }
        }

        private event Action<int> OnConnected;
        private event Action<int, byte[], int> OnReceivedData;
        private event Action<int> OnDisconnected;
        private event Action<int, Exception> OnReceivedError;

        public static NextServer CreateServer(FizzySteamworks transport, int maxConnections)
        {
            var s = new NextServer(maxConnections);

            s.OnConnected += id => transport.OnServerConnected.Invoke(id);
            s.OnDisconnected += id => transport.OnServerDisconnected.Invoke(id);
            s.OnReceivedData += (id, data, ch) =>
                transport.OnServerDataReceived.Invoke(id, new ArraySegment<byte>(data), ch);
            s.OnReceivedError += (id, exception) => transport.OnServerError.Invoke(id, exception);

            if (!SteamManager.Initialized) Debug.LogError("SteamWorks not initialized.");

            s.Host();

            return s;
        }

        private void Host()
        {
            SteamNetworkingConfigValue_t[] options = { };
            listenSocket = SteamNetworkingSockets.CreateListenSocketP2P(0, options.Length, options);
        }

        private void OnConnectionStatusChanged(SteamNetConnectionStatusChangedCallback_t param)
        {
            var clientSteamID = param.m_info.m_identityRemote.GetSteamID64();
            if (param.m_info.m_eState == ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_Connecting)
            {
                if (connToMirrorID.Count >= maxConnections)
                {
                    Debug.Log($"Incoming connection {clientSteamID} would exceed max connection count. Rejecting.");
                    SteamNetworkingSockets.CloseConnection(param.m_hConn, 0, "Max Connection Count", false);
                    return;
                }

                EResult res;

                if ((res = SteamNetworkingSockets.AcceptConnection(param.m_hConn)) == EResult.k_EResultOK)
                    Debug.Log($"Accepting connection {clientSteamID}");
                else
                    Debug.Log($"Connection {clientSteamID} could not be accepted: {res.ToString()}");
            }
            else if (param.m_info.m_eState ==
                     ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_Connected)
            {
                var connectionId = nextConnectionID++;
                connToMirrorID.Add(param.m_hConn, connectionId);
                steamIDToMirrorID.Add(param.m_info.m_identityRemote.GetSteamID(), connectionId);
                OnConnected.Invoke(connectionId);
                Debug.Log($"Client with SteamID {clientSteamID} connected. Assigning connection id {connectionId}");
            }
            else if (param.m_info.m_eState ==
                     ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_ClosedByPeer)
            {
                if (connToMirrorID.TryGetValue(param.m_hConn, out var connId))
                    InternalDisconnect(connId, param.m_hConn);
            }
            else
            {
                Debug.Log($"Connection {clientSteamID} state changed: {param.m_info.m_eState.ToString()}");
            }
        }

        private void InternalDisconnect(int connId, HSteamNetConnection socket)
        {
            OnDisconnected.Invoke(connId);
            SteamNetworkingSockets.CloseConnection(socket, 0, "Graceful disconnect", false);
            connToMirrorID.Remove(connId);
            steamIDToMirrorID.Remove(connId);
            Debug.Log($"Client with ConnectionID {connId} disconnected.");
        }
    }
}