using System.Collections;
using Mirror;
using UnityEngine;
using Wildflare.Player.Interfaces;
using Wildflare.Player.Movement.Net;
using Random = UnityEngine.Random;

namespace Wildflare.Player.Combat.Net
{
    public class GlockNet : NetworkBehaviour, IHoldable, IWeapon
    {
        [SerializeField] GameObject bullet;
        [SerializeField] Transform cam;
        [SerializeField]private Animator anim;
        public Transform tip;
        public Transform Tip => tip;
        public GameObject bullet_g => bullet;
        GameObject currentlySpawnedBullet = null;
        private Vector3 posOnStart;
        
        Vector3 hitPoint;
        int bulletForce = 600;
        public float damage;
        private WeaponNet _weapon;
        public float Damage => damage;
        
        [SerializeField] private GameObject explosion;

        private PlayerMovementNet movement;

        private bool isActiveWeapon = false;
        
        

        //If this is true then the AnimationManager will be called and animation times will
        //be based on player speed
        
        private static readonly int doShoot = Animator.StringToHash("doShoot");

        public void OnSelect() {
            if(_weapon == null) _weapon = transform.root.GetComponent<WeaponNet>();
            isActiveWeapon = true;
            _weapon.enabled = true;
            gameObject.SetActive(true);
            StartCoroutine(_weapon.OnSelect());
        }

        public void OnDeselect() {
            if(_weapon == null) _weapon = transform.root.GetComponent<WeaponNet>();
            isActiveWeapon = false;
            _weapon.enabled = false;
        }

        public void AnimateIn() {
            anim.SetTrigger("Reset");
        }

        public void Shoot(RaycastHit _hitInfo) => ShootGraphics(_hitInfo);

        void Awake() {
            Transform root = transform.root;
            movement = root.GetComponent<PlayerMovementNet>();
        }

        void Start() {
            posOnStart = transform.localPosition;
        }
        
        void ShootGraphics(RaycastHit _hitInfo)
        {
            //Find active weapon
            MonoBehaviour currentWeapon = null;

            foreach(Transform child in cam)
            {
                if(child.TryGetComponent(out IHoldable __weapon))
                {
                    if(child.gameObject.activeSelf == false) continue;
                    currentWeapon = __weapon as MonoBehaviour;
                    break;
                }
            }
            if(currentWeapon == null) return;
            var tipPos = tip.position;
            hitPoint = _hitInfo.point;
            BulletInfo info = new BulletInfo(tipPos, hitPoint, bulletForce);
            //Client
            SpawnBullet(info);
            StartCoroutine(BulletDestroyer());
            //Server
            CmdGlockSpawnBullet(info);
        }
        
        [Client]
        private void Update() {
            if(!isActiveWeapon) return;
            //AnimationManager();
        }

        void AnimationManager() {
            var animInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (movement.currentVelocity < 6) anim.speed = movement.currentVelocity / 12;
            else anim.speed = 1;
            
            if (movement.currentVelocity <= 0.2f || !movement.isGrounded) {
                anim.enabled = false;
                transform.localPosition = Vector3.Lerp(transform.localPosition, posOnStart, Time.deltaTime * 5);
            }
            else if (!anim.enabled  && movement.isGrounded) {
                anim.enabled = true;
            }
        }

        public void SpawnBullet(BulletInfo _info)
        {
            currentlySpawnedBullet = Instantiate(bullet, _info.tipPosition, Quaternion.identity);
            currentlySpawnedBullet.transform.GetComponentInChildren<TrailRenderer>().AddPosition(_info.tipPosition);
            var exp = Instantiate(explosion, _info.tipPosition, Random.rotation);
            print("Spawned Bullet");
            exp.transform.parent = this.transform;
            if (_info.hitPoint != Vector3.zero) {
                currentlySpawnedBullet.transform.forward = _info.hitPoint - _info.tipPosition;
                currentlySpawnedBullet.transform.position = _info.hitPoint;
            }
            else {
                var forward = cam.forward;
                currentlySpawnedBullet.transform.forward = forward;
                currentlySpawnedBullet.transform.position = cam.position + (forward * 100);
            }

            Invoke(nameof(DestroyBullet), 0.1f);

            anim.SetTrigger(doShoot);
        }

        void DestroyBullet() => Destroy(currentlySpawnedBullet);

        IEnumerator BulletDestroyer()
        {
            yield return new WaitForSeconds(2f);
            Destroy(currentlySpawnedBullet);
            currentlySpawnedBullet = null;
        }

        public struct BulletInfo{
            public Vector3 tipPosition;
            public Vector3 hitPoint;
            public float bulletForce;

            public BulletInfo(Vector3 _tipPosition, Vector3 _hitPoint, float _bulletForce) {
                bulletForce = _bulletForce;
                tipPosition = _tipPosition;
                hitPoint = _hitPoint;
            }
        }
        
        [Command]
        public void CmdGlockSpawnBullet(GlockNet.BulletInfo _info) => RpcGlockSpawnBullet(_info);
        [ClientRpc(includeOwner = false)]
        void RpcGlockSpawnBullet(GlockNet.BulletInfo _info)
        {
            SpawnBullet(_info);
            StartCoroutine(BulletDestroyer());
        }
    }
}


