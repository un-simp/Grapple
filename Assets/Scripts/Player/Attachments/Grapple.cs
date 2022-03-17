using System.Collections;
using UnityEngine;
using Barji.Player.Cam;
using Barji.Player.Graphics;
using Barji.Player.Movement;
using Barji.Player.Sounds;
using Barji.Player.Inputs;

namespace Barji.Player.Attachments
{
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(GrappleGraphics))]
    public class Grapple : MonoBehaviour
    {
        public float grappleRange = 100;
        public Vector3 hitPos;
        public LayerMask swingable;

        [Header("Physics Feel")] 
        [Tooltip("Amount that the spring is reduced when active.")] public float damper = 2;

        [Tooltip("Strength of the spring.")] public float springiness = 5;

        public float mass = 5;
        private bool canGrapple = true;
        private GrappleGraphics graphics;
        public RaycastHit hitInfo;

        [SerializeField] private PlayerSounds sounds;
        [SerializeField] private PlayerInput input;
        private SpringJoint joint;
        private PlayerMovement movement;
        private Rigidbody rb;

        [Header("VR")]
        [SerializeField] private Transform activeHand;
        [SerializeField] private Transform leftHand;
        [SerializeField] private Transform rightHand;


        private void Awake()
        {
            movement = GetComponent<PlayerMovement>();
            graphics = GetComponent<GrappleGraphics>();
            rb = GetComponent<Rigidbody>();
        
        }

        private void Start() 
        {
            if(movement.isVR)
                rightHand.GetChild(rightHand.childCount-1).gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!canGrapple) return;
            GrappleManager();
        }

        private void FixedUpdate()
        {
            if (!canGrapple) return;
            if (PlayerMovement.currentState != PlayerMovement.state.Grappling) return;
            DoGrapple();
        }

        private void GrappleManager()
        {
            bool isGrappling = PlayerMovement.currentState == PlayerMovement.state.Grappling;

            if(movement.isVR)
            {
                if(!isGrappling)
                {
                    if(input.switchRight)
                    {
                        activeHand.parent.parent = rightHand;
                        activeHand.parent.localPosition = Vector3.zero;
                        activeHand.parent.localEulerAngles = Vector3.zero;
                    }
                    else if(input.switchLeft)
                    {
                        activeHand.parent.parent = leftHand;
                        activeHand.parent.localPosition = Vector3.zero;
                        activeHand.parent.localEulerAngles = Vector3.zero;
                    }  
                }

                if(input.grappling && !isGrappling)
                    graphics.DrawGuide(activeHand.position, activeHand.position + (activeHand.forward * grappleRange), 1);
                else
                    graphics.DrawGuide(activeHand.position, activeHand.position + (activeHand.forward * grappleRange), 0);
                
            }
            

            if (input.grappling && !isGrappling && CanGrapple())
            {
                PlayerMovement.currentState = PlayerMovement.state.Grappling;
                InstantStart();
            }
                
            else if (input.stopGrapple && isGrappling)
            {
                InstantStop();
                if (movement.rayHitGround)
                    PlayerMovement.currentState = PlayerMovement.state.Walking;
                else
                    PlayerMovement.currentState = PlayerMovement.state.Airborn;
            }
            
            if(GetComponent<SpringJoint>() && PlayerMovement.currentState != PlayerMovement.state.Grappling)
                Destroy(GetComponent<SpringJoint>());
        }
        
        void InstantStart()
        {
            StartGrapple();
            movement.wallNormal = Vector3.zero;
        }
        public void InstantStop()
        {
            StopGrapple();
        }

        private void StartGrapple()
        {
            if(!movement.isVR)
            {
                GetComponent<CameraController>().TweenTargetRot(0);
                var glider = GetComponent<Glider>();
                if (glider.IsGliding)
                    glider.StopGliding();
            } //TODO: move glider code out
            
            joint = gameObject.AddComponent<SpringJoint>();

            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = hitPos;

            var distanceFromPoint = Vector3.Distance(transform.position, joint.connectedAnchor);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = springiness;
            joint.damper = damper;
            joint.massScale = mass;


            movement.speed = movement.speedOnStart / 3;
            if (movement.maxVelocity < movement.maxVelocityOnStart * 2)
                movement.maxVelocity = movement.maxVelocityOnStart * 2;

            graphics.StartGrapple(hitInfo);
            graphics.SpawnParticle(hitInfo);
            sounds.PlayGrappleThrow(joint.connectedAnchor);
        }

        private void DoGrapple()
        {
            if (joint == null) return;
            var vecToPlayer = (joint.connectedAnchor - transform.position);
            vecToPlayer.Normalize();
            rb.AddForce(vecToPlayer * movement.upthrust);
        }

        private void StopGrapple()
        {
            movement.speed = movement.speedOnStart;
            if(GetComponent<SpringJoint>() && PlayerMovement.currentState != PlayerMovement.state.Grappling)
                Destroy(GetComponent<SpringJoint>());
            graphics.StopGrapple();
            sounds.StopGrapple();
        }

        public bool CanGrapple()
        {
            RaycastHit hit;

            Vector3 rayOrigin;
            Vector3 rayDirection;

            if(movement.isVR)
            {
                rayOrigin = activeHand.position;
                rayDirection = activeHand.forward;
            }
            else
            {
                rayOrigin = movement.cam.transform.position;
                rayDirection = movement.cam.transform.forward;
            }

            var Raycast = Physics.Raycast(rayOrigin, rayDirection, out hit, grappleRange);
            bool isGrappling = PlayerMovement.currentState == PlayerMovement.state.Grappling;
            if (Raycast && !isGrappling)
            {
                if (hit.transform.gameObject.layer == 8)
                {
                    hitInfo = hit;
                    hitPos = hit.point;
                    return true;
                }

                return false;
            }

            return false;
        }
    }
}