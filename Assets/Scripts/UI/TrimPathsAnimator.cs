using DG.Tweening;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;
using Barji.Sounds;

namespace Barji.UI
{
    [RequireComponent(typeof(UISounds))]
    public class TrimPathsAnimator : MonoBehaviour
    {
        public float time;
        private ProceduralImage pimage => GetComponent<ProceduralImage>();

        public void Fill()
        {
            pimage.DOFillAmount(1, time).SetEase(Ease.OutExpo);
            var col = pimage.color;
            var colFull = new Color(col.r, col.g, col.b, 0.7f);
            pimage.DOColor(colFull, time);
        }

        public void Empty()
        {
            var col = pimage.color;
            var colEmpty = new Color(col.r, col.g, col.b, 0.2f);
            pimage.DOFillAmount(0, time).SetEase(Ease.InExpo);
            pimage.DOColor(colEmpty, time);
        }
    }
}