using UnityEngine;
using Mirror;
using Steamworks;
using System.Collections;

namespace Wildflare.Networking.Steamworks
{
    public class SteamLobby : MonoBehaviour
    {
        [SerializeField] private GameObject buttons = null;
        [SerializeField] private GameObject loading = null;

        private NetworkManager networkManager;

        protected Callback<LobbyCreated_t> lobbyCreated;
        protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
        protected Callback<LobbyEnter_t> lobbyEntered;

        private const string hostAddressKey = "HostAddress";

        public static CSteamID lobbyID {get; private set;}

        void Start()
        {
            networkManager = GetComponent<NetworkManager>();

            if(!SteamManager.Initialized) return;

            lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
            lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        }

        public void HostLobby()
        {
            buttons.SetActive(false);

            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, networkManager.maxConnections);
        }

        private void OnLobbyCreated(LobbyCreated_t callback)
        {
            if(callback.m_eResult != EResult.k_EResultOK)
            {
                buttons.SetActive(true);
                return;
            }

            lobbyID = new CSteamID(callback.m_ulSteamIDLobby);

            networkManager.StartHost();

            SteamMatchmaking.SetLobbyData(lobbyID, hostAddressKey, SteamUser.GetSteamID().ToString());
        }

        private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback) {
            //Potentially will work Idk...;
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        }

        private void OnLobbyEntered(LobbyEnter_t callback)
        {
            if(NetworkServer.active) return;

            string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), hostAddressKey);

            networkManager.networkAddress = hostAddress;
            networkManager.StartClient();

            buttons.SetActive(false);

            loading.SetActive(true);

            StartCoroutine(AnimateLoading());
        }

        IEnumerator AnimateLoading()
        {
            loading.GetComponent<TMPro.TMP_Text>().text = "Loading";
            for(int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(0.5f);
                loading.GetComponent<TMPro.TMP_Text>().text = "Loading" + new string('.', i+1);
            }
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(AnimateLoading());
        }
    }
}