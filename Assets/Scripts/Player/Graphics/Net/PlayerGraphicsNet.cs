using UnityEngine;
using Mirror;
using TMPro;
using Wildflare.Player.Movement.Net;

namespace Wildflare.Player.Graphics.Net
{
    public class PlayerGraphicsNet : NetworkBehaviour
    {
        public TMP_Text velocityTxt;
        PlayerMovementNet movement;

        void Awake()
        {
            movement = GetComponent<PlayerMovementNet>();
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

