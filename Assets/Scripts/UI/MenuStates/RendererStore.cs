using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

namespace Wildflare
{
    public class RendererStore : MonoBehaviour {
        [SerializeField] List<TMP_Text> texts = new List<TMP_Text>();
        [SerializeField] List<Image> images = new List<Image>();

        public void TweenOpacity(float _endOpacity, float _time) 
        {
            foreach (var text in texts) {
                Color col = new Color(text.color.r, text.color.g, text.color.b, _endOpacity);
                text.DOColor(col, _time).SetEase(Ease.InExpo);
            }
            foreach (var image in images) 
            {
                Color col = new Color(image.color.r, image.color.g, image.color.b, _endOpacity);
                image.DOColor(col, _time).SetEase(Ease.InExpo);
            }
        }

    }
}
