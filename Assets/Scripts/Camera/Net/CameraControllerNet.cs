using Mirror;
using UnityEngine;
using Wildflare.Player.Movement.Net;

namespace Wildflare.Player.Cam.Net
{
    public class CameraControllerNet : NetworkBehaviour
    {
        public Vector2 sensitivity;
        public Camera playerCam;

        public Transform orientation;

        private bool cursorLocked;

        private float desiredX;

        private float mouseX, mouseY;
        private PlayerMovementNet movement;
        private float xRotation;

        private void Awake()
        {
            movement = GetComponent<PlayerMovementNet>();
        }

        private void Update()
        {
            if (!movement.isActive) return;
            if (!hasAuthority) return;

            mouseX = Input.GetAxis("Mouse X") * sensitivity.x * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * sensitivity.y * Time.deltaTime;
        }

        private void LateUpdate()
        {
            if (!movement.isActive) return;
            if (!hasAuthority) return;

            //Find current look rotation
            var rot = playerCam.transform.localRotation.eulerAngles;
            desiredX = rot.y + mouseX;

            //Rotate, and also make sure we dont over- or under-rotate.
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            //Perform the rotations
            playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
            orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                cursorLocked = !cursorLocked;
                if (cursorLocked)
                    Cursor.lockState = CursorLockMode.Locked;
                else
                    Cursor.lockState = CursorLockMode.None;
            }
        }

        public override void OnStartLocalPlayer()
        {
            if (hasAuthority)
            {
                Cursor.lockState = CursorLockMode.Locked;
                cursorLocked = true;
            }
        }
    }
}