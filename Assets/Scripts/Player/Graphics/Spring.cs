using UnityEngine;

namespace Barji.Player.Graphics
{
    public class Spring
    {
        private float damper;
        private float strength;
        private float target;
        private float velocity;

        public float Value { get; private set; }

        public void Update(float deltaTime)
        {
            var direction = target - Value >= 0 ? 1f : -1f;
            var force = Mathf.Abs(target - Value) * strength;
            velocity += (force * direction - velocity * damper) * deltaTime;
            Value += velocity * deltaTime;
        }

        public void Reset()
        {
            velocity = 0f;
            Value = 0f;
        }

        public void SetValue(float value)
        {
            this.Value = value;
        }

        public void SetTarget(float target)
        {
            this.target = target;
        }

        public void SetDamper(float damper)
        {
            this.damper = damper;
        }

        public void SetStrength(float strength)
        {
            this.strength = strength;
        }

        public void SetVelocity(float velocity)
        {
            this.velocity = velocity;
        }
    }
}