using System;
using System.Collections;
using UnityEngine;
using Wildflare.Player.Inputs;
using DG.Tweening;
using TMPro;
using Wildflare.Player.Cam;
using Wildflare.Player.Graphics;

namespace Wildflare.Player.Movement
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerMovement : MonoBehaviour
    {
        //Assignables
        public Transform orientation;
        [SerializeField] private Transform groundCheck;
        [HideInInspector] public PlayerInput input;
        private CameraController camController;
        private PlayerGraphics graphics;
        public Camera cam;
        private Camera overlayCam;
        public Rigidbody rb;

        //State
        [SerializeField]private bool debug = false;
        [HideInInspector]public bool isActive = true;
        bool readyToJump = true;

        //Affectors
        public bool canMove = true;
        public float speed = 50;
        public float maxVelocity = 12;
        public float upthrust = 40;
        [SerializeField] private float acceleration = 1;
        public float currentVelocity { get; set; }
        [SerializeField] float absMaxVel = 40;
        [SerializeField] float drag = 0.2f;
        [SerializeField] int jumpForce = 400;
        [SerializeField] int gravityMultiplier = 12;
        [SerializeField] private float minGroundRange = 0.1f;
        
        public bool isGrounded { get; private set; }

        public float lowerVelocityThresholdMultiplier = 0.9f;
        private float FOV;

        //On Starts
        [HideInInspector]public float speedOnStart;
        [HideInInspector]public float maxVelocityOnStart;

        //Layers
        [Tooltip("Player Will Slide Down When Contacting Layer")]
        [SerializeField]private LayerMask ground;

        //Properties
        public bool isMoving;

        //Optionals
        [HideInInspector] public bool isGliding;
        [HideInInspector] public bool isGrappling;
        [HideInInspector] public Vector3 wallNormal;


        private bool isWallrunning;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            input = GetComponent<PlayerInput>();
            overlayCam = cam.transform.GetChild(0).GetComponent<Camera>();
            camController = GetComponent<CameraController>();
            graphics = GetComponent<PlayerGraphics>();
            
            maxVelocityOnStart = maxVelocity;
            speedOnStart = speed;
            FOV = cam.fieldOfView;
            
            canMove = true;
        }

        void FixedUpdate() {
            if (!isActive) return;
            
            if(canMove && !isGliding){
                Gravity();
                Movement();
                //CounterMovement(); 
                JumpHandler();
            }

            ClampVelocity();
        }

        #region Movement
    
        void Movement()
        {
            if (isWallrunning) {
                WallRun();
                return;
            }
            
            float horizontalMultiplier = 1;
            float verticalMultiplier = 1;

            //On Ground
            if(isGrounded && !isGrappling) 
            {
                maxVelocity = Mathf.Lerp(maxVelocity, maxVelocityOnStart, Time.deltaTime * 3);
                HandleFOV(FOV);
            }
            //In Air
            else 
            {
                horizontalMultiplier = 0.75f;
                verticalMultiplier = 0.5f;
                HandleFOV(FOV * 1.1f);
            }

            //-0.5 due to the lerping never reaching maxVelocity.
            acceleration = currentVelocity / maxVelocity;
            if(!isGrounded && currentVelocity >= maxVelocity - 0.5f){
                maxVelocity += Time.deltaTime * acceleration;
            }
            else if(currentVelocity <= maxVelocity * lowerVelocityThresholdMultiplier){
                maxVelocity -= Time.deltaTime * 10 * acceleration;
            }

            maxVelocity = Mathf.Clamp(maxVelocity, maxVelocityOnStart, absMaxVel);
            Vector3 dir = ((orientation.forward * input.yInput * verticalMultiplier) + (orientation.right * input.xInput * horizontalMultiplier)).normalized;
            rb.AddForce(dir * speed);
        }

        void CounterMovement()
        {
            if(rb.velocity.magnitude > 0 && isGrounded){
                rb.AddForce(-rb.velocity.normalized * drag * speed);
            }

            if(rb.velocity.magnitude < 0.5f && !isMoving && isGrounded){
                rb.velocity = Vector3.zero;
            }

            var vel = rb.velocity;
            vel.y = 0;
            vel = vel.normalized;
        }

        void ClampVelocity()
        {
            //Old clamping code
            
            //Vector3 tempVector = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            ////tempVector = Vector3.ClampMagnitude(tempVector, maxVelocity);
            //tempVector.y = rb.velocity.y;
            
            //New Clamping Code
            
            Vector3 noUpVec = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (noUpVec.magnitude > maxVelocity) 
            {
                noUpVec *= 0.9f;
                noUpVec.y = rb.velocity.y;
                rb.velocity = noUpVec;
            }
            currentVelocity = rb.velocity.magnitude;
        }

        void Gravity()
        {
            rb.AddForce(Vector3.down * gravityMultiplier);
        }

        #endregion

        #region Jumping

        void JumpHandler()
        {
            if(readyToJump && input.jumping && isGrounded)
            {
                Jump();
            }

            if (isWallrunning && input.jumping) 
            {
                JumpWallrun();    
            }
        }

        void Jump()
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            readyToJump = false;
        }

        void JumpWallrun() 
        {
            rb.AddForce(wallNormal * jumpForce * 0.5f, ForceMode.VelocityChange);
            rb.AddForce(orientation.forward * jumpForce * 0.2f, ForceMode.VelocityChange);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
        
        #endregion

        #region Wallrun

        private void WallRun() {
            var noY = rb.velocity;
            noY.y = Mathf.Clamp(noY.y, -2f, 2f);
            rb.velocity = noY;

            Vector3 wallTangent = Vector3.Cross(wallNormal, Vector3.up).normalized;
            
            if (Vector3.Dot(orientation.forward, wallTangent) < 0) {
                wallTangent *= -1;
            }
            rb.AddForce(Vector3.down);
            if(input.yInput <= 0) return;
            rb.AddForce(wallTangent * speed);
        }

        #endregion
        
        #region Collisions

        private void OnCollisionEnter(Collision other) 
        {
            if(wallNormal == other.GetContact(0).normal) return;
            wallNormal = other.GetContact(0).normal;
            var dot = Vector3.Dot(wallNormal, orientation.forward);
            bool isWall = wallNormal != Vector3.up;
            if(isWall && !isGrounded) {
                if (dot < 0.3f) {
                    isWallrunning = true;
                    graphics.SpawnWallImpact(other.GetContact(0).point + wallNormal * 0.2f, other.transform.GetComponent<Renderer>().material, wallNormal);
                    Vector3 wallTangent = Vector3.Cross(wallNormal, Vector3.up).normalized;
                    if (Vector3.Dot(orientation.forward, wallTangent) < 0) {
                        camController.TweenTargetRot(15);
                        return;
                    }
                    if(wallNormal - Vector3.up != Vector3.zero)
                        camController.TweenTargetRot(-15);
                    return;
                }
            }
            camController.TweenTargetRot(0);
            isWallrunning = false;
        }


        private void OnCollisionStay(Collision other) {
            //6 = Ground, 8 = Swingable
            if (other.gameObject.layer == 6 || other.gameObject.layer == 8) {
                if (Physics.Raycast(groundCheck.position, Vector3.down, minGroundRange, ground) && !isGrounded) {
                    isGrounded = true;
                    isWallrunning = false;
                    camController.TweenTargetRot(0);
                    wallNormal = Vector3.zero;
                    graphics.SpawnGroundImpact(groundCheck.position, other.transform.GetComponent<Renderer>().material);
                    return;
                }
            }
            
            //If looking away from wall and press W, drop off
            //0.5 is 45degrees from the wall
            if (isWallrunning && Vector3.Dot(orientation.forward, other.contacts[0].normal) > 0.5f && input.yInput > 0) 
            {
                rb.AddForce(wallNormal * jumpForce * 0.5f, ForceMode.VelocityChange);
            }

            //Slide Down Wall
            bool isWall = wallNormal != Vector3.up;
            if(isWall && !isGrounded && !isWallrunning) {
                rb.AddForce(Vector3.down * 100);
                rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -10, 0) ,rb.velocity.z);
            }
        }

        private void OnCollisionExit(Collision other) 
        {
            //6 = Ground, 8 = Swingable
            if(other.gameObject.layer != 6 && other.gameObject.layer != 8) return;
            
            //Jumped
            if (isGrounded) {
                isGrounded = false;
                readyToJump = true;
            }
            
            //Stop wallrunning
            if (isWallrunning) {
                isWallrunning = false;
                camController.TweenTargetRot(0);
            }
        }

        #endregion

        #region Other

        void OnDrawGizmos()
        {
            if(!debug) return;
            Color groundCheckCol = isGrounded ? Color.green : Color.red;
            Debug.DrawRay(groundCheck.position, Vector3.down * minGroundRange, groundCheckCol);
        }

        public void HandleFOV(float _desiredFOV){
            cam.DOFieldOfView(_desiredFOV, 0.5f);
            overlayCam.DOFieldOfView(_desiredFOV, 0.5f);
        }

        #endregion
    }
}