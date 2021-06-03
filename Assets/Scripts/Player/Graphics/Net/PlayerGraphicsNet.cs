using Mirror;
using TMPro;
using Wildflare.Player.Movement.Net;

namespace Wildflare.Player.Graphics.Net
{
    public class PlayerGraphicsNet : NetworkBehaviour
    {
        public TMP_Text velocityTxt;
        private PlayerMovementNet movement;

        private void Awake()
        {
            movement = GetComponent<PlayerMovementNet>();
        }

        private void Update()
        {
            if (!hasAuthority) return;

            velocityTxt.text = movement.currentVelocity.ToString("F2") + "U/S";

            foreach (var nametag in AuthorityCleanup.nametags)
            {
                if (nametag == null) AuthorityCleanup.nametags.Remove(nametag);
                nametag.transform.forward = -(transform.position - nametag.transform.position).normalized;
            }
        }
    }
}