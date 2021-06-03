using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace Mirror.FizzySteam
{
    public class LegacyServer : LegacyCommon, IServer
    {
        private readonly int maxConnections;
        private int nextConnectionID;

        private readonly BidirectionalDictionary<CSteamID, int> steamToMirrorIds;

        private LegacyServer(FizzySteamworks transport, int maxConnections) : base(transport)
        {
            this.maxConnections = maxConnections;
            steamToMirrorIds = new BidirectionalDictionary<CSteamID, int>();
            nextConnectionID = 1;
        }

        public bool Disconnect(int connectionId)
        {
            if (steamToMirrorIds.TryGetValue(connectionId, out var steamID))
            {
                SendInternal(steamID, InternalMessages.DISCONNECT);
                steamToMirrorIds.Remove(connectionId);
                return true;
            }

            Debug.LogWarning("Trying to disconnect unknown connection id: " + connectionId);
            return false;
        }

        public void Shutdown()
        {
            foreach (KeyValuePair<CSteamID, int> client in steamToMirrorIds)
            {
                Disconnect(client.Value);
                WaitForClose(client.Key);
            }

            Dispose();
        }

        public void Send(int connectionId, byte[] data, int channelId)
        {
            if (steamToMirrorIds.TryGetValue(connectionId, out var steamId))
            {
                Send(steamId, data, channelId);
            }
            else
            {
                Debug.LogError("Trying to send on unknown connection: " + connectionId);
                OnReceivedError.Invoke(connectionId, new Exception("ERROR Unknown Connection"));
            }
        }

        public string ServerGetClientAddress(int connectionId)
        {
            if (steamToMirrorIds.TryGetValue(connectionId, out var steamId)) return steamId.ToString();

            Debug.LogError("Trying to get info on unknown connection: " + connectionId);
            OnReceivedError.Invoke(connectionId, new Exception("ERROR Unknown Connection"));
            return string.Empty;
        }

        public void FlushData() { }
        private event Action<int> OnConnected;
        private event Action<int, byte[], int> OnReceivedData;
        private event Action<int> OnDisconnected;
        private event Action<int, Exception> OnReceivedError;

        public static LegacyServer CreateServer(FizzySteamworks transport, int maxConnections)
        {
            var s = new LegacyServer(transport, maxConnections);

            s.OnConnected += id => transport.OnServerConnected.Invoke(id);
            s.OnDisconnected += id => transport.OnServerDisconnected.Invoke(id);
            s.OnReceivedData += (id, data, channel) =>
                transport.OnServerDataReceived.Invoke(id, new ArraySegment<byte>(data), channel);
            s.OnReceivedError += (id, exception) => transport.OnServerError.Invoke(id, exception);

            if (!SteamManager.Initialized) Debug.LogError("SteamWorks not initialized.");

            return s;
        }

        protected override void OnNewConnection(P2PSessionRequest_t result)
        {
            SteamNetworking.AcceptP2PSessionWithUser(result.m_steamIDRemote);
        }

        protected override void OnReceiveInternalData(InternalMessages type, CSteamID clientSteamID)
        {
            switch (type)
            {
                case InternalMessages.CONNECT:
                    if (steamToMirrorIds.Count >= maxConnections)
                    {
                        SendInternal(clientSteamID, InternalMessages.DISCONNECT);
                        return;
                    }

                    SendInternal(clientSteamID, InternalMessages.ACCEPT_CONNECT);

                    var connectionId = nextConnectionID++;
                    steamToMirrorIds.Add(clientSteamID, connectionId);
                    OnConnected.Invoke(connectionId);
                    Debug.Log($"Client with SteamID {clientSteamID} connected. Assigning connection id {connectionId}");
                    break;
                case InternalMessages.DISCONNECT:
                    if (steamToMirrorIds.TryGetValue(clientSteamID, out var connId))
                    {
                        OnDisconnected.Invoke(connId);
                        CloseP2PSessionWithUser(clientSteamID);
                        steamToMirrorIds.Remove(clientSteamID);
                        Debug.Log($"Client with SteamID {clientSteamID} disconnected.");
                    }

                    break;
                default:
                    Debug.Log("Received unknown message type");
                    break;
            }
        }

        protected override void OnReceiveData(byte[] data, CSteamID clientSteamID, int channel)
        {
            if (steamToMirrorIds.TryGetValue(clientSteamID, out var connectionId))
            {
                OnReceivedData.Invoke(connectionId, data, channel);
            }
            else
            {
                CloseP2PSessionWithUser(clientSteamID);
                Debug.LogError("Data received from steam client thats not known " + clientSteamID);
                OnReceivedError.Invoke(-1, new Exception("ERROR Unknown SteamID"));
            }
        }

        protected override void OnConnectionFailed(CSteamID remoteId)
        {
            var connectionId = steamToMirrorIds.TryGetValue(remoteId, out var connId) ? connId : nextConnectionID++;
            OnDisconnected.Invoke(connectionId);

            steamToMirrorIds.Remove(remoteId);
        }
    }
}