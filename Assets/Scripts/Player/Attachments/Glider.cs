using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wildflare.Player.Graphics;
using Wildflare.Player.Movement;

namespace Wildflare.Player.Attachments
{
    public class Glider : MonoBehaviour
    {
        //Logic
        public float glideDuration;
        [HideInInspector]public float currentGlideTime;
        private Rigidbody rb;
        private PlayerMovement movement;
        private GliderGraphics graphics;
        private bool canGlide;
        public bool IsGliding { get; set; }
        
        private void Awake()
        {
            movement = GetComponent<PlayerMovement>();
            rb = GetComponent<Rigidbody>();
            graphics = GetComponent<GliderGraphics>();
        }

        private void Update()
        {
            if (!canGlide)
            {
                currentGlideTime += Time.deltaTime;
                if (currentGlideTime >= glideDuration)
                {
                    currentGlideTime = glideDuration;
                    canGlide = true;
                }
                return;
            }
            
            //Start gliding if we press mouse 1 in the air.
            if (movement.currentState == PlayerMovement.state.Airborn)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    StartGliding();
                }
            }
            
            //Stop Gliding If we release mouse 1
            if (movement.currentState == PlayerMovement.state.Gliding)
            {
                if (Input.GetMouseButtonUp(1))
                {
                    StopGliding();
                }
            }
        }

        private void FixedUpdate()
        {
            if(movement.currentState != PlayerMovement.state.Gliding) return;
            if (currentGlideTime > 0)
            {
                DoGliding();
                currentGlideTime -= Time.deltaTime;
            }
            else if(canGlide)
            {
                StopGliding();
            }
        }

        void StartGliding()
        {
            IsGliding = true;
            movement.currentState = PlayerMovement.state.Gliding;
            graphics.StartGliding();
        }

        void DoGliding()
        {
            rb.AddForce(Vector3.up * 0.1f, ForceMode.VelocityChange);
            rb.AddForce(movement.orientation.forward * 0.5f, ForceMode.VelocityChange);
        }

        public void StopGliding()
        {
            IsGliding = false;
            canGlide = false;
            currentGlideTime = 0;
            graphics.StopGliding();
            if(movement.currentState == PlayerMovement.state.Gliding)
                movement.currentState = PlayerMovement.state.Airborn;
        }

        private void OnCollisionEnter(Collision other)
        {
            if(canGlide && currentGlideTime != glideDuration)
                StopGliding();
        }
    }
}
