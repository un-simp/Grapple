using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Wildflare.UI.Settings;

namespace Wildflare.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;
        public static List<AudioSource> sources = new List<AudioSource>();

        [SerializeField] private AudioMixer soundMixer;
        [SerializeField] private AudioMixer musicMixer;

        private void Awake()
        {
            if (instance != null) return;
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            SetSFXVolume(SettingsManager.GetSFXVolume());
            SetMusicVolume(SettingsManager.GetMusicVolume());
        }

        public void SetSFXVolume(float _desiredLinearVolume)
        {
            //Stored As Linear
            SettingsManager.SetSFXVolume(_desiredLinearVolume);
            //Logarithmicly Converted
            soundMixer.SetFloat("SFXVolume", Mathf.Log10(_desiredLinearVolume) * 20);
        }
        
        public void SetMusicVolume(float _desiredLinearVolume)
        {
            //Stored As Linear
            SettingsManager.SetMusicVolume(_desiredLinearVolume);
            //Logarithmicly Converted
            musicMixer.SetFloat("MusicVolume", Mathf.Log10(_desiredLinearVolume) * 20);
        }
    }
}