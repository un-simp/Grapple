using UnityEngine;
using Wildflare.Player.Inputs.Net;

namespace Wildflare.Player.Graphics.Net
{
    public class SwayNet : MonoBehaviour
    {
        public float intensity = 20;
        public float damper = 10;

        private PlayerInputNet input;
        private Quaternion originRot;

        public void Awake()
        {
            originRot = transform.localRotation;
            input = transform.root.GetComponentInChildren<PlayerInputNet>();
        }

        private void Update()
        {
            ApplySway();
        }

        private void ApplySway()
        {
            //Inputs
            var mouseX = input.mouseX;
            var mouseY = input.mouseY;

            //Calculate Target Rotation
            var adjustmentX = Quaternion.AngleAxis(intensity * -mouseX, Vector3.up);
            var adjustmentY = Quaternion.AngleAxis(intensity * mouseY, Vector3.right);
            var targetRot = originRot * adjustmentX * adjustmentY;

            //Rotates toward target rotation
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, Time.deltaTime * damper);
        }
    }
}