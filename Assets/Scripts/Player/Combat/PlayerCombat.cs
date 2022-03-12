using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Barji.Player.Cam;
using Barji.Player.Inputs;
using Barji.Player.Movement;
using Barji.Player.Sounds;

namespace Barji.Player.Combat
{
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] private Transform spear;
        private PlayerInput input;
        private PlayerMovement movement;
        [SerializeField]private PlayerSounds sounds;
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
            if (PlayerMovement.currentState == PlayerMovement.state.Grappling || PlayerMovement.currentState == PlayerMovement.state.Gliding)
                return;

            if (Input.GetKeyDown(KeyCode.LeftShift) && canLunge)
            {
                Lunge();
            }
        }

        void Lunge()
        {
            movement.rb.AddForce(movement.orientation.forward * lungeForce, ForceMode.VelocityChange);
            movement.rb.AddForce(Vector3.up * lungeForce / 10, ForceMode.VelocityChange);
            movement.maxVelocity += 4;
            camController.ShakeLunge();
            LungeAnimation();
            sounds.PlayLunge();
            canLunge = false;
        }

        private void LungeAnimation()
        {
            //spear.localPosition = new Vector3(spear.localPosition.x, 0, spear.position.z);
            float spearPosY = spear.transform.localPosition.y;
            spear.transform.DOPunchPosition(Vector3.up, 0.1f);
        }
    }
}
