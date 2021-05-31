using System.Collections;
using System.Collections.Generic;
using Mirror;
using Steamworks;
using UnityEngine;
using TMPro;

public class PlayerInfoDisplay : NetworkBehaviour
{
    [SyncVar(hook=nameof(HandleSteamIDUpdated))]private ulong steamId;


    [SerializeField] private TMP_Text displayNameText;

    #region Server

    public void SetSteamID(ulong _steamID) => this.steamId = _steamID;

    #endregion

    #region Client

    void Update()
    {
        
    }

    private void HandleSteamIDUpdated(ulong oldID, ulong newID)
    {
        var cSteamID = new CSteamID(newID);

        displayNameText.text = SteamFriends.GetFriendPersonaName(cSteamID);
    }


    #endregion
}
