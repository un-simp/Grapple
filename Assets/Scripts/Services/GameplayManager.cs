using UnityEngine;
using Wildflare.UI.Settings;

namespace Wildflare.Services
{
    public class GameplayManager : MonoBehaviour
    {
        public static GameplayManager singleton;

        void Awake() => singleton = this;

        public void SetSensitivity(float value)
        {
            SettingsManager.SetSensitivity(value);
        }
    }
}