using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Barji.UI.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Barji.Audio;
using Barji.Player.Movement;


namespace Barji.Services
{
    public class GameplayManager : MonoBehaviour
    {
        public static GameplayManager singleton;
        [SerializeField] private GameObject endScreen;
        [SerializeField] private Image endPanel;
        [SerializeField] private TMP_Text timerText;

        [SerializeField] private Color endScreenColor;
        [SerializeField] private CanvasGroup leaderboard;

        [SerializeField] private TMP_Text[] leaderboardEntriesTxt;
        [SerializeField] private TMP_Text localScoreTxt;
        public Dictionary<string, float> scores = new Dictionary<string, float>();
        public Dictionary<int, float> localScore = new Dictionary<int, float>();

        void Awake()
        {
            if (singleton == null)
            {
                singleton = this;
            }
            else
                Destroy(this.gameObject);

            OnSceneLoad();
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
            singleton = null;
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
        
        //Called when a level is started
        public void LevelStart()
        {
            if(endScreen == null)
                return;
            endScreen.SetActive(false);
            endPanel.color = new Color(0, 0, 0, 0);
            leaderboard.alpha = 0;
            foreach (Transform child in endPanel.transform)
            {
                TMP_Text text = child.GetComponent<TMP_Text>();
                //Lower Alpha to 0
                child.GetComponent<TMP_Text>().color = new Color(text.color.r, text.color.g, text.color.b, 0);
                //Move up each element
                child.transform.localPosition += new Vector3(0, 50, 0);
                child.gameObject.SetActive(false);
            }
        }
        
        //Called when a level is completed
        public void LevelOver()
        {
            Cursor.lockState = CursorLockMode.None;
            timerText.text = LevelManager.singleton.TimeElapsed.ToString("F2");
            endScreen.SetActive(true);
            endPanel.DOColor(endScreenColor, .2f).SetEase(Ease.InExpo);
            PlayerMovement.currentState = PlayerMovement.state.Stopped;
            GameObject.FindWithTag("Player").GetComponent<Rigidbody>().isKinematic = true;
            StartCoroutine(InAnimation());
            StartCoroutine(RetrieveScores());
        }

        IEnumerator RetrieveScores()
        {
            GetComponent<Leaderboard>().UpdateScore((int)(LevelManager.singleton.TimeElapsed * 1000));
            yield return new WaitForSeconds(1f);
            GetComponent<Leaderboard>().DownloadScores();
            yield return new WaitForSeconds(1f);
            for(int i = 0; i < scores.Count; i++)
            {
                leaderboardEntriesTxt[i].text = (i + 1) + ". " + scores.Keys.ElementAt(i) + ": " + scores.Values.ElementAt(i) / 1000 + "s";
            }
            localScoreTxt.text = localScore.Keys.ElementAt(0).ToString() + " You: " + (localScore.Values.ElementAt(0) / 1000).ToString() + "s";
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

            leaderboard.DOFade(1, 1f).SetEase(Ease.InExpo);
            
            for(int i = 1; i < endPanel.transform.childCount; i++)
            {
                var child = endPanel.transform.GetChild(i);
                child.gameObject.SetActive(true);
                TMP_Text text = child.GetComponent<TMP_Text>();
                child.GetComponent<TMP_Text>().DOColor(new Color(text.color.r, text.color.g, text.color.b, 255), 0.2f).SetEase(Ease.OutExpo);
                child.transform.DOLocalMoveY(child.localPosition.y - 50, 0.2f);
                yield return new WaitForSeconds(0.2f);
            }
        }

        void OnSceneLoad()
        {
            LevelStart();
            GetComponent<AudioManager>().OnSceneLoad();
        }

        private void OnDestroy() 
        {
            singleton = null;    
        }
    }
}