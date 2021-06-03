using System.Collections;
using Mirror;
using UnityEngine;
using Wildflare.Player.Inputs.Net;
using Wildflare.Player.Interactions.Net;
using Wildflare.Player.Movement.Net;

namespace Wildflare.Player.Combat.Net
{
    public class WeaponNet : NetworkBehaviour
    {
        [SerializeField] private Transform cam;
        private bool canShoot = true;
        private PlayerCombatNet combat;
        private readonly float delay = 0.35f;

        private RaycastHit hit;
        private PlayerInputNet inputs;
        private bool inTransition;
        private PlayerMovementNet movement;

        private void Awake()
        {
            movement = transform.root.GetComponent<PlayerMovementNet>();
            inputs = transform.root.GetComponent<PlayerInputNet>();
            combat = GetComponent<PlayerCombatNet>();
        }

        private void FixedUpdate()
        {
            if (!movement.hasAuthority) return;

            var hitSomething = Physics.Raycast(cam.position, cam.forward, out hit, Mathf.Infinity);

            if (Input.GetKey(KeyCode.Mouse0) && canShoot && !inTransition)
            {
                try
                {
                    if (hit.transform.TryGetComponent(out PlayerInteractionNet __interaction) && hitSomething)
                        Damage(__interaction);
                }
                catch { }

                combat.currentActiveWeapon.Shoot(hit);
                StartCoroutine(Delay());
            }
        }

        public IEnumerator OnSelect()
        {
            inTransition = true;
            yield return new WaitForSeconds(0.55f);
            inTransition = false;
        }


        private void Damage(PlayerInteractionNet __interaction)
        {
            CmdUpdateHealth(__interaction);
        }

        private IEnumerator Delay()
        {
            canShoot = false;
            yield return new WaitForSeconds(delay);
            canShoot = true;
        }

        [Command]
        private void CmdUpdateHealth(PlayerInteractionNet __interaction)
        {
            __interaction.health_gs -= 1;
        }
    }
}