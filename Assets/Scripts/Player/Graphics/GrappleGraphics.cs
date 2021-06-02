using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Wildflare.Player.Combat;
using Wildflare.Player.Movement;

namespace Wildflare.Player.Graphics
{
    public class GrappleGraphics : MonoBehaviour
    {
        private Grapple grapple;
        private PlayerMovement movement;
        [SerializeField]LineRenderer lr;
        [SerializeField]Color lineColor;
        [SerializeField]Material lineMaterial;
        [SerializeField]Transform hookStart;
        [SerializeField]Transform lineEnd;
        [SerializeField]private Sway sway;
        [SerializeField]private Transform spearOrientation;
        [SerializeField]private GameObject spearHitParticles;
        [SerializeField]private Material spearHitParticlesMat;

        private Transform transformOnStart;

        public Transform hook;

        public Image crosshair;
        private Vector3 crosshairScaleOnStart;

        public int resolution;
        public float graphicsDamper;
        public float strength;
        public float speed;
        public float waveCount;
        public float waveHeight;
        public AnimationCurve affectCurve;
        private Spring spring;


        private Vector3 grapplePoint;
        private float grappleTime;
        private Vector3 inverseHitNormal;

        //Other

        bool showUI;
        public Material speedlinesMat;
        public Transform speedlines;

        //Networking
        Vector3 lineEndTarget;

        void Awake()
        {
            //Assigning References
            grapple = GetComponent<Grapple>();
            movement = GetComponent<PlayerMovement>();

            spring = new Spring();
            spring.SetTarget(0);

            transformOnStart = transform;
        }

        void Start()
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
        
        void Update()
        {   
            DrawRope();
            AlterSpeedlineOpacity();

            if(!grapple.getisActiveWeapon) return;

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
            if(grapple.getIsGrappling){
                grapplePoint = lineEndTarget;
            }

            hook.position = lineEnd.position + hook.up * 2.2f;

            if(lr.positionCount > 2 && !grapple.getIsGrappling) {
                lr.positionCount = 2;
            }

            Vector3 gunTip = hookStart.position;

            grappleTime = Mathf.Lerp(grappleTime, grapple.getIsGrappling ? 1f : 0, Time.deltaTime * 15f);
            lineEnd.position = Vector3.Lerp(gunTip, grapplePoint - (inverseHitNormal * 2.2f), grappleTime);

            lr.enabled = grappleTime > 0.01f;

            if (!grapple.getIsGrappling) {
                lr.SetPosition(0, gunTip);
                lr.SetPosition(1, lineEnd.position);
                return;
            }

            if(lr.positionCount == 2)
            {
                lr.positionCount = resolution + 1;
            }
            
            Vector3 up = Quaternion.LookRotation((grapplePoint - gunTip).normalized) * Vector3.up;

            for(int i = 0; i < resolution + 1; i++)
            {
                float delta = i / (float)resolution;
                Vector3 offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value * affectCurve.Evaluate(delta);
                lr.SetPosition(i, Vector3.Lerp(hookStart.position, lineEnd.position, delta) + offset);
            }
            hook.up = inverseHitNormal;
        }

        public void StopGrapple()
        {
            lineEndTarget = hookStart.position;

            hook.parent = movement.cam.transform;
            lineEnd.parent = movement.cam.transform;

            ChangeHookRenderLayer(8);
            LineDismantle();
        }

        void UpdateSpring()
        {
            if(!grapple.getIsGrappling)
            {
                spring.Reset();
            }

            if(lr.positionCount == 2)
            {
                spring.SetVelocity(speed);
            }

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

        public void AlterSpeedlineOpacity(){
            float opacity = 0;
            if(movement.currentVelocity > 20){
                opacity = ((movement.currentVelocity - 20) / 20) - 0.2f;
            }

            speedlinesMat.color = new Color(speedlinesMat.color.r, speedlinesMat.color.g, speedlinesMat.color.b, opacity);
            speedlines.transform.localPosition = Vector3.Lerp(new Vector3(0, 0, -40), new Vector3(0, 0, -16), opacity);
        }

        public void ChangeHookRenderLayer(int _layer)
        {
            foreach(Transform child in hook.GetChild(0)){
                child.gameObject.layer = _layer;
            }

            //lr.gameObject.layer = _layer;
        }

    }
}