using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wildflare.Player.Graphics;
using Wildflare.Player.Interfaces;

namespace Wildflare.Player.Combat
{
    public class Spear : MonoBehaviour, IHoldable
    {
        public Grapple grapple;
        public GrappleGraphics grappleGraphics;

        public void OnSelect(){
            gameObject.SetActive(true);
            grapple.OnSelect();
        }
        public void OnDeselect(){
            grapple.OnDeselect();
        }
        
        public void AnimateIn() {
            grappleGraphics.AnimateIn();
        }
    }
}

