using UnityEngine;
using Barji.Player.Movement;
using Valve.VR;

namespace Barji.Player.Inputs
{
    public class PlayerInput : MonoBehaviour
    {
        [HideInInspector] public float xInput, yInput;
        [HideInInspector] public float mouseX, mouseY;

        [HideInInspector] public bool jumping;
        [HideInInspector] public bool grappling;
        [HideInInspector] public bool stopGrapple;
        [HideInInspector] public bool squeeze;
        private PlayerMovement movement;

        [HideInInspector]public bool switchLeft;
        [HideInInspector]public bool switchRight;

        [SerializeField] private SteamVR_Action_Vector2 touchpad;
        [SerializeField] private SteamVR_Action_Boolean jump;
        [SerializeField] private SteamVR_Action_Boolean triggerRight;
        [SerializeField] private SteamVR_Action_Boolean triggerLeft;
        [SerializeField] private SteamVR_Action_Boolean squeezer;

        public bool isVR;

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
            if(isVR)
            {
                xInput = touchpad.axis.x;
                yInput = touchpad.axis.y;
                jumping = jump.state;

                grappling = triggerRight.state || triggerLeft.state;

                stopGrapple = triggerRight.stateUp || triggerLeft.stateUp;

                switchLeft = triggerLeft.stateDown;
                switchRight = triggerRight.stateDown;

                squeeze = squeezer.state;
            }
            else
            {
                xInput = Input.GetAxis("Horizontal");
                yInput = Input.GetAxis("Vertical");
                mouseX = Input.GetAxisRaw("Mouse X");
                mouseY = Input.GetAxisRaw("Mouse Y");
                jumping = Input.GetButton("Jump");
                grappling = Input.GetKey(KeyCode.Mouse0);
                stopGrapple = Input.GetKeyUp(KeyCode.Mouse0);
            }

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