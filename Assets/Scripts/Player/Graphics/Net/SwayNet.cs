using UnityEngine;
using Wildflare.Player.Inputs.Net;

namespace Wildflare.Player.Graphics.Net {
    public class SwayNet : MonoBehaviour
    {
        public float intensity = 20;
        public float damper = 10;
        private Quaternion originRot;

        PlayerInputNet input;

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
            float mouseX = input.mouseX;
            float mouseY = input.mouseY;

            //Calculate Target Rotation
            Quaternion adjustmentX = Quaternion.AngleAxis(intensity * -mouseX, Vector3.up);
            Quaternion adjustmentY = Quaternion.AngleAxis(intensity * mouseY, Vector3.right);
            Quaternion targetRot = originRot * adjustmentX * adjustmentY;

            //Rotates toward target rotation
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, Time.deltaTime * damper);
        }

    }
}

