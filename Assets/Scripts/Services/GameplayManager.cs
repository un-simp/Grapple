using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Wildflare.UI.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Wildflare.Audio;
using Wildflare.Player.Movement;


namespace Wildflare.Services
{
    public class GameplayManager : MonoBehaviour
    {
        public static GameplayManager singleton;
        [SerializeField] private GameObject endScreen;
        [SerializeField] private Image endPanel;
        [SerializeField] private TMP_Text timerText;

        void Awake()
        {
            if (singleton == null)
            {
                singleton = this;
                SceneManager.sceneLoaded += OnSceneLoad;
            }
            else
                Destroy(this.gameObject);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartLevel();
            }
        }

        public void SetSensitivity(float value)
        {
            SettingsManager.SetSensitivity(value);
        }

        public void RestartLevel()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
        
        //Called when a level is started
        public void LevelStart()
        {
            endScreen.SetActive(false);
            endPanel.color = new Color(255, 255, 255, 0);
            foreach (Transform child in endPanel.transform)
            {
                TMP_Text text = child.GetComponent<TMP_Text>();
                //Lower Alpha to 0
                child.GetComponent<TMP_Text>().color = new Color(text.color.r, text.color.g, text.color.b, 0);
                //Move up each element
                child.transform.localPosition += new Vector3(0, 50, 0);
                EventTrigger t;
                if (child.TryGetComponent(out t))
                    t.enabled = false;
                child.gameObject.SetActive(false);
            }
        }
        
        //Called when a level is completed
        public void LevelOver()
        {
            Cursor.lockState = CursorLockMode.None;
            timerText.text = LevelManager.singleton.TimeElapsed.ToString("F2");
            endScreen.SetActive(true);
            endPanel.DOColor(new Color(255, 255, 255, 255), 1f).SetEase(Ease.InExpo);
            PlayerMovement.currentState = PlayerMovement.state.Stopped;
            GameObject.FindWithTag("Player").GetComponent<Rigidbody>().isKinematic = true;
            StartCoroutine(InAnimation());
        }

        IEnumerator InAnimation()
        {
            var level = endPanel.transform.GetChild(0);
            level.gameObject.SetActive(true);
            TMP_Text levelText = level.GetComponent<TMP_Text>();
            levelText.text = "Level " + SceneManager.GetActiveScene().buildIndex + " Complete";
            level.GetComponent<TMP_Text>().DOColor(new Color(levelText.color.r, levelText.color.g, levelText.color.b, 255), 0.2f).SetEase(Ease.OutExpo);
            level.DOLocalMoveY(level.localPosition.y - 50, 0.2f);
            //Stay on screen for 0.5s
            yield return new WaitForSeconds(0.5f);
            level.DOLocalMoveY(level.localPosition.y - 50, 0.2f);
            level.GetComponent<TMP_Text>().DOColor(new Color(levelText.color.r, levelText.color.g, levelText.color.b, 0), 0.2f).SetEase(Ease.OutExpo);
            yield return new WaitForSeconds(0.2f);
            
            for(int i = 1; i < endPanel.transform.childCount; i++)
            {
                var child = endPanel.transform.GetChild(i);
                child.gameObject.SetActive(true);
                TMP_Text text = child.GetComponent<TMP_Text>();
                child.GetComponent<TMP_Text>().DOColor(new Color(text.color.r, text.color.g, text.color.b, 255), 0.2f).SetEase(Ease.OutExpo);
                child.transform.DOLocalMoveY(child.localPosition.y - 50, 0.2f);
                yield return new WaitForSeconds(0.2f);
                EventTrigger t;
                if (child.TryGetComponent(out t))
                    t.enabled = true;
            }
        }

        void OnSceneLoad(Scene s, LoadSceneMode l)
        {
            LevelStart();
            GetComponent<AudioManager>().OnSceneLoad();
        }
    }
}