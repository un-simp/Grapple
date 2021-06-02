using System.Collections;
using UnityEngine;
using Mirror;
using Wildflare.Player.Interactions;
using Wildflare.Player.Inputs.Net;
using Wildflare.Player.Interactions.Net;
using Wildflare.Player.Movement.Net;


namespace Wildflare.Player.Combat.Net
{
    public class WeaponNet : NetworkBehaviour
    {
        float delay = 0.35f;
        bool canShoot = true;
        private bool inTransition = false;
    
        RaycastHit hit;
        PlayerMovementNet movement;
        PlayerInputNet inputs;
        PlayerCombatNet combat;
    
        [SerializeField] Transform cam;
    
        void Awake() 
        {
            movement = transform.root.GetComponent<PlayerMovementNet>();
            inputs = transform.root.GetComponent<PlayerInputNet>();
            combat = GetComponent<PlayerCombatNet>();
        }

        public IEnumerator OnSelect() {
            inTransition = true;
            yield return new WaitForSeconds(0.55f);
            inTransition = false;
        }
    
        void FixedUpdate()
        {
            if(!movement.hasAuthority) return;
    
            bool hitSomething = Physics.Raycast(cam.position, cam.forward, out hit, Mathf.Infinity);

            if(Input.GetKey(KeyCode.Mouse0) && canShoot && !inTransition)
            {
                try{
                    if(hit.transform.TryGetComponent(out PlayerInteractionNet __interaction) && hitSomething)
                    {
                        Damage(__interaction);
                    }     
                }
                catch{}
                combat.currentActiveWeapon.Shoot(hit);
                StartCoroutine(Delay());
            }
        }
    
    
        void Damage(PlayerInteractionNet __interaction)
        {
            CmdUpdateHealth(__interaction);
        }
    
        IEnumerator Delay()
        {
            canShoot = false;
            yield return new WaitForSeconds(delay);
            canShoot = true;
        }
    
        [Command] void CmdUpdateHealth(PlayerInteractionNet __interaction) => __interaction.health_gs -= 1;

        
    }
}

