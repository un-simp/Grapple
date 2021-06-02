using UnityEngine;
using TMPro;
using Wildflare.Player.Movement;

namespace Wildflare.Player.Graphics {
    public class PlayerGraphics : MonoBehaviour {
        
        private PlayerMovement movement;
        [SerializeField]private TMP_Text velocityTxt;

        [SerializeField] private GameObject impactParticles;
        [SerializeField] private Material impactMat;

        void Start() {
            movement = GetComponent<PlayerMovement>();
        }

        public void Update() {
            velocityTxt.text = movement.currentVelocity.ToString("F2");
        }

        public void SpawnGroundImpact(Vector3 _position, Material _other) {
            impactMat.color = _other.color;
            impactMat.mainTexture = _other.mainTexture != null ? _other.mainTexture : null;
            Instantiate(impactParticles, _position, impactParticles.transform.rotation);
        }
        
        public void SpawnWallImpact(Vector3 _position, Material _other, Vector3 _normal) {
            impactMat.color = _other.color;
            impactMat.mainTexture = _other.mainTexture != null ? _other.mainTexture : null;
            GameObject particle = Instantiate(impactParticles, _position, impactParticles.transform.rotation);
            particle.transform.forward = _normal;
        }
    }
}