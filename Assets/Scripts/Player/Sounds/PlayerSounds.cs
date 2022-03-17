using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Barji.Player.Movement;
using Random = UnityEngine.Random;

namespace Barji.Player.Sounds
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

        [Header("Grapple")] 
        [SerializeField] private AudioSource grappleAS;
        [SerializeField] private AudioClip grappleThrow;
        [SerializeField] private AudioClip grappleImpact;
        [SerializeField] private AudioClip grappleStretch;

        [Header("Lunge")] [SerializeField] private AudioSource lungeAS;
        [SerializeField] private AudioClip lunge;

        private void Awake()
        {
            movement = transform.root.GetComponent<PlayerMovement>();
            movement.OnFootstep += PlayFootstepSound;
            movement.OnLanded += PlayLandedSound;
        }

        void Update()
        {
            if (PlayerMovement.currentState == PlayerMovement.state.Walking)
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
            footstepsAS.PlayOneShot(randomSound, 0.2f);
        }

        public void PlayGrappleThrow(Vector3 hitPoint)
        {
            grappleAS.PlayOneShot(grappleThrow, 1f);
            grappleAS.clip = grappleStretch;
            grappleAS.volume = 0.05f;
            grappleAS.Play();
            StartCoroutine(PlayGrappleHit(hitPoint));
        }

        public void StopGrapple()
        {
            grappleAS.Stop();
        }

        private IEnumerator PlayGrappleHit(Vector3 hitPoint)
        {
            yield return new WaitForSeconds(0.1f);
            AudioSource.PlayClipAtPoint(grappleImpact, hitPoint);
        }

        public void PlayLunge()
        {
            lungeAS.PlayOneShot(lunge, 0.2f);
        }

        public void PlayLandedSound()
        {
            landedAS.PlayOneShot(landedSound, 0.3f);
        }
        
        
    }
}
