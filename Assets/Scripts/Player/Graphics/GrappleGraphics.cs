using UnityEngine;
using UnityEngine.UI;
using Wildflare.Player.Combat;
using Wildflare.Player.Movement;

namespace Wildflare.Player.Graphics
{
    public class GrappleGraphics : MonoBehaviour
    {
        [SerializeField] private LineRenderer lr;
        [SerializeField] private Color lineColor;
        [SerializeField] private Material lineMaterial;
        [SerializeField] private Transform hookStart;
        [SerializeField] private Transform lineEnd;
        [SerializeField] private Sway sway;
        [SerializeField] private Transform spearOrientation;
        [SerializeField] private GameObject spearHitParticles;
        [SerializeField] private Material spearHitParticlesMat;

        public Transform hook;

        public Image crosshair;

        public int resolution;
        public float graphicsDamper;
        public float strength;
        public float speed;
        public float waveCount;
        public float waveHeight;
        public AnimationCurve affectCurve;
        public Material speedlinesMat;
        public Transform speedlines;
        private Vector3 crosshairScaleOnStart;
        private Grapple grapple;


        private Vector3 grapplePoint;
        private float grappleTime;
        private Vector3 inverseHitNormal;

        //Networking
        private Vector3 lineEndTarget;
        private PlayerMovement movement;

        //Other

        private bool showUI;
        private Spring spring;

        private Transform transformOnStart;

        private void Awake()
        {
            //Assigning References
            grapple = GetComponent<Grapple>();
            movement = GetComponent<PlayerMovement>();

            spring = new Spring();
            spring.SetTarget(0);

            transformOnStart = transform;
        }

        private void Start()
        {
            lr.sharedMaterial = lineMaterial;
            lr.startColor = lineColor;
            lr.endColor = lineColor;
            lr.startWidth = 0.1f;
            lr.endWidth = 0.1f;
            lr.useWorldSpace = true;
            lr.positionCount = 2;

            crosshairScaleOnStart = crosshair.transform.localScale;
        }

        private void Update()
        {
            DrawRope();
            AlterSpeedlineOpacity();
            UpdateSpring();
        }


        public void StartGrapple(RaycastHit _hitInfo)
        {
            hook.parent = movement.cam.transform.parent;

            lineEnd.parent = movement.cam.transform.parent;

            sway.enabled = false;

            lineEndTarget = _hitInfo.point;
            inverseHitNormal = (_hitInfo.point - movement.transform.position).normalized;
            ChangeHookRenderLayer(0);
        }

        public void DrawRope()
        {
            bool isGrappling = movement.currentState == PlayerMovement.state.Grappling;
            if (isGrappling) grapplePoint = lineEndTarget;

            hook.position = lineEnd.position + hook.up * 2.2f;

            if (lr.positionCount > 2 && !isGrappling) lr.positionCount = 2;

            var gunTip = hookStart.position;

            grappleTime = Mathf.Lerp(grappleTime, isGrappling ? 1f : 0, Time.deltaTime * 15f);
            lineEnd.position = Vector3.Lerp(gunTip, grapplePoint - inverseHitNormal * 2.2f, grappleTime);

            lr.enabled = grappleTime > 0.01f;

            if (!isGrappling)
            {
                lr.SetPosition(0, gunTip);
                lr.SetPosition(1, lineEnd.position);
                return;
            }

            if (lr.positionCount == 2) lr.positionCount = resolution + 1;

            var up = Quaternion.LookRotation((grapplePoint - gunTip).normalized) * Vector3.up;

            for (var i = 0; i < resolution + 1; i++)
            {
                var delta = i / (float) resolution;
                var offset = up * (waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value *
                                   affectCurve.Evaluate(delta));
                lr.SetPosition(i, Vector3.Lerp(hookStart.position, lineEnd.position, delta) + offset);
            }

            hook.up = inverseHitNormal;
        }

        public void StopGrapple()
        {
            lineEndTarget = hookStart.position;

            var cam = movement.cam.transform;
            hook.parent = cam;
            lineEnd.parent = cam;

            ChangeHookRenderLayer(8);
            LineDismantle();
        }

        private void UpdateSpring()
        {
            bool isGrappling = movement.currentState == PlayerMovement.state.Grappling;
            if (!isGrappling) spring.Reset();

            if (lr.positionCount == 2) spring.SetVelocity(speed);

            spring.SetDamper(graphicsDamper);
            spring.SetStrength(strength);
            spring.Update(Time.deltaTime);
        }

        public void LineDismantle()
        {
            hook.localRotation = spearOrientation.localRotation;
            sway.enabled = true;
            inverseHitNormal = Vector3.zero;
        }

        public void AlterSpeedlineOpacity()
        {
            float opacity = 0;
            if (movement.currentVelocity > 20) opacity = (movement.currentVelocity - 20) / 20 - 0.2f;

            speedlinesMat.color =
                new Color(speedlinesMat.color.r, speedlinesMat.color.g, speedlinesMat.color.b, opacity);
            speedlines.transform.localPosition = Vector3.Lerp(new Vector3(0, 0, -40), new Vector3(0, 0, -16), opacity);
        }

        public void ChangeHookRenderLayer(int _layer)
        {
            foreach (Transform child in hook.GetChild(0)) child.gameObject.layer = _layer;

            //lr.gameObject.layer = _layer;
        }
    }
}