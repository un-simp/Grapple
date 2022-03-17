using System;
using System.Collections;
using Barji.Player.Combat;
using DG.Tweening;
using UnityEngine;
using Barji.Player.Cam;
using Barji.Player.Graphics;
using Barji.Player.Inputs;

namespace Barji.Player.Movement
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerMovement : MonoBehaviour
    {
        //Assignables
        [SerializeField] private Transform groundCheck;
        [SerializeField]private LayerMask groundLayer = 6;
        private PlayerGraphics graphics;
        private PlayerInput input;
        private PlayerCombat combat;
        private CameraController camController;
        private Camera overlayCam;
        private new CapsuleCollider collider;
        public Transform orientation;
        public Camera cam;
        public Rigidbody rb;
        
        //State
        [SerializeField] private bool debug;
        private bool canJump = true;
        public bool canMove = true;

        //Affectors
        [SerializeField] private float acceleration = 1;
        [SerializeField] private float drag = 0.2f;
        [SerializeField] private int jumpForce = 400;
        [SerializeField] private int gravityMultiplier = 12;
        [SerializeField] private float minGroundRange = 0.1f;
        public float absMaxVel = 40;
        public float speed = 50;
        public float maxVelocity = 12;
        public float upthrust = 40;
        public float lowerVelocityThresholdMultiplier = 0.9f;

        //Stores
        private Vector3 wallTangentDir;
        [HideInInspector] public float FOV;
        [HideInInspector] public Vector3 wallNormal;
        private ContactPoint lastContactPoint;
        private bool footstepOngoing;

        //On Starts
        [HideInInspector] public float speedOnStart;
        [HideInInspector] public float maxVelocityOnStart;

        //Properties
        public bool isMoving { get; set; }
        public float currentVelocity { get; private set; }
        public bool isGrounded { get; private set; }
        public bool rayHitGround => Physics.Raycast(groundCheck.position, Vector3.down, minGroundRange, groundLayer);
        
        //Events
        public Action OnFootstep;
        public Action OnLanded;
        
        //States
        public enum state
        {
            Walking, Wallrunning, Airborn, Gliding, Grappling, Stopped
        }
        public static state currentState = state.Airborn;

        public bool isVR;


        void Awake()
        {
            if(!isVR)
            {
                camController = GetComponent<CameraController>();
                combat = GetComponent<PlayerCombat>();
                overlayCam = cam.transform.GetChild(0).GetComponent<Camera>();

            }
            rb = GetComponent<Rigidbody>();
            input = GetComponent<PlayerInput>();
            graphics = GetComponent<PlayerGraphics>();
            collider = GetComponent<CapsuleCollider>();
            maxVelocityOnStart = maxVelocity;
            speedOnStart = speed;
            FOV = cam.fieldOfView;
            canMove = true;
            currentState = state.Airborn;
        }

        void LateUpdate()
        {
            if(isVR)
                orientation.eulerAngles = new Vector3(0, orientation.eulerAngles.y, orientation.eulerAngles.z);
        }

        private void FixedUpdate()
        {
            if(!canMove || currentState == state.Stopped) return;
            
            //State Independant
            ClampVelocity();

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
                    if(isVR) return;
                    WallRun();
                    JumpHandler();
                    Gravity(); Gravity(); //Double Gravity
                    break;
                case state.Grappling:
                    AirbornMovement();
                    Gravity();
                    break;
                case state.Gliding:
                    break;
            }
        }

        #region Movement

        private void StartWalking(Collision other)
        {
            isGrounded = true;
            canJump = true;
            wallNormal = Vector3.zero;
            Renderer r;
            if(other.gameObject.TryGetComponent(out r))
                graphics.SpawnGroundImpact(groundCheck.position, other.transform.GetComponent<Renderer>().material);
            
            OnLanded.Invoke();
            if(!isVR)
            {
                camController.TweenTargetRot(0);
                combat.canLunge = true;
            }
            
            currentState = state.Walking;
            StopCoroutine(nameof(Footstep));
            StartCoroutine(nameof(Footstep));
            
            //Add a small force to counteract friction in the case of 'B-hopping'
            if (isMoving)
            {
                var dir = (orientation.forward * input.yInput + orientation.right * input.xInput).normalized;
                rb.AddForce(dir * 7, ForceMode.Impulse);
            }
        }
        
        IEnumerator Footstep()
        {
            yield return new WaitForSeconds(0.2f);
            if (currentState != state.Walking)
                yield break;
            if(isMoving)
                OnFootstep.Invoke();
            yield return Footstep();
        }

        private void Walking()
        {
            //On Ground
            maxVelocity = Mathf.Lerp(maxVelocity, maxVelocityOnStart, Time.deltaTime * 3);
            HandleFOV(FOV);
            Movement(1, 1);
        }
        

        private void AirbornMovement()
        {
            HandleFOV(Mathf.Clamp((FOV + FOV / 3) * (currentVelocity / absMaxVel), FOV, FOV + FOV / 3));
            Movement(0.75f, 0.75f);
        }

        void Movement(float hMultiplier, float vMultiplier)
        {
            acceleration = currentVelocity / maxVelocity;
            
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
            //New Clamping Code

            var noUpVec = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (noUpVec.magnitude > maxVelocity)
            {
                noUpVec *= 0.97f;
                noUpVec = Vector3.ClampMagnitude(noUpVec, maxVelocity);
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
            if (currentState == state.Walking && canJump && input.jumping) Jump();
            
            if ((currentState == state.Wallrunning) && input.jumping) JumpWallrun();
        }

        private void Jump()
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            canJump = false;
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
                    Renderer r;
                    if(other.gameObject.TryGetComponent(out r))
                        graphics.SpawnWallImpact(other.GetContact(0).point + wallNormal * 0.2f,
                        other.transform.GetComponent<Renderer>().material, wallNormal);
                    OnLanded.Invoke();
                    if (Vector3.Dot(orientation.forward, wallTangent) < 0)
                    {
                        if(!isVR)
                            camController.TweenTargetRot(15);
                        return;
                    }

                    if (wallNormal - Vector3.up != Vector3.zero)
                        if(!isVR)
                            camController.TweenTargetRot(-15);
                    return;
                }
            if(!isVR)
                camController.TweenTargetRot(0);
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
            //If touching a ceiling then drop
            if (wallNormal == -Vector3.up)
                rb.AddForce(Vector3.down * 5, ForceMode.VelocityChange);
        }

        #endregion
        
        #region Collisions

        private void OnTriggerEnter(Collider other) 
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("Bounce") && rb.velocity.y < 0)
                rb.velocity = new Vector3(rb.velocity.x, -rb.velocity.y, rb.velocity.z);    
        }

        private void OnCollisionEnter(Collision other)
        {
            //Disregard the collision if grappling or not on swingable/ground layer
            bool shouldDisregardCollision = (other.gameObject.layer != 6 && other.gameObject.layer != 8);
            if(shouldDisregardCollision) return;

            
            bool isWall = other.GetContact(0).normal != Vector3.up && !rayHitGround;
            
            if (currentState == state.Grappling && !isWall)
            {
                isGrounded = true;
                canJump = true;
                return;
            }
            
            //If we touch the ground and we aren't walking, start walking
            if (!isWall)
            {
                StartWalking(other);
            }
            //If we hit a wall then start wallrunning
            else if(currentState != state.Grappling)
            {
                StartWallrun(other);
            }
        }


        private void OnCollisionStay(Collision other)
        {
            //Disregard the collision if grappling or not on swingable/ground layer
            bool shouldDisregardCollision = (other.gameObject.layer != 6 && other.gameObject.layer != 8) || currentState == state.Grappling;
            if(shouldDisregardCollision) return;
            
            bool isWall = other.GetContact(0).normal != Vector3.up && !rayHitGround;
            //If we touch the ground and we aren't walking, start walking
            if (!isWall && currentState != state.Walking)
            {
                StartWalking(other);
            }
            
            //Save the contact point for OnCollisionExit
            lastContactPoint = other.GetContact(0);

            //Slide Down Wall
            if (isWall && currentState == state.Airborn)
            {
                rb.AddForce(Vector3.down * 100);
                rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -10, 0), rb.velocity.z);
            }
        }

        private void OnCollisionExit(Collision other)
        {
            //Disregard the collision if grappling or not on swingable/ground layer
            bool shouldDisregardCollision = (other.gameObject.layer != 6 && other.gameObject.layer != 8);
            if(shouldDisregardCollision) return;
            
            var isWall = lastContactPoint.normal != Vector3.up && !rayHitGround;
            
            if (currentState == state.Grappling && !isWall)
            {
                isGrounded = true;
                canJump = true;
                return;
            }

            //Stop wallrunning
            if (currentState == state.Wallrunning)
            {
                if(!isVR)
                    camController.TweenTargetRot(0);
                if (rayHitGround)
                {
                    currentState = state.Walking;
                    canJump = true;
                    return;
                }
                currentState = state.Airborn;
            }

            //Jumped
            //This needs to be placed after the Stop wallrunning logic to ensure that the 
            //state isn't switched away from wallrunning to airborn before that logic executes
            if (currentState == state.Walking && rayHitGround)
            {
                currentState = state.Airborn;
                canJump = false;
                isGrounded = false;
                return;
            }
            
            //Handle any anomolies (If any modders are reading this I understand your frustration, just know this was a last ditch effort lmao)
            if(!rayHitGround && currentState != state.Grappling)
                currentState = state.Airborn;
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
            overlayCam.DOFieldOfView(_desiredFOV + 20, 0.5f);
        }

        #endregion
    }
}