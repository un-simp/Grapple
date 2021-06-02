using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI.ProceduralImage;
using Wildflare.Sounds;

namespace Wildflare.UI
{
    [RequireComponent(typeof(UISounds))]
    public class TrimPathsAnimator : MonoBehaviour {
        public float time;
        private ProceduralImage pimage => GetComponent<ProceduralImage>();

        public void Fill() {
            pimage.DOFillAmount(1, time).SetEase(Ease.OutExpo);
            Color col = pimage.color;
            Color colFull = new Color(col.r, col.g, col.b, 0.7f);
            pimage.DOColor(colFull, time);
        }

        public void Empty() {
            Color col = pimage.color;
            Color colEmpty = new Color(col.r, col.g, col.b, 0.2f);
            pimage.DOFillAmount(0, time).SetEase(Ease.InExpo);
            pimage.DOColor(colEmpty, time);
        }
    }
}
