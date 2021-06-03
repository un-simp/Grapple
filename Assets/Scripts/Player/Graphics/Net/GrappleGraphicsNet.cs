using Mirror;
using UnityEngine;
using UnityEngine.UI;
using Wildflare.Player.Combat.Net;
using Wildflare.Player.Movement.Net;

namespace Wildflare.Player.Graphics.Net
{
    public class GrappleGraphicsNet : NetworkBehaviour
    {
        [SerializeField] private LineRenderer lr;
        [SerializeField] private Color lineColor;
        [SerializeField] private Material lineMaterial;
        [SerializeField] private Transform hookStart;
        [SerializeField] private Transform lineEnd;
        [SerializeField] private SwayNet sway;
        [SerializeField] private Transform spearOrientation;
        [SerializeField] private GameObject spearHitParticles;
        [SerializeField] private Material spearHitParticlesMat;
        [SerializeField] private Animator anim;

        public Transform hook;

        public Image crosshair;

        public int resolution;
        public float graphicsDamper;
        public float strength;
        public float speed;
        public float waveCount;
        public float waveHeight;
        public AnimationCurve affectCurve;
        public Animator spearAnim;
        public Material speedlinesMat;
        public Transform speedlines;
        private Vector3 crosshairScaleOnStart;
        private GrappleNet grapple;


        private Vector3 grapplePoint;
        private float grappleTime;
        private Vector3 inverseHitNormal;

        //Networking
        private Vector3 lineEndTarget;
        private PlayerMovementNet movement;

        //Other

        private bool showUI;
        private Spring spring;

        private Transform transformOnStart;

        private void Awake()
        {
            //Assigning References
            grapple = GetComponent<GrappleNet>();
            movement = GetComponent<PlayerMovementNet>();

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

        [Client]
        private void Update()
        {
            DrawRope();
            AlterSpeedlineOpacity();

            if (!hasAuthority || !grapple.getisActiveWeapon) return;

            UpdateSpring();
        }


        public void StartGrapple(RaycastHit _hitInfo)
        {
            hook.parent = movement.cam.transform.parent;
            CmdUpdateHookParent(false);

            lineEnd.parent = movement.cam.transform.parent;
            CmdUpdateLineEndParent(false);

            sway.enabled = false;
            spearAnim.enabled = false;

            lineEndTarget = _hitInfo.point;
            inverseHitNormal = (_hitInfo.point - movement.transform.position).normalized;
            ChangeHookRenderLayer(0);
            CmdUpdateLinePosition(_hitInfo.point);
        }

        public void DrawRope()
        {
            if (grapple.getIsGrappling) grapplePoint = lineEndTarget;

            hook.position = lineEnd.position + hook.up * 2.2f;

            if (lr.positionCount > 2 && !grapple.getIsGrappling) lr.positionCount = 2;

            var gunTip = hookStart.position;

            grappleTime = Mathf.Lerp(grappleTime, grapple.getIsGrappling ? 1f : 0, Time.deltaTime * 15f);
            lineEnd.position = Vector3.Lerp(gunTip, grapplePoint - inverseHitNormal * 2.2f, grappleTime);

            lr.enabled = grappleTime > 0.01f;

            if (!grapple.getIsGrappling)
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
                var offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value *
                             affectCurve.Evaluate(delta);
                lr.SetPosition(i, Vector3.Lerp(hookStart.position, lineEnd.position, delta) + offset);
            }

            hook.up = inverseHitNormal;
        }

        public void StopGrapple()
        {
            lineEndTarget = hookStart.position;
            CmdUpdateLinePosition(hookStart.position);

            hook.parent = movement.cam.transform;
            CmdUpdateHookParent(true);
            lineEnd.parent = movement.cam.transform;
            CmdUpdateLineEndParent(true);

            ChangeHookRenderLayer(8);
            LineDismantle();

            anim.StartPlayback();
        }

        private void UpdateSpring()
        {
            if (!grapple.getIsGrappling) spring.Reset();

            if (lr.positionCount == 2) spring.SetVelocity(speed);

            spring.SetDamper(graphicsDamper);
            spring.SetStrength(strength);
            spring.Update(Time.deltaTime);
        }

        public void LineDismantle()
        {
            hook.localRotation = spearOrientation.localRotation;
            CmdUpdateSpearOrientation();
            sway.enabled = true;
            spearAnim.enabled = true;
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

        public void AnimateIn()
        {
            //anim.StopPlayback();
            //anim.Rebind();
            //anim.Update(0f);
            anim.SetTrigger("Reset");
        }

        #region Remote Calls

        [Command]
        private void CmdUpdateLineEndParent(bool camIsParent)
        {
            RpcUpdateLineEndParent(camIsParent);
        }

        [ClientRpc(includeOwner = false)]
        private void RpcUpdateLineEndParent(bool camIsParent)
        {
            if (camIsParent) lineEnd.parent = movement.cam.transform;
            else lineEnd.parent = movement.cam.transform.parent;
        }

        [Command]
        private void CmdUpdateHookParent(bool camIsParent)
        {
            RpcUpdateHookParent(camIsParent);
        }

        [ClientRpc(includeOwner = false)]
        private void RpcUpdateHookParent(bool camIsParent)
        {
            if (camIsParent) lineEnd.parent = movement.cam.transform;
            else lineEnd.parent = movement.cam.transform.parent;
        }

        [Command]
        private void CmdUpdateLinePosition(Vector3 newPos)
        {
            RpcUpdateLinePosition(newPos);
        }

        [ClientRpc(includeOwner = false)]
        private void RpcUpdateLinePosition(Vector3 newPos)
        {
            lineEndTarget = newPos;
            inverseHitNormal = (newPos - movement.transform.position).normalized;
        }

        [Command]
        private void CmdUpdateSpearOrientation()
        {
            RpcUpdateSpearOrientation();
        }

        [ClientRpc(includeOwner = false)]
        private void RpcUpdateSpearOrientation()
        {
            hook.localRotation = spearOrientation.localRotation;
        }

        #endregion
    }
}