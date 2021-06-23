using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Wildflare.UI.Interfaces;

namespace Wildflare.UI.Settings
{
    public class Scroller : MonoBehaviour
    {
        private int currentItem;
        [SerializeField] private TMP_Text itemTxt;
        [SerializeField] string[] items = new string[0];
        [SerializeField] private MonoBehaviour setting;
        private ISettingLogic settingLogic;
        
        private void OnValidate()
        {
            if (!(setting is ISettingLogic))
                setting = null;
        }

        private void Awake()
        {
            settingLogic = setting as ISettingLogic;
        }

        void Start()
        {
            if (settingLogic == null) return;
            currentItem = settingLogic.GetCurrentSetting();
            Apply();
        }

        public void NextItem()
        {
            if(currentItem == items.Length-1) return;
            currentItem++;
            Apply();
        }
        
        public void PreviousItem()
        {
            if(currentItem == 0) return;
            currentItem--;
            Apply();
        }

        void Apply()
        {
            itemTxt.text = items[currentItem];
            settingLogic.ApplySettings(currentItem);
        }
    }
}
