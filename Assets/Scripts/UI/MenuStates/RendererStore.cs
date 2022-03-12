using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Barji
{
    public class RendererStore : MonoBehaviour
    {
        [SerializeField] private List<TMP_Text> texts = new List<TMP_Text>();
        [SerializeField] private List<Image> images = new List<Image>();

        public void TweenOpacity(float _endOpacity, float _time)
        {
            foreach (var text in texts)
            {
                var col = new Color(text.color.r, text.color.g, text.color.b, _endOpacity);
                text.DOColor(col, _time).SetEase(Ease.InExpo);
            }

            foreach (var image in images)
            {
                var col = new Color(image.color.r, image.color.g, image.color.b, _endOpacity);
                image.DOColor(col, _time).SetEase(Ease.InExpo);
            }
        }
    }
}