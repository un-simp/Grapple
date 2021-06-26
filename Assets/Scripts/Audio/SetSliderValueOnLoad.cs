using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Wildflare.UI.Settings;

namespace Wildflare.Audio
{
    public class SetSliderValueOnLoad : MonoBehaviour
    {
        enum SliderType
        {
            SFX, Music, Sensitivity
        }

        [SerializeField] private SliderType sliderType;
        void Start()
        {
            switch (sliderType)
            {
                case SliderType.SFX:
                    GetComponent<Slider>().value = SettingsManager.GetSFXVolume();
                    break;
                case SliderType.Music:
                    GetComponent<Slider>().value = SettingsManager.GetMusicVolume();
                    break;
                case SliderType.Sensitivity:
                    GetComponent<Slider>().value = SettingsManager.GetSensitivity();
                    break;
            }
        }
    }
}
