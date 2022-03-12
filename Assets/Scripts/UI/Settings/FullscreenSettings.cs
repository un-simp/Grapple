using UnityEngine;
using Barji.UI.Interfaces;

namespace Barji.UI.Settings
{
    public class FullscreenSettings : MonoBehaviour, ISettingLogic
    {
        void Update()
        {
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Return))
            {
                ApplySettings(Screen.fullScreen ? 1 : 0);
            }
        }
        public void ApplySettings(int index)
        {
            switch (index)
            {
                case 0:
                    Screen.fullScreen = false;
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    break;
                case 1:
                    Screen.fullScreen = true;
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    break;
                default:
                    Screen.fullScreen = true;
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    break;
            }
            SettingsManager.SetIsFullscreen(index);
        }

        public int GetCurrentSetting()
        {
            return SettingsManager.GetIsFullscreen();
        }
    }
}