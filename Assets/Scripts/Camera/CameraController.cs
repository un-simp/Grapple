using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Wildflare.Player
{
    public class CameraController : NetworkBehaviour
    {
        public Vector2 sensitivity;
        private PlayerMovement movement;
        public Camera playerCam;

        public Transform orientation;

        float desiredX;
        float xRotation;

        bool cursorLocked;

        float mouseX, mouseY;

        void Awake()
        {
            movement = GetComponent<PlayerMovement>();
        }

        public override void OnStartLocalPlayer()
        {
            if(hasAuthority){
                Cursor.lockState = CursorLockMode.Locked;
                cursorLocked = true;
            }
        }

        void Update(){
            if(!movement.isActive) return;
            if(!hasAuthority) return;

            mouseX = Input.GetAxis("Mouse X") * sensitivity.x * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * sensitivity.y * Time.deltaTime;
        }

        void LateUpdate(){
            if(!movement.isActive) return;
            if(!hasAuthority) return;

            //Find current look rotation
            Vector3 rot = playerCam.transform.localRotation.eulerAngles;
            desiredX = rot.y + mouseX;
            
            //Rotate, and also make sure we dont over- or under-rotate.
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            //Perform the rotations
            playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
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

    } 
}

