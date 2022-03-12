using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Barji.UI.Settings;

namespace Barji.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioClip[] musics;
        public static List<AudioSource> sources = new List<AudioSource>();

        [SerializeField] private AudioMixer soundMixer;
        [SerializeField] private AudioMixer musicMixer;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
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

        public void OnSceneLoad()
        {
            var newClip = musics[SceneManager.GetActiveScene().buildIndex];
            if (newClip == musicSource.clip) return;
            musicSource.Stop();
            musicSource.clip = newClip;
            musicSource.Play();
        }
    }
}