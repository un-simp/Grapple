using UnityEngine;
using Wildflare.UI.Interfaces;
using System;

namespace Wildflare.UI.Settings
{
    public class ResolutionSettings : MonoBehaviour, ISettingLogic
    {
        void Start() => ApplySettings(SettingsManager.GetResolution());
        
        public void ApplySettings(int index)
        {
            switch (index)
            {
                case 0: 
                    Screen.SetResolution(1280, 720, PlayerPrefs.GetInt("IsFullscreen") != 0);
                    break;
                case 1: 
                    Screen.SetResolution(1920, 1080, PlayerPrefs.GetInt("IsFullscreen") != 0);
                    break;
                case 2: 
                    Screen.SetResolution(2560, 1440, PlayerPrefs.GetInt("IsFullscreen") != 0);
                    break;
                case 3:
                    Screen.SetResolution(2560, 1440, PlayerPrefs.GetInt("IsFullscreen") != 0);
                    break;
                default:
                    Screen.SetResolution(1920, 1080, PlayerPrefs.GetInt("IsFullscreen") != 0);
                    break;
            }
            SettingsManager.SetResolution(index);
        }

        public int GetCurrentSetting() => SettingsManager.GetResolution();
    }
}