using UnityEngine;
using Wildflare.Player.Inputs;
using DG.Tweening;
using Mirror;

namespace Wildflare.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerMovement : NetworkBehaviour
    {

        //Assignables
        public Transform orientation;
        [SerializeField] private Transform groundCheck;
        [HideInInspector] public PlayerInput input;
        public Camera cam;
        public Camera overlayCam;
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
        [SerializeField] float absMaxVel = 40;
        [SerializeField] float drag = 0.2f;
        [SerializeField] int jumpForce = 400;
        [SerializeField] int gravityMultiplier = 12;
        [SerializeField] private float minGroundRange = 0.1f;
        public bool isGrounded{get; set;}
        public float lowerVelocityThresholdMultiplier = 0.9f;
        private float FOV;

        //On Starts
        [HideInInspector]public float speedOnStart;
        [HideInInspector]public float maxVelocityOnStart;
        [HideInInspector]public float currentVelocity; 
        private Vector3 camPositionOnStart;

        //Layers
        [Tooltip("Player Will Slide Down When Contacting Layer")] 
        [SerializeField]private LayerMask slideMask;
        [SerializeField]private LayerMask ground;

        //Properties
        public bool isMoving;

        //Optionals
        [HideInInspector] public bool isGliding;
        [HideInInspector] public bool isGrappling;

        public Material mat;
        
        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            input = GetComponent<PlayerInput>();
            DOTween.Init();
            maxVelocityOnStart = maxVelocity;
            FOV = cam.fieldOfView;
            speedOnStart = speed;
            camPositionOnStart = cam.transform.localPosition;
            canMove = true;
        }

        void Update()
        {
            if(!isActive || !hasAuthority) return;
            ClampVelocity();
        }

        void FixedUpdate()
        {
            if(!isActive || !hasAuthority) return;
            if(canMove){
                if(!isGliding){
                    Gravity();
                    Movement();
                    CounterMovement(); 
                }
            }
            JumpHandler();
        }

        #region Movement
    
        void Movement()
        {
            float horizontalMultiplier = 1;
            float verticalMultiplier = 1;
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
            if(!isGrounded && currentVelocity >= maxVelocity - 0.5f){
                maxVelocity += Time.deltaTime;
            }
            else if(currentVelocity <= maxVelocity * lowerVelocityThresholdMultiplier){
                maxVelocity -= Time.deltaTime * 10;
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
            vel.Normalize();
            var distance = rb.velocity.magnitude * Time.deltaTime;

            if(Physics.CheckSphere(transform.position, 1, slideMask) && !isGrounded)
            {
                rb.AddForce(Vector3.down * 100);
                rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -10, 0) ,rb.velocity.z);
            }
        }

        void ClampVelocity()
        {
            Vector3 tempVector = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            tempVector = Vector3.ClampMagnitude(tempVector, maxVelocity);
            tempVector.y = rb.velocity.y;
            rb.velocity = tempVector;
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
            if(readyToJump && input.jumping)
            {
                Jump();
            }
        }

        void Jump()
        {
            if(isGrounded)
            {
                readyToJump = false; 
                rb.AddForce(Vector3.up * jumpForce);
                if(currentVelocity > 6){
                    rb.AddForce(orientation.forward * (jumpForce / 2));
                }
                Invoke(nameof(ResetJump), 0.25f);
            }
        }

        void ResetJump()
        {
            readyToJump = true;
        }

        void OnCollisionStay(Collision col)
        {
            if(col.gameObject.layer == 6)
            {
                isGrounded = true;
            }
        }

        void OnCollisionExit(Collision col)
        {
            if(col.gameObject.layer == 6)
            {
                isGrounded = false;
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