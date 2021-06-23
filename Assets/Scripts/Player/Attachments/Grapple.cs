using System.Collections;
using UnityEngine;
using Wildflare.Player.Cam;
using Wildflare.Player.Graphics;
using Wildflare.Player.Movement;
using Wildflare.Player.Sounds;

namespace Wildflare.Player.Attachments
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
        private SpringJoint joint;
        private PlayerMovement movement;
        private Rigidbody rb;


        private void Awake()
        {
            movement = GetComponent<PlayerMovement>();
            graphics = GetComponent<GrappleGraphics>();
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (!canGrapple) return;
            GrappleManager();
        }

        private void FixedUpdate()
        {
            if (!canGrapple) return;
            if (movement.currentState != PlayerMovement.state.Grappling) return;
            DoGrapple();
        }

        private void GrappleManager()
        {
            bool isGrappling = movement.currentState == PlayerMovement.state.Grappling;
            if (Input.GetKey(KeyCode.Mouse0) && !isGrappling && CanGrapple())
            {
                movement.currentState = PlayerMovement.state.Grappling;
                InstantStart();
            }
                
            else if (Input.GetKeyUp(KeyCode.Mouse0) && isGrappling)
            {
                InstantStop();
                if (movement.rayHitGround)
                    movement.currentState = PlayerMovement.state.Walking;
                else
                    movement.currentState = PlayerMovement.state.Airborn;
            }
            
            if(GetComponent<SpringJoint>() && movement.currentState != PlayerMovement.state.Grappling)
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
            GetComponent<CameraController>().TweenTargetRot(0);
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
            if(GetComponent<SpringJoint>() && movement.currentState != PlayerMovement.state.Grappling)
                Destroy(GetComponent<SpringJoint>());
            graphics.StopGrapple();
        }

        public bool CanGrapple()
        {
            RaycastHit hit;
            var Raycast = Physics.Raycast(movement.cam.transform.position, movement.cam.transform.forward, out hit, grappleRange);
            bool isGrappling = movement.currentState == PlayerMovement.state.Grappling;
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