using UnityEngine;

namespace MilkShake
{
    /// <summary>
    ///     ScriptableObject asset implementation of the IShakeParameters interface.
    /// </summary>
    [CreateAssetMenu(fileName = "New Shake Preset", menuName = "MilkShake/Shake Preset")]
    public class ShakePreset : ScriptableObject, IShakeParameters
    {
        [Header("Shake Type")] [SerializeField]
        private ShakeType shakeType;

        [Header("Shake Strength")] [SerializeField]
        private float strength;

        [SerializeField] private float roughness;

        [Header("Fade")] [SerializeField] private float fadeIn;

        [SerializeField] private float fadeOut;

        [Header("Shake Influence")] [SerializeField]
        private Vector3 positionInfluence;

        [SerializeField] private Vector3 rotationInfluence;

        /// <summary>
        ///     The type of shake (One-Shot or Sustained)
        /// </summary>
        public ShakeType ShakeType
        {
            get => shakeType;
            set => shakeType = value;
        }

        /// <summary>
        ///     The intensity / magnitude of the shake.
        /// </summary>
        public float Strength
        {
            get => strength;
            set => strength = value;
        }

        /// <summary>
        ///     The roughness of the shake.
        ///     Lower values are slower and smoother, higher values are faster and noisier.
        /// </summary>
        public float Roughness
        {
            get => roughness;
            set => roughness = value;
        }

        /// <summary>
        ///     The time, in seconds, for the shake to fade in.
        /// </summary>
        public float FadeIn
        {
            get => fadeIn;
            set => fadeIn = value;
        }

        /// <summary>
        ///     The time, in seconds, for the shake to fade out.
        /// </summary>
        public float FadeOut
        {
            get => fadeOut;
            set => fadeOut = value;
        }

        /// <summary>
        ///     How much influence the shake has over the camera's position.
        ///     All values are valid, even numbers greater than 1 and negative numbers.
        /// </summary>
        public Vector3 PositionInfluence
        {
            get => positionInfluence;
            set => positionInfluence = value;
        }

        /// <summary>
        ///     How much influence the shake has over the camera's rotation.
        ///     All values are valid, even numbers greater than 1 and negative numbers.
        /// </summary>
        public Vector3 RotationInfluence
        {
            get => rotationInfluence;
            set => rotationInfluence = value;
        }
    }
}