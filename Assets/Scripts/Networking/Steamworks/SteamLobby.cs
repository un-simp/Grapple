using System.Collections;
using Mirror;
using Steamworks;
using TMPro;
using UnityEngine;

namespace Wildflare.Networking.Steamworks
{
    public class SteamLobby : MonoBehaviour
    {
        private const string hostAddressKey = "HostAddress";
        [SerializeField] private GameObject buttons;
        [SerializeField] private GameObject loading;
        protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;

        protected Callback<LobbyCreated_t> lobbyCreated;
        protected Callback<LobbyEnter_t> lobbyEntered;

        private NetworkManager networkManager;

        public static CSteamID lobbyID { get; private set; }


        private void Start()
        {
            if (!SteamManager.Initialized) return;

            networkManager = GetComponent<NetworkManager>();

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
            if (callback.m_eResult != EResult.k_EResultOK)
            {
                buttons.SetActive(true);
                return;
            }

            lobbyID = new CSteamID(callback.m_ulSteamIDLobby);

            networkManager.StartHost();

            SteamMatchmaking.SetLobbyData(lobbyID, hostAddressKey, SteamUser.GetSteamID().ToString());
        }

        private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
        {
            //Potentially will work Idk...;
            /*
            if (networkManager.isNetworkActive) {
                SteamMatchmaking.LeaveLobby(lobbyID);
                lobbyID = CSteamID.Nil;
            }
            */
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        }

        private void OnLobbyEntered(LobbyEnter_t callback)
        {
            if (NetworkServer.active) return;

            buttons.SetActive(false);
            loading.SetActive(true);
            StartCoroutine(AnimateLoading());

            var hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), hostAddressKey);

            networkManager.networkAddress = hostAddress;
            networkManager.StartClient();
        }

        private IEnumerator AnimateLoading()
        {
            loading.GetComponent<TMP_Text>().text = "Loading";
            for (var i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(0.5f);
                loading.GetComponent<TMP_Text>().text = "Loading" + new string('.', i + 1);
            }

            yield return new WaitForSeconds(0.5f);
            StartCoroutine(AnimateLoading());
        }
    }
}