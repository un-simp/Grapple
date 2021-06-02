using UnityEngine;
using Mirror;
using Wildflare.Player.Movement;

namespace Wildflare.Player.Inputs
{
    public class PlayerInput : MonoBehaviour
    {
        [HideInInspector]public float xInput, yInput;
        [HideInInspector]public float mouseX, mouseY;

        [HideInInspector]public bool jumping;
        PlayerMovement movement;

        void Awake()
        {
            movement = GetComponent<PlayerMovement>();
        }

        void Update()
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
        }
    }
}