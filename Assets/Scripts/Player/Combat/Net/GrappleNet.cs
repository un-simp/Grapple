using System.Collections;
using Mirror;
using UnityEngine;
using Wildflare.Player.Graphics.Net;
using Wildflare.Player.Movement.Net;

namespace Wildflare.Player.Combat.Net
{
    [RequireComponent(typeof(PlayerMovementNet))]
    [RequireComponent(typeof(GrappleGraphicsNet))]
    public class GrappleNet : NetworkBehaviour
    {
        public float grappleRange = 100;
        public Vector3 hitPos;
        public LayerMask swingable;

        [Header("Physics Feel")] [Tooltip("Amount that the spring is reduced when active.")]
        public float damper = 2;

        [Tooltip("Strength of the spring.")] public float springiness = 5;

        public float mass = 5;
        private bool canGrapple = true;
        private GliderNet glider;
        private GrappleGraphicsNet graphics;
        public RaycastHit hitInfo;

        private SpringJoint joint;
        private PlayerMovementNet movement;
        private Rigidbody rb;

        public bool getIsGrappling { get; set; }

        public bool getisActiveWeapon { get; set; } = true;

        private void Awake()
        {
            movement = GetComponent<PlayerMovementNet>();
            glider = GetComponent<GliderNet>();
            graphics = GetComponent<GrappleGraphicsNet>();
            rb = GetComponent<Rigidbody>();
        }

        [Client]
        private void Update()
        {
            if (!movement.hasAuthority || !getisActiveWeapon) return;
            if (!canGrapple) return;

            GrappleManager();

            if (!movement.isActive) return;
            movement.isGrappling = getIsGrappling;
        }

        private void FixedUpdate()
        {
            if (!isClient || !hasAuthority) return;
            if (!canGrapple) return;

            if (getIsGrappling || !getisActiveWeapon) DoGrapple();
        }

        private void GrappleManager()
        {
            if (Input.GetKey(KeyCode.Mouse0) && !getIsGrappling && CanGrapple() && !glider.isGliding)
                InstantStart();
            else if (Input.GetKeyUp(KeyCode.Mouse0) && getIsGrappling) InstantStop();
        }

        private void InstantStart()
        {
            StartGrapple();
            getIsGrappling = true;
            CmdSetIsGrappling(true);
        }

        public void InstantStop()
        {
            StopGrapple();
            getIsGrappling = false;
            CmdSetIsGrappling(false);
        }

        private void StartGrapple()
        {
            joint = gameObject.AddComponent<SpringJoint>();

            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = hitPos;

            var distanceFromPoint = Vector3.Distance(transform.position, joint.connectedAnchor);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = springiness;
            joint.damper = damper;
            joint.massScale = mass;


            movement.speed = movement.speedOnStart / 3;
            if (movement.maxVelocity < movement.maxVelocityOnStart * 2)
                movement.maxVelocity = movement.maxVelocityOnStart * 2;

            graphics.StartGrapple(hitInfo);
        }

        private void DoGrapple()
        {
            if (joint == null) return;
            var vecToPlayer = (joint.connectedAnchor - transform.position).normalized;
            rb.AddForce(vecToPlayer * movement.upthrust);
        }

        private void StopGrapple()
        {
            movement.speed = movement.speedOnStart;
            Destroy(GetComponent<SpringJoint>());
            graphics.StopGrapple();
        }

        public bool CanGrapple()
        {
            RaycastHit hit;
            var Raycast = Physics.Raycast(movement.cam.transform.position, movement.cam.transform.forward, out hit,
                grappleRange);
            if (Raycast && !getIsGrappling)
            {
                if (hit.transform.gameObject.layer == 8)
                {
                    hitInfo = hit;
                    hitPos = hit.point;
                    return true;
                }

                return false;
            }

            return false;
        }

        public void OnSelect()
        {
            getisActiveWeapon = true;
            CmdSetIsActiveWeapon(true);
            StartCoroutine(CanGrappleDelay());
        }

        private IEnumerator CanGrappleDelay()
        {
            yield return new WaitForSeconds(0.55f);
            canGrapple = true;
        }

        public void OnDeselect()
        {
            canGrapple = false;
            getisActiveWeapon = false;
            CmdSetIsActiveWeapon(true);
            InstantStop();
            CmdCallGrappleStop();
        }

        [Command]
        private void CmdSetIsActiveWeapon(bool _state)
        {
            RpcSetIsActiveWeapon(_state);
        }

        [ClientRpc(includeOwner = false)]
        private void RpcSetIsActiveWeapon(bool _state)
        {
            getisActiveWeapon = _state;
        }

        [Command]
        private void CmdCallGrappleStop()
        {
            RpcCallGrappleStop();
        }

        [ClientRpc(includeOwner = false)]
        private void RpcCallGrappleStop()
        {
            InstantStop();
        }

        [Command]
        private void CmdSetIsGrappling(bool _isGrappling)
        {
            RpcSetIsGrappling(_isGrappling);
        }

        [ClientRpc(includeOwner = false)]
        private void RpcSetIsGrappling(bool _isGrappling)
        {
            getIsGrappling = _isGrappling;
        }
    }
}