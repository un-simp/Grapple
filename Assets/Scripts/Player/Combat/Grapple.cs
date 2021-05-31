using System.Collections;
using UnityEngine;
using Wildflare.Player.Graphics;
using Mirror;

namespace Wildflare.Player.Combat
{
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(GrappleGraphics))]
    public class Grapple : NetworkBehaviour
    {
        private PlayerMovement movement;   
        private Glider glider;
        private GrappleGraphics graphics;
        private Rigidbody rb;
        private SpringJoint joint;

        public bool getIsGrappling
        {
            get{ return isGrappling; }
            set{ isGrappling = value; }
        }
        public float grappleRange = 100;
        public Vector3 hitPos;
        public RaycastHit hitInfo;
        public LayerMask swingable;

        [Header("Physics Feel")]
        [Tooltip("Amount that the spring is reduced when active.")]
        public float damper = 2;
        [Tooltip("Strength of the spring.")]
        public float springiness = 5;
        public float mass = 5;

        private bool isGrappling;
        private bool canGrapple = true;

        private bool isActiveWeapon = true;

        public bool getisActiveWeapon
        {
            get => isActiveWeapon;
            set => isActiveWeapon = value;
        }

        void Awake()
        {
            movement = GetComponent<PlayerMovement>();
            glider = GetComponent<Glider>();
            graphics = GetComponent<GrappleGraphics>();
            rb = GetComponent<Rigidbody>();
        }

        [Client]
        void Update()
        {   
            if(!hasAuthority || !isActiveWeapon) return;
            if (!canGrapple) return;
            
            GrappleManager();

            if(!movement.isActive) return;
            movement.isGrappling = isGrappling;
        }

        void FixedUpdate()
        {
            if(!isClient || !hasAuthority) return;
            if (!canGrapple) return;

            if(isGrappling || !isActiveWeapon){
                DoGrapple();
            }
        }

        void GrappleManager()
        {
            if(Input.GetKey(KeyCode.Mouse0) && !isGrappling && CanGrapple() && !glider.isGliding){
                InstantStart();
            }
            else if(Input.GetKeyUp(KeyCode.Mouse0) && isGrappling){
                InstantStop();
            }
        }

        void InstantStart()
        {
            StartGrapple();
            isGrappling = true;
            CmdSetIsGrappling(true);
        }
        public void InstantStop()
        {
            StopGrapple();
            isGrappling = false;
            CmdSetIsGrappling(false);
        }

        void StartGrapple()
        {   

            joint = gameObject.AddComponent<SpringJoint>();

            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = hitPos;

            float distanceFromPoint = Vector3.Distance(transform.position, joint.connectedAnchor);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = springiness;
            joint.damper = damper;
            joint.massScale = mass;


            movement.speed = movement.speedOnStart / 3;
            if(movement.maxVelocity < movement.maxVelocityOnStart *2){
                movement.maxVelocity = movement.maxVelocityOnStart * 2;
            }

            graphics.StartGrapple(hitInfo);
        }   

        void DoGrapple()
        {
            if(joint == null) return;
            var vecToPlayer = (joint.connectedAnchor - transform.position).normalized;
            rb.AddForce(vecToPlayer * movement.upthrust);
        }

        void StopGrapple()
        {
            movement.speed = movement.speedOnStart;
            Destroy(GetComponent<SpringJoint>());
            graphics.StopGrapple();
        }

        public bool CanGrapple(){
            RaycastHit hit;
            bool Raycast = Physics.Raycast(movement.cam.transform.position, movement.cam.transform.forward, out hit, grappleRange);
            if(Raycast && !isGrappling){
                if(hit.transform.gameObject.layer == 8){
                    hitInfo = hit;
                    hitPos = hit.point;
                    return true;
                }
                return false;
            }
            else{
                return false;
            }
        }

        public void OnSelect(){
            getisActiveWeapon = true;
            CmdSetIsActiveWeapon(true);
            StartCoroutine(CanGrappleDelay());
        }

        IEnumerator CanGrappleDelay() {
            yield return new WaitForSeconds(0.55f);
            canGrapple = true;
        }
        public void OnDeselect() {
            canGrapple = false;
            getisActiveWeapon = false;
            CmdSetIsActiveWeapon(true);
            InstantStop();
            CmdCallGrappleStop();
        }

        [Command] void CmdSetIsActiveWeapon(bool _state) => RpcSetIsActiveWeapon(_state);
        [ClientRpc(includeOwner=false)] void RpcSetIsActiveWeapon(bool _state) => getisActiveWeapon = _state;

        [Command] void CmdCallGrappleStop() => RpcCallGrappleStop();
        [ClientRpc(includeOwner=false)] void RpcCallGrappleStop() => InstantStop();
        
        [Command]void CmdSetIsGrappling(bool _isGrappling) => RpcSetIsGrappling(_isGrappling);
        [ClientRpc(includeOwner=false)] void RpcSetIsGrappling(bool _isGrappling) => isGrappling = _isGrappling;
    }
}
