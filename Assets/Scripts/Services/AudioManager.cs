using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Wildflare.Audio
{
    public class AudioManager : MonoBehaviour
    {
        private const string key = "SFXVolume";
        public static AudioManager instance;
        public static List<AudioSource> sources = new List<AudioSource>();

        [SerializeField] private AudioMixer mixer;

        private void Awake()
        {
            if (instance != null) return;
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            SetVolume(PlayerPrefs.GetFloat(key));
            UpdateSlider();
        }

        public void UpdateSlider()
        {
            var sliders = FindObjectsOfType<Slider>();
            foreach (var slider in sliders)
                if (slider.CompareTag("Volume"))
                    slider.value = PlayerPrefs.GetFloat(key);
        }

        public void SetVolume(float _desiredLinearVolume)
        {
            //Stored As Linear
            PlayerPrefs.SetFloat(key, _desiredLinearVolume);
            //Logarithmicly Converted
            mixer.SetFloat(key, Mathf.Log10(_desiredLinearVolume) * 20);
        }
    }
}