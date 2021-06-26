using UnityEngine;
using Wildflare.Player.Inputs;

namespace Wildflare.Player.Graphics
{
    public class Sway : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float movementIntensity = 20;
        [SerializeField] private float movementDamper = 10;
        [SerializeField] private float movementmaxAmount;
        [Header("Rotation")]
        [SerializeField] private float rotationIntensity = 20;
        [SerializeField] private float rotationDamper = 10;
        private PlayerInput input;

        private Vector3 initialPos;
        private Quaternion initialRot;

        [SerializeField]private bool enableMovement = false;

        public void Awake()
        {
            initialRot = transform.localRotation;
            initialPos = transform.localPosition;

            input = transform.root.GetComponentInChildren<PlayerInput>();
        }

        private void Update()
        {
            RotSway();
            if(!enableMovement) return;
            MoveSway();
        }

        private void RotSway()
        {
            //Inputs
            var mouseX = input.mouseX;
            var mouseY = input.mouseY;

            //Calculate Target Rotation
            var adjustmentX = Quaternion.AngleAxis(rotationIntensity * -mouseX, Vector3.up);
            var adjustmentY = Quaternion.AngleAxis(rotationIntensity * mouseY, Vector3.right);
            var targetRot = initialRot * adjustmentX * adjustmentY;

            //Rotates toward target rotation
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, Time.fixedDeltaTime * rotationDamper);
        }

        private void MoveSway()
        {
            var mouseX = -input.mouseX * movementIntensity;
            var mouseY = -input.mouseY * movementIntensity;

            mouseX = Mathf.Clamp(mouseX, -movementmaxAmount, movementmaxAmount);
            mouseY = Mathf.Clamp(mouseY, -movementmaxAmount, movementmaxAmount);

            Vector3 finalPos = new Vector3(mouseX, mouseY, 0);
            transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos + initialPos, Time.fixedDeltaTime * movementDamper);
            
        }
    }
}