using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Barji.Services;

namespace Barji
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField]private TMP_Text timerText;
        public float TimeElapsed { get; private set; }
        private bool isIncrementing = false;

        public static LevelManager singleton;

        private void Awake()
        {
            singleton = this;
        }

        void Update()
        {
            if (!isIncrementing) return;
            TimeElapsed += Time.deltaTime;
            timerText.text = TimeElapsed.ToString("F2");
        }
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Start"))
            {
                isIncrementing = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Finish") && isIncrementing)
            {
                isIncrementing = false;
                GameplayManager.singleton.LevelOver();
            }
        }
    }
}
