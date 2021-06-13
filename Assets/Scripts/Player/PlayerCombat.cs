using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Wildflare.Player.Cam;
using Wildflare.Player.Inputs;
using Wildflare.Player.Movement;

namespace Wildflare
{
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] private Transform spear;
        private PlayerInput input;
        private PlayerMovement movement;
        private CameraController camController;

        [SerializeField] private float lungeForce;
        public bool canLunge = true;

        void Awake()
        {
            camController = GetComponent<CameraController>();
            input = GetComponent<PlayerInput>();
            movement = GetComponent<PlayerMovement>();
        }

        void Update()
        {
            //If gliding or grappling then we can't lunge
            if (movement.currentState == PlayerMovement.state.Grappling || movement.currentState == PlayerMovement.state.Gliding)
            {
                spear.localPosition = Vector3.zero;
                return;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && canLunge)
            {
                Lunge();
            }
        }

        void Lunge()
        {
            movement.rb.AddForce(movement.orientation.forward * lungeForce, ForceMode.VelocityChange);
            movement.rb.AddForce(Vector3.up * lungeForce / 10, ForceMode.VelocityChange);
            movement.maxVelocity += 2;
            camController.ShakeLunge();
            LungeAnimation();
            canLunge = false;
        }

        private void LungeAnimation()
        {
            //spear.localPosition = new Vector3(spear.localPosition.x, 0, spear.position.z);
            float spearPosY = spear.transform.localPosition.y;
            var tween = spear.transform.DOLocalMoveY(spearPosY + 0.5f, 0.1f);
        }
    }
}
