using System;
using UnityEngine;

namespace Wildflare.UI.Settings
{
    public static class SettingsManager
    {

        public static Action onSensitivityChanged;
        static SettingsManager()
        {
            //PlayerPrefs.DeleteKey("FirstOpenInstance"); <- used for testing
            if (!PlayerPrefs.HasKey("FirstOpenInstance"))
            {
                PlayerPrefs.SetInt("FirstOpenInstance", 1);
                SetSFXVolume(0.5f);
                SetMusicVolume(0.5f);
                SetSensitivity(0.5f);
                SetIsFullscreen(1);
                SetResolution(1);
                SetRefreshRate(0);
            }
        }

        //Volume
        public static void SetSFXVolume(float value) => PlayerPrefs.SetFloat("SFXVolume", value);
        public static float GetSFXVolume() => PlayerPrefs.GetFloat("SFXVolume");
        public static void SetMusicVolume(float value) => PlayerPrefs.SetFloat("MusicVolume", value);
        public static float GetMusicVolume() => PlayerPrefs.GetFloat("MusicVolume");
        //Sensitivity
        public static void SetSensitivity(float value)
        {
            PlayerPrefs.SetFloat("Sensitivity", value);
            if(onSensitivityChanged != null)
                onSensitivityChanged.Invoke();
        }
        public static float GetSensitivity() => PlayerPrefs.GetFloat("Sensitivity");
        
        //Resolution
        /// <summary>
        /// 0: 720p 1: 1080p, 2: 1440p, 3: 1080p Ultrawide
        /// </summary>
        public static void SetResolution(int value) => PlayerPrefs.SetInt("Resolution", value);
        /// <summary>
        /// 0: 720p 1: 1080p, 2: 1440p, 3: 1080p Ultrawide
        /// </summary>
        public static int GetResolution() => PlayerPrefs.GetInt("Resolution");

        //IsFullscreen 0: false, 1: true
        public static void SetIsFullscreen(int value) => PlayerPrefs.SetInt("IsFullscreen", value);
        public static int GetIsFullscreen() => PlayerPrefs.GetInt("IsFullscreen");
        
        //RefreshRate
        /// <summary>
        /// 0: 60hz, 100hz, 120hz, 144hz, 240hz
        /// </summary>
        public static void SetRefreshRate(int value) => PlayerPrefs.SetInt("RefreshRate", value);
        //RefreshRate
        /// <summary>
        /// 0: 60hz, 100hz, 120hz, 144hz, 240hz
        /// </summary>
        public static int GetRefreshRate() => PlayerPrefs.GetInt("RefreshRate");
    }
}