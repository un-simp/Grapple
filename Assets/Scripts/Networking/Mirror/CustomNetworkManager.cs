using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

namespace Wildflare.Networking.Steamworks
{
    public class CustomNetworkManager : NetworkManager
    {
        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            base.OnServerAddPlayer(conn);

            CSteamID steamID = SteamMatchmaking.GetLobbyMemberByIndex(SteamLobby.lobbyID, numPlayers-1);

            var PlayerInfoDisplay = conn.identity.GetComponent<PlayerInfoDisplay>();

            PlayerInfoDisplay.SetSteamID(steamID.m_SteamID);
        }
    }
}
