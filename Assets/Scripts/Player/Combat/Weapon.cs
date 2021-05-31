using System;
using System.Collections;
using UnityEngine;
using Mirror;
using Wildflare.Player.Interactions;
using Wildflare.Player.Inputs;

namespace Wildflare.Player.Combat
{
    public class Weapon : NetworkBehaviour
    {
        float delay = 0.35f;
        bool canShoot = true;
    
        RaycastHit hit;
        PlayerMovement movement;
        PlayerInput inputs;
        PlayerCombat combat;
    
        [SerializeField] Transform cam;
    
        void Awake() 
        {
            movement = transform.root.GetComponent<PlayerMovement>();
            inputs = transform.root.GetComponent<PlayerInput>();
            combat = GetComponent<PlayerCombat>();
        }
    
        void FixedUpdate()
        {
            if(!movement.hasAuthority) return;
    
            bool hitSomething = Physics.Raycast(cam.position, cam.forward, out hit, Mathf.Infinity);

            if(Input.GetKey(KeyCode.Mouse0) && canShoot)
            {
                try{
                    if(hit.transform.TryGetComponent(out PlayerInteraction __interaction) && hitSomething)
                    {
                        Damage(__interaction);
                    }     
                }
                catch{}
                combat.currentActiveWeapon.Shoot(hit);
                StartCoroutine(Delay());
            }
        }
    
    
        void Damage(PlayerInteraction __interaction)
        {
            CmdUpdateHealth(__interaction);
        }
    
        IEnumerator Delay()
        {
            canShoot = false;
            yield return new WaitForSeconds(delay);
            canShoot = true;
        }
    
        [Command] void CmdUpdateHealth(PlayerInteraction __interaction) => __interaction.health_gs -= 1;

        
    }
}

