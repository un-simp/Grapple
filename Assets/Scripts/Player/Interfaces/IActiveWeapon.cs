using UnityEngine;

namespace Barji.Player.Interfaces
{
    public interface IAnimatable
    {
        void AnimateIn();
    }

    public interface IWeapon : IAnimatable
    {
        Transform Tip { get; }
        float Damage { get; }
        void Shoot(RaycastHit _hitInfo);
    }

    public interface IHoldable : IAnimatable
    {
        void OnSelect();
        void OnDeselect();
    }
}