using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wildflare.Player.Movement;
using Random = UnityEngine.Random;

namespace Wildflare
{
    public class PlayerSounds : MonoBehaviour
    {
        private PlayerMovement movement;
        
        [Header("Footsteps")]
        [SerializeField] private AudioSource footstepsAS;
        [SerializeField] private AudioClip[] footstepSounds;
        [Header("Landed")]
        [SerializeField] private AudioSource landedAS;
        [SerializeField] private AudioClip landedSound;
        [Header("Wind")] 
        [SerializeField] private AudioSource windAS;

        private void Awake()
        {
            movement = transform.root.GetComponent<PlayerMovement>();
            movement.OnFootstep += PlayFootstepSound;
            movement.OnLanded += PlayLandedSound;
        }

        void Update()
        {
            if (movement.currentState == PlayerMovement.state.Walking)
            {
                windAS.volume = 0;
                return;
            }
            windAS.volume = (movement.currentVelocity - 12) / movement.absMaxVel * 0.05f;
        }

        public void PlayFootstepSound()
        {
            if(footstepSounds.Length == 0) return;
            //Find a random sound in the array
            AudioClip randomSound = footstepSounds[Random.Range(0, footstepSounds.Length)];
            footstepsAS.PlayOneShot(randomSound);
        }

        public void PlayLandedSound()
        {
            landedAS.PlayOneShot(landedSound);
        }
        
        
    }
}
