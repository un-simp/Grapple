using UnityEngine;
using Mirror;
using Wildflare.Player.Movement.Net;

namespace Wildflare.Player.Inputs.Net
{
    public class PlayerInputNet : NetworkBehaviour
    {
        [HideInInspector]public float xInput, yInput;
        [HideInInspector]public float mouseX, mouseY;

        [HideInInspector]public bool jumping;
        PlayerMovementNet movement;

        void Awake()
        {
            movement = GetComponent<PlayerMovementNet>();
        }

        void Update()
        {
            if(!movement.hasAuthority) return;
            InputsHandler();
        }

        public void InputsHandler()
        {
            xInput = Input.GetAxisRaw("Horizontal");
            yInput = Input.GetAxisRaw("Vertical");

            mouseX = Input.GetAxisRaw("Mouse X");
            mouseY = Input.GetAxisRaw("Mouse Y");

            jumping = Input.GetButton("Jump");
        }
    }
}