using Mirror;
using Steamworks;

namespace Wildflare.Networking.Steamworks
{
    public class CustomNetworkManager : NetworkManager
    {
        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            base.OnServerAddPlayer(conn);

            var steamID = SteamMatchmaking.GetLobbyMemberByIndex(SteamLobby.lobbyID, numPlayers - 1);

            var PlayerInfoDisplay = conn.identity.GetComponent<PlayerInfoDisplay>();

            PlayerInfoDisplay.SetSteamID(steamID.m_SteamID);
        }
    }
}