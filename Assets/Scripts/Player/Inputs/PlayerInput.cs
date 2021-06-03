using UnityEngine;
using Wildflare.Player.Movement;

namespace Wildflare.Player.Inputs
{
    public class PlayerInput : MonoBehaviour
    {
        [HideInInspector] public float xInput, yInput;
        [HideInInspector] public float mouseX, mouseY;

        [HideInInspector] public bool jumping;
        private PlayerMovement movement;

        private void Awake()
        {
            movement = GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            InputsHandler();
        }

        public void InputsHandler()
        {
            xInput = Input.GetAxisRaw("Horizontal");
            yInput = Input.GetAxisRaw("Vertical");

            mouseX = Input.GetAxisRaw("Mouse X");
            mouseY = Input.GetAxisRaw("Mouse Y");

            jumping = Input.GetButton("Jump");

            if (xInput == 0 && yInput == 0)
            {
                movement.isMoving = false;
            }
            else
            {
                movement.isMoving = true;
            }
        }
    }
}