using Mirror;
using Steamworks;
using TMPro;
using UnityEngine;

public class PlayerInfoDisplay : NetworkBehaviour
{
    [SerializeField] private TMP_Text displayNameText;

    [SyncVar(hook = nameof(HandleSteamIDUpdated))]
    private ulong steamId;

    #region Server

    public void SetSteamID(ulong _steamID)
    {
        steamId = _steamID;
    }

    #endregion

    #region Client

    private void Update() { }

    private void HandleSteamIDUpdated(ulong oldID, ulong newID)
    {
        var cSteamID = new CSteamID(newID);

        displayNameText.text = SteamFriends.GetFriendPersonaName(cSteamID);
    }

    #endregion
}