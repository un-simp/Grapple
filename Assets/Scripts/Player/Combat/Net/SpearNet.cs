using UnityEngine;
using Wildflare.Player.Graphics.Net;
using Wildflare.Player.Interfaces;

namespace Wildflare.Player.Combat.Net
{
    public class SpearNet : MonoBehaviour, IHoldable
    {
        public GrappleNet grapple;
        public GrappleGraphicsNet grappleGraphics;

        public void OnSelect()
        {
            gameObject.SetActive(true);
            grapple.OnSelect();
        }

        public void OnDeselect()
        {
            grapple.OnDeselect();
        }

        public void AnimateIn()
        {
            grappleGraphics.AnimateIn();
        }
    }
}