using System;
using DG.Tweening;
using UnityEngine;
using Wildflare.Player.Cam;
using Wildflare.Player.Graphics;
using Wildflare.Player.Inputs;
using Wildflare.UI.MenuStates;

namespace Wildflare.Player.Movement
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerMovement : MonoBehaviour
    {
        //Assignables
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask ground;
        private PlayerGraphics graphics;
        private PlayerInput input;
        private CameraController camController;
        private Camera overlayCam;
        public Transform orientation;
        public Camera cam;
        public Rigidbody rb;


        //State
        [SerializeField] private bool debug;
        private bool readyToJump = true;
        public bool canMove = true;

        //Affectors
        [SerializeField] private float acceleration = 1;
        [SerializeField] private float absMaxVel = 40;
        [SerializeField] private float drag = 0.2f;
        [SerializeField] private int jumpForce = 400;
        [SerializeField] private int gravityMultiplier = 12;
        [SerializeField] private float minGroundRange = 0.1f;
        public float speed = 50;
        public float maxVelocity = 12;
        public float upthrust = 40;
        public float lowerVelocityThresholdMultiplier = 0.9f;

        //Stores
        private float FOV;
        private Vector3 wallTangentDir;
        [HideInInspector] public Vector3 wallNormal;
        private Vector3 velocityWhenLanded;

        //On Starts
        [HideInInspector] public float speedOnStart;
        [HideInInspector] public float maxVelocityOnStart;

        //Properties
        public bool isMoving { get; set; }
        public float currentVelocity { get; private set; }
        public bool isGrounded { get; private set; }
        
        //States
        public enum state
        {
            Walking, Sliding, Wallrunning, Airborn, Gliding, Grappling
        }

        [HideInInspector]public state currentState = state.Airborn;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            input = GetComponent<PlayerInput>();
            camController = GetComponent<CameraController>();
            graphics = GetComponent<PlayerGraphics>();
            overlayCam = cam.transform.GetChild(0).GetComponent<Camera>();
            maxVelocityOnStart = maxVelocity;
            speedOnStart = speed;
            FOV = cam.fieldOfView;
            canMove = true;
        }

        private void FixedUpdate()
        {
            print("Current state: " + currentState);
            
            if(!canMove) return;

            switch (currentState)
            {
                case state.Walking:
                    Walking();
                    JumpHandler();
                    Gravity();
                    if(!isMoving)
                        CounterMovement();
                    break;
                case state.Airborn:
                    AirbornMovement();
                    Gravity();
                    break;
                case state.Wallrunning:
                    WallRun();
                    JumpHandler();
                    break;
                case state.Grappling:
                    AirbornMovement();
                    Gravity();
                    break;
                case state.Sliding:
                    break;
            }

            //State Independant
            ClampVelocity();
        }

        #region Movement

        private void Walking()
        {
            //On Ground
            maxVelocity = Mathf.Lerp(maxVelocity, maxVelocityOnStart, Time.deltaTime * 3);
            HandleFOV(FOV);
            Movement(1, 1);
        }

        private void AirbornMovement()
        {
            HandleFOV(FOV * 1.1f);
            Movement(0.75f, 0.75f);
        }

        void Movement(float hMultiplier, float vMultiplier)
        {
            acceleration = currentVelocity / maxVelocity;
            
            //-0.5 due to the lerping never reaching maxVelocity.
            if (currentState == state.Airborn && currentVelocity >= maxVelocity - 0.5f)
                maxVelocity += Time.deltaTime * acceleration;
            else if (currentVelocity <= maxVelocity * lowerVelocityThresholdMultiplier) 
                maxVelocity -= Time.deltaTime * 10 * acceleration;

            maxVelocity = Mathf.Clamp(maxVelocity, maxVelocityOnStart, absMaxVel);
            var dir = (orientation.forward * input.yInput * vMultiplier + orientation.right * input.xInput * hMultiplier).normalized;
            rb.AddForce(dir * speed);
        }

        private void CounterMovement()
        {
            Vector3 vel = rb.velocity;
            vel.y = 0;
            rb.AddForce(-vel * drag);
        }

        private void ClampVelocity()
        {
            //Old clamping code

            //Vector3 tempVector = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            ////tempVector = Vector3.ClampMagnitude(tempVector, maxVelocity);
            //tempVector.y = rb.velocity.y;

            //New Clamping Code

            var noUpVec = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (noUpVec.magnitude > maxVelocity)
            {
                noUpVec *= 0.9f;
                noUpVec.y = rb.velocity.y;
                rb.velocity = noUpVec;
            }

            currentVelocity = rb.velocity.magnitude;
        }

        private void Gravity()
        {
            rb.AddForce(Vector3.down * gravityMultiplier);
        }

        #endregion
        
        #region Jumping

        private void JumpHandler()
        {
            if (currentState == state.Walking && readyToJump && input.jumping) Jump();

            if ((currentState == state.Wallrunning) && input.jumping) JumpWallrun();
        }

        private void Jump()
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            readyToJump = false;
        }

        private void JumpWallrun()
        {
            rb.AddForce(wallNormal * jumpForce * 0.5f, ForceMode.VelocityChange);
            rb.AddForce(orientation.forward * jumpForce * 0.2f, ForceMode.VelocityChange);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }

        #endregion
        
        #region Wallrun

        private void StartWallrun(Collision other)
        {
            if (wallNormal == other.GetContact(0).normal) return;
            wallNormal = other.GetContact(0).normal;
            var wallTangent = Vector3.Cross(wallNormal, Vector3.up).normalized;
            wallTangentDir = Vector3.Dot(orientation.forward, wallTangent) < 0 ? -wallTangent : wallTangent;

            var dot = Vector3.Dot(wallNormal, orientation.forward);
            if (currentState != state.Walking)
                if (dot < 0.7f)
                {
                    currentState = state.Wallrunning;
                    graphics.SpawnWallImpact(other.GetContact(0).point + wallNormal * 0.2f,
                        other.transform.GetComponent<Renderer>().material, wallNormal);
                    if (Vector3.Dot(orientation.forward, wallTangent) < 0)
                    {
                        camController.TweenTargetRot(15);
                        return;
                    }

                    if (wallNormal - Vector3.up != Vector3.zero)
                        camController.TweenTargetRot(-15);
                    return;
                }
            camController.TweenTargetRot(0);
            //currentState = state.Walking;
        }

        private void WallRun()
        {
            var noY = rb.velocity;
            noY.y = Mathf.Clamp(noY.y, -2f, 2f);
            rb.velocity = noY;

            rb.AddForce(Vector3.down);
            rb.AddForce(wallTangentDir * currentVelocity);
            
            //If looking away from wall and press W, drop off
            //0.5 is 45degrees from the wall
            if (currentState == state.Wallrunning && Vector3.Dot(orientation.forward, wallNormal) > 0.7f && input.yInput > 0)
                rb.AddForce(wallNormal * jumpForce * 0.5f, ForceMode.VelocityChange);
        }

        #endregion
        
        #region Collisions

        private void OnCollisionEnter(Collision other)
        {
            if(currentState == state.Grappling) return;
            
            bool isWall = other.GetContact(0).normal != Vector3.up;
            if (!isWall)
            {
                isGrounded = true;
                currentState = state.Walking;
                wallNormal = Vector3.zero;
                graphics.SpawnGroundImpact(groundCheck.position, other.transform.GetComponent<Renderer>().material);
                camController.ShakeGroundImpact();
                camController.TweenTargetRot(0);
            }
            else
                StartWallrun(other);
        }


        private void OnCollisionStay(Collision other)
        {
            //Slide Down Wall
            var isWall = other.GetContact(0).normal != Vector3.up;
            if (isWall && currentState == state.Airborn)
            {
                rb.AddForce(Vector3.down * 100);
                rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -10, 0), rb.velocity.z);
            }

            if (!isWall && currentState != state.Walking && currentState != state.Grappling)
            {
                isGrounded = true;
                currentState = state.Walking;
                wallNormal = Vector3.zero;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            //6 = Ground, 8 = Swingable
            if (other.gameObject.layer != 6 && other.gameObject.layer != 8) return;

            //Jumped
            if (currentState == state.Walking && isGrounded)
            {
                currentState = state.Airborn;
                isGrounded = false;
                readyToJump = true;
            }

            //Stop wallrunning
            if (currentState == state.Wallrunning)
            {
                currentState = state.Airborn;
                camController.TweenTargetRot(0);
            }
        }

        #endregion
        
        #region Other

        private void OnDrawGizmos()
        {
            if (!debug) return;
            var groundCheckCol = isGrounded ? Color.green : Color.red;
            Debug.DrawRay(groundCheck.position, Vector3.down * minGroundRange, groundCheckCol);
            
            Debug.DrawLine(groundCheck.position, (groundCheck.position - rb.velocity) * Mathf.Infinity, Color.green);
            Debug.DrawLine(groundCheck.position, (groundCheck.position - transform.InverseTransformDirection(rb.velocity)) * Mathf.Infinity, Color.red);
        }

        public void HandleFOV(float _desiredFOV)
        {
            cam.DOFieldOfView(_desiredFOV, 0.5f);
            overlayCam.DOFieldOfView(_desiredFOV, 0.5f);
        }

        #endregion
    }
}