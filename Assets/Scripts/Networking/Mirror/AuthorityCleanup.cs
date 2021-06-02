using UnityEngine;
using Mirror;
using System.Collections.Generic;
using Wildflare.Player;

public class AuthorityCleanup : NetworkBehaviour
{
    public GameObject[] itemsToDisable;
    public Camera cam;
    public GameObject hook;
    public GameObject awp;
    public GameObject nametag;

    public static List<GameObject> nametags = new List<GameObject>();
    

    public override void OnStartClient()
    {
        if(hasAuthority){
            nametag.SetActive(false);
            return;
        }

        nametags.Add(nametag);

        foreach(GameObject item in itemsToDisable)
        {
            item.SetActive(false);
        }

        cam.enabled = false;
        hook.layer = 0;
        hook.transform.GetChild(0).gameObject.layer = 0;
        foreach(Transform child in hook.transform.GetChild(0))
        {
            child.gameObject.layer = 0;
        }
        
        awp.layer = 0;
        foreach(Transform child in awp.transform)
        {
            child.gameObject.layer = 0;
        }   
    }
}
