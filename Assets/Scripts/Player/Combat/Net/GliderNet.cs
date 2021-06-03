using DG.Tweening;
using UnityEngine;
using Wildflare.Player.Movement.Net;

namespace Wildflare.Player.Combat.Net
{
    public class GliderNet : MonoBehaviour
    {
        private const float gliderSpeed = 0.5f;
        private const float gliderLift = 0.1f;
        private const float gliderLength = 4;
        private static readonly int DoDown = Animator.StringToHash("doDown");
        [SerializeField] private Transform glider;

        public Animator spearAnim;
        private readonly Vector3 gliderOffset = new Vector3(0, 5, 0);
        private Vector3 gliderStartPos;
        private Vector3 gliderStartRot;
        private GrappleNet grapple;
        private PlayerMovementNet movement;
        private float oscillatingGliderTimer;

        private Rigidbody rb;

        [HideInInspector] public bool isGliding { get; private set; }

        private void Awake()
        {
            movement = GetComponent<PlayerMovementNet>();
            grapple = GetComponent<GrappleNet>();
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            gliderStartPos = glider.localPosition;
            var localEulerAngles = glider.localEulerAngles;
            gliderStartRot = localEulerAngles;
            glider.localPosition = gliderStartPos + gliderOffset;
            localEulerAngles = new Vector3(localEulerAngles.x, localEulerAngles.y, -90);
            glider.localEulerAngles = localEulerAngles;
            glider.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!movement.isActive) return;
            movement.isGliding = isGliding;
            GliderManager();
        }

        private void FixedUpdate()
        {
            if (isGliding) DoGliding();
        }

        private void LateUpdate()
        {
            if (glider.localPosition == gliderStartPos + gliderOffset && movement.canMove)
                glider.gameObject.SetActive(false);
        }

        private void GliderManager()
        {
            if (Input.GetKeyDown(KeyCode.Mouse1) && !grapple.getIsGrappling && oscillatingGliderTimer == 0)
                StartGliding();

            if (Input.GetKeyUp(KeyCode.Mouse1) && isGliding)
            {
                oscillatingGliderTimer = gliderLength;
                StopGliding();
            }

            if (movement.isGrounded && isGliding) StopGliding();

            GliderTimer();
        }

        private void StartGliding()
        {
            isGliding = true;
            movement.canMove = false;
            glider.gameObject.SetActive(true);
            glider.DOLocalMove(gliderStartPos, 0.3f);
            glider.DOLocalRotate(gliderStartRot, 0.4f);
            spearAnim.SetBool(DoDown, true);
        }

        private void DoGliding()
        {
            rb.AddForce(Vector3.up * gliderLift, ForceMode.Impulse);
            rb.AddForce(movement.orientation.forward * gliderSpeed, ForceMode.Impulse);
        }

        private void StopGliding()
        {
            isGliding = false;
            movement.canMove = true;
            glider.DOLocalMove(gliderStartPos + gliderOffset, 0.3f);
            glider.DOLocalRotate(new Vector3(gliderStartRot.x, gliderStartRot.y, -90), 0.4f);
            spearAnim.SetBool("doDown", false);
        }

        private void GliderTimer()
        {
            if (isGliding)
                oscillatingGliderTimer += Time.deltaTime;
            else
                oscillatingGliderTimer -= Time.deltaTime * 1.5f;
            oscillatingGliderTimer = Mathf.Clamp(oscillatingGliderTimer, 0, gliderLength);

            if (oscillatingGliderTimer == gliderLength) StopGliding();
        }

        public bool IsGliding()
        {
            if (isGliding) return true;
            return false;
        }
    }
}