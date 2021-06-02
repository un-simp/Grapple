using System;
using UnityEngine;
using Wildflare.Player.Movement;
using DG.Tweening;

namespace Wildflare.Player.Cam
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]private Vector2 sensitivity;
        [SerializeField]private Camera playerCam;
        private PlayerMovement movement;
        public Transform orientation;

        float desiredX;
        float xRotation;

        bool cursorLocked;

        float mouseX, mouseY;

        private float targetRot;
        [Range(0.0001f, 10f)][SerializeField] private float damper;

        void Awake()
        {
            movement = GetComponent<PlayerMovement>();
        }

        public void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            cursorLocked = true;
        }

        void Update(){
            if(!movement.isActive) return;

            mouseX = Input.GetAxis("Mouse X") * sensitivity.x * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * sensitivity.y * Time.deltaTime;
        }

        void LateUpdate(){
            if(!movement.isActive) return;

            //Find current look rotation
            Vector3 rot = playerCam.transform.localRotation.eulerAngles;
            desiredX = rot.y + mouseX;
            
            //Rotate, and also make sure we dont over- or under-rotate.
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            //Perform the rotations
            playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, targetRot);
            orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);

            if(Input.GetKeyDown(KeyCode.Escape)){
                cursorLocked = !cursorLocked;
                if(cursorLocked){
                    Cursor.lockState = CursorLockMode.Locked;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                }
            }
        }

        public void TweenTargetRot(float _endRot) {
            DOTween.To(() => targetRot, x => targetRot = x, _endRot, damper).SetEase(Ease.OutExpo);
        }

    }
}

