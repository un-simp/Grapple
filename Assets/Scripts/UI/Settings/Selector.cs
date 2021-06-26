using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wildflare.UI.Interfaces;
using Wildflare.UI.Settings;

namespace Wildflare
{
    public class Selector : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour setting;
        private ISettingLogic settingLogic;
        [SerializeField] private GameObject selectedImage;

        private bool isEnabled;
        
        private void OnValidate()
        {
            if (!(setting is ISettingLogic))
                setting = null;
        }

        private void Awake()
        {
            settingLogic = setting as ISettingLogic;
        }

        private void Start()
        {
            if (settingLogic == null) return;
            isEnabled = SettingsManager.GetIsFullscreen() != 0;
            Apply();
        }

        public void ChangeState()
        {
            isEnabled = !isEnabled;
            Apply();
        }

        void Apply()
        {
            selectedImage.SetActive(isEnabled);
            settingLogic.ApplySettings(isEnabled ? 1 : 0);
        }
    }
}
