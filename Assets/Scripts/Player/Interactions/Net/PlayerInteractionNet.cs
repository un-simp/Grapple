using Mirror;
using UnityEngine;
using Wildflare.Player.Combat;

namespace Wildflare.Player.Interactions.Net
{
    public class PlayerInteractionNet : NetworkBehaviour
    {
        [SerializeField] private Material green;
        [SerializeField] private Material orange;
        [SerializeField] private Material red;

        [field: SyncVar(hook = nameof(HandleHealthChange))]
        public int health_gs { get; set; } = 3;

        private void HandleHealthChange(int oldHealth, int newHealth)
        {
            ChangeColor(newHealth);

            if (newHealth == 0)
            {
                CmdResetHealth();
                //GetComponent<Grapple>().Gra[[();
                transform.position = new Vector3(0, 10, 0);
            }
        }

        private void ChangeColor(int _newHealth)
        {
            if (_newHealth == 3) transform.GetChild(0).GetComponent<Renderer>().material = green;
            else if (_newHealth == 2) transform.GetChild(0).GetComponent<Renderer>().material = orange;
            else if (_newHealth == 1) transform.GetChild(0).GetComponent<Renderer>().material = red;
        }

        [Command]
        private void CmdResetHealth()
        {
            health_gs = 3;
        }
    }
}