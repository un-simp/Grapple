using UnityEngine;

namespace Wildflare.UI.Settings
{
    public sealed class SettingsManager : MonoBehaviour
    {
        public static SettingsManager singleton;

        void Awake() => singleton ??= this;

        //Volume
        public void SetSFXVolume(float value) => PlayerPrefs.SetFloat("SFXVolume", value);
        public float GetSFXVolume() => PlayerPrefs.GetFloat("SFXVolume");
        public void SetMusicVolume(float value) => PlayerPrefs.SetFloat("MusicVolume", value);
        public float GetMusicVolume() => PlayerPrefs.GetFloat("MusicVolume");
        //Sensitivity
        public void SetSensitivity(int value) => PlayerPrefs.SetFloat("Sensitivity", value);
        public float GetSensitivity() => PlayerPrefs.GetFloat("Sensitivity");
        
        //Resolution
        /// <summary>
        /// 0: 720p 1: 1080p, 2: 1440p, 3: 1080p Ultrawide
        /// </summary>
        public void SetResolution(int value) => PlayerPrefs.SetInt("Resolution", value);
        /// <summary>
        /// 0: 720p 1: 1080p, 2: 1440p, 3: 1080p Ultrawide
        /// </summary>
        public int GetResolution() => PlayerPrefs.GetInt("Resolution");

        //IsFullscreen 0: false, 1: true
        public void SetIsFullscreen(int value) => PlayerPrefs.SetInt("IsFullscreen", value);
        public int GetIsFullscreen() => PlayerPrefs.GetInt("IsFullscreen");
        
        //RefreshRate
        /// <summary>
        /// 0: 60hz, 100hz, 120hz, 144hz, 240hz
        /// </summary>
        public void SetRefreshRate(int value) => PlayerPrefs.SetInt("RefreshRate", value);
        //RefreshRate
        /// <summary>
        /// 0: 60hz, 100hz, 120hz, 144hz, 240hz
        /// </summary>
        public int GetRefreshRate() => PlayerPrefs.GetInt("RefreshRate");
    }
}