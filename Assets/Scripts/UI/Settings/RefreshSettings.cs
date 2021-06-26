using UnityEngine;
using Wildflare.UI.Interfaces;

namespace Wildflare.UI.Settings
{
    public class RefreshSettings : MonoBehaviour, ISettingLogic
    {
        public void ApplySettings(int index)
        {
            switch (index)
            {
                case 0:
                    Application.targetFrameRate = 60;
                    break;
                case 1:
                    Application.targetFrameRate = 100;
                    break;
                case 2:
                    Application.targetFrameRate = 120;
                    break;
                case 3:
                    Application.targetFrameRate = 144;
                    break;
                case 4:
                    Application.targetFrameRate = 240;
                    break;
                default:
                    Application.targetFrameRate = 60;
                    break;
            }
            SettingsManager.SetRefreshRate(index);
        }

        public int GetCurrentSetting() => SettingsManager.GetRefreshRate();
    }
}