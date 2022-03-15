using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Barji.Player.Attachments;
using Barji.Player.Movement;

namespace Barji.Player.Graphics
{
    public class GrappleGraphics : MonoBehaviour
    {
        [SerializeField] private LineRenderer lr;
        [SerializeField] private Color lineColor;
        [SerializeField] private Material lineMaterial;
        [SerializeField] private Transform hookStart;
        [SerializeField] private Transform lineEnd;
        [SerializeField] private Transform spearOrientation;
        [SerializeField] private GameObject spearHitParticles;
        [SerializeField] private Material spearHitParticlesMat;
        [SerializeField] private Sway sway;
        [SerializeField] private Transform spearMesh;
        public Transform spear;

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

        private Vector3 spearStartPosition;
        private Quaternion spearMeshStartRot;
        private Vector3 spearStartRot;
        private Transform spearParentOnStart;

        [Header("VR")]
        [SerializeField] private LineRenderer rayGuide;


        private void Awake()
        {
            //Assigning References
            grapple = GetComponent<Grapple>();
            movement = GetComponent<PlayerMovement>();

            spring = new Spring();
            spring.SetTarget(0);


            spearStartPosition = spear.localPosition;
            spearParentOnStart = spear.parent;
            spearMeshStartRot = spearMesh.transform.localRotation;
            spearStartRot = spear.localEulerAngles;

            if(movement.isVR)
                rayGuide.positionCount = 2;
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

            if(!movement.isVR)
                crosshairScaleOnStart = crosshair.transform.localScale;
        }

        private void Update()
        {
            if (PlayerMovement.currentState == PlayerMovement.state.Stopped) return;
            AlterSpeedlineOpacity();
            UpdateSpring();
        }

        void LateUpdate()
        {
            if (PlayerMovement.currentState == PlayerMovement.state.Stopped) return;
            DrawRope();
        }


        public void StartGrapple(RaycastHit _hitInfo)
        {
            if(!movement.isVR)
                sway.enabled = false;
            spear.parent = null;
            ChangeGrappleRenderLayer("Default");
            lineEnd.parent = movement.cam.transform.parent;
            lineEndTarget = _hitInfo.point;
            inverseHitNormal = (_hitInfo.point - hookStart.transform.position).normalized;
        }

        public void DrawRope()
        {
            bool isGrappling = PlayerMovement.currentState == PlayerMovement.state.Grappling;
            if (isGrappling) grapplePoint = lineEndTarget;

            if(isGrappling || grappleTime > 0.01f)
            {       
                spear.position = lineEnd.position;
                if(isGrappling)
                {
                    spear.position = grapplePoint;
                    spearMesh.up = inverseHitNormal;
                }
                else if (movement.isVR)
                {
                    spear.localRotation = Quaternion.Lerp(spear.localRotation, Quaternion.identity, Time.deltaTime * 8);
                    spearMesh.localRotation = Quaternion.Lerp(spearMesh.localRotation, spearMeshStartRot, Time.deltaTime * 8);
                }
                else
                {
                    spear.localRotation = Quaternion.Lerp(spear.localRotation, Quaternion.Euler(spearStartRot), Time.deltaTime * 8);
                    //spearMesh.localRotation = Quaternion.Lerp(spearMesh.localRotation, spearMeshStartRot, Time.deltaTime * 8);
                }
            }
            else
            {   
                if(movement.isVR)
                {
                    spear.localPosition = Vector3.zero;
                    spear.localEulerAngles = Vector3.zero;
                }
                else
                {
                    spear.localPosition = spearStartPosition;
                    spear.localEulerAngles = spearStartRot;
                }
                spearMesh.localRotation = spearMeshStartRot;
            }

            if (lr.positionCount > 2 && !isGrappling) lr.positionCount = 2;

            grappleTime = Mathf.Lerp(grappleTime, isGrappling ? 1f : 0, Time.deltaTime * 15f);
            lineEnd.position = Vector3.Lerp(hookStart.position, grapplePoint - inverseHitNormal * 2.2f, grappleTime);

            lr.enabled = grappleTime > 0.01f;

            if (!isGrappling)
            {
                lr.SetPosition(0, hookStart.position);
                lr.SetPosition(1, lineEnd.position);
                return;
            }

            if (lr.positionCount == 2) lr.positionCount = resolution + 1;

            var up = Quaternion.LookRotation((grapplePoint - hookStart.position).normalized) * Vector3.up;

            for (var i = 0; i < resolution + 1; i++)
            {
                var delta = i / (float) resolution;
                var offset = up * (waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value * affectCurve.Evaluate(delta));
                lr.SetPosition(i, Vector3.Lerp(hookStart.position, lineEnd.position, delta) + offset);
            }
        }

        public void StopGrapple()
        {
            lineEndTarget = hookStart.position;
            var cam = movement.cam.transform;
            spear.parent = spearParentOnStart;
            lineEnd.parent = cam;
            LineDismantle();
            ChangeGrappleRenderLayer("SecondCam");
            if(!movement.isVR)
                sway.enabled = true;
        }

        private void UpdateSpring()
        {
            bool isGrappling = PlayerMovement.currentState == PlayerMovement.state.Grappling;
            if (!isGrappling) spring.Reset();

            if (lr.positionCount == 2) spring.SetVelocity(speed);

            spring.SetDamper(graphicsDamper);
            spring.SetStrength(strength);
            spring.Update(Time.deltaTime);
        }

        public void LineDismantle()
        {
            spear.localRotation = spearOrientation.localRotation;
            inverseHitNormal = Vector3.zero;
        }

        public void SpawnParticle(RaycastHit hit)
        {
            Renderer r;
            if (!hit.transform.TryGetComponent(out r)) return;
            var hitMat = hit.transform.GetComponent<Renderer>().material;
            spearHitParticlesMat.color = hitMat.color;
            spearHitParticlesMat.mainTexture = hitMat.mainTexture != null ? hitMat.mainTexture : null;
            var particle = Instantiate(spearHitParticles, hit.point, Quaternion.identity);
            particle.transform.forward = hit.normal;
        }

        public void AlterSpeedlineOpacity()
        {
            float opacity = 0;
            if (movement.currentVelocity > 20) opacity = (movement.currentVelocity - 20) / 20 - 0.2f;
            
            speedlinesMat.color = new Color(speedlinesMat.color.r, speedlinesMat.color.g, speedlinesMat.color.b, opacity);
            speedlines.transform.localPosition = Vector3.Lerp(new Vector3(0, 0, -40), new Vector3(0, 0, -16), opacity);
        }

        void ChangeGrappleRenderLayer(string desiredLayer)
        {
            //Get all renderers in spear mesh and change layers
            foreach (Transform child in spearMesh)
            {
                child.gameObject.layer = LayerMask.NameToLayer(desiredLayer);;
            }
        }

        public void OffsetSpearPos()
        {
            if(!movement.isVR)
                sway.enabled = false;
            spearMesh.DOKill(true);
            spearMesh.DOLocalMoveZ(0.5f, 0.2f);
        }

        public void ResetSpearPos()
        {
            spearMesh.DOKill(true);
            spearMesh.DOLocalMoveZ(0.261f, 0.2f);
            if(!movement.isVR)
                sway.enabled = true;
        }

        public void DrawGuide(Vector3 startPoint, Vector3 endPoint, float opacity)
        {
            rayGuide.SetPosition(0, startPoint);
            rayGuide.SetPosition(1, endPoint);
            rayGuide.startColor = new Color(rayGuide.startColor.r, rayGuide.startColor.g, rayGuide.startColor.b, opacity);
            rayGuide.endColor = new Color(rayGuide.startColor.r, rayGuide.startColor.g, rayGuide.startColor.b, opacity);
        }
    }
}