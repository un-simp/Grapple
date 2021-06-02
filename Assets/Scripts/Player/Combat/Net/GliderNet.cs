using UnityEngine;
using DG.Tweening;
using Wildflare.Player.Movement.Net;

namespace Wildflare.Player.Combat.Net
{
    public class GliderNet : MonoBehaviour
    {
        private PlayerMovementNet movement;
        private GrappleNet grapple;

        [HideInInspector]public bool isGliding { get; private set; }
        private Vector3 gliderStartPos;
        private readonly Vector3 gliderOffset = new Vector3(0, 5, 0);
        private Vector3 gliderStartRot;
        private const float gliderSpeed = 0.5f;
        private const float gliderLift = 0.1f;
        private const float gliderLength = 4;
        private float oscillatingGliderTimer = 0;
        [SerializeField]private Transform glider;

        private Rigidbody rb;

        public Animator spearAnim;
        private static readonly int DoDown = Animator.StringToHash("doDown");

        void Awake()
        {
            movement = GetComponent<PlayerMovementNet>();
            grapple = GetComponent<GrappleNet>();
            rb = GetComponent<Rigidbody>();
        }

        void Start()
        {
            gliderStartPos = glider.localPosition;
            Vector3 localEulerAngles = glider.localEulerAngles;
            gliderStartRot = localEulerAngles;
            glider.localPosition = gliderStartPos + gliderOffset;
            localEulerAngles = new Vector3(localEulerAngles.x, localEulerAngles.y, -90);
            glider.localEulerAngles = localEulerAngles;
            glider.gameObject.SetActive(false);
        }

        void Update()
        {
            if(!movement.isActive) return;
            movement.isGliding = isGliding;
            GliderManager();
        }

        void FixedUpdate()
        {
            if(isGliding){
                DoGliding();
            }
        }

        void LateUpdate()
        {
            if(glider.localPosition == gliderStartPos + gliderOffset && movement.canMove)
            {
                glider.gameObject.SetActive(false);
            }
        }

        void GliderManager(){
            if(Input.GetKeyDown(KeyCode.Mouse1) && !grapple.getIsGrappling && oscillatingGliderTimer == 0)
            {
                StartGliding();
            }

            if(Input.GetKeyUp(KeyCode.Mouse1) && isGliding)
            {
                oscillatingGliderTimer = gliderLength;
                StopGliding();
            }

            if(movement.isGrounded && isGliding)
            {
                StopGliding();
            }

            GliderTimer();
        }

        void StartGliding()
        {
            isGliding = true;
            movement.canMove = false;
            glider.gameObject.SetActive(true);
            glider.DOLocalMove(gliderStartPos, 0.3f);
            glider.DOLocalRotate(gliderStartRot, 0.4f); 
            spearAnim.SetBool(DoDown, true);
        }

        void DoGliding()
        {
            rb.AddForce(Vector3.up * gliderLift, ForceMode.Impulse);
            rb.AddForce(movement.orientation.forward * gliderSpeed, ForceMode.Impulse);
        }

        void StopGliding()
        {
            isGliding = false;
            movement.canMove = true; 
            glider.DOLocalMove(gliderStartPos + gliderOffset, 0.3f);
            glider.DOLocalRotate(new Vector3(gliderStartRot.x, gliderStartRot.y, -90), 0.4f);
            spearAnim.SetBool("doDown", false);
        }

        void GliderTimer(){
            if(isGliding){
                oscillatingGliderTimer += Time.deltaTime;
            }
            else{
                oscillatingGliderTimer -= Time.deltaTime * 1.5f;
            }
            oscillatingGliderTimer = Mathf.Clamp(oscillatingGliderTimer, 0, gliderLength);

            if(oscillatingGliderTimer == gliderLength){
                StopGliding();
            }
        }

        public bool IsGliding(){
            if(isGliding){
                return true;
            }
            return false;
        }
    }
}
