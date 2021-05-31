using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

namespace Wildflare.Player.Graphics
{
    public class PlayerGraphics : NetworkBehaviour
    {
        public TMP_Text velocityTxt;
        PlayerMovement movement;

        void Awake()
        {
            movement = GetComponent<PlayerMovement>();
        }
    
        void Update()
        {
            if(!hasAuthority) return;

            velocityTxt.text = movement.currentVelocity.ToString("F2") + "U/S";

            foreach(GameObject nametag in AuthorityCleanup.nametags)
            {
                if(nametag == null){
                    AuthorityCleanup.nametags.Remove(nametag);
                }
                nametag.transform.forward = -(transform.position - nametag.transform.position).normalized;
            }
        }
    }
}

