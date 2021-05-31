using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;

namespace Wildflare.Player.Inputs
{
    [RequireComponent(typeof(PlayerMovement))]
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