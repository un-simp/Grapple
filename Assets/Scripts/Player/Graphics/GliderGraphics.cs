using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI.ProceduralImage;
using Wildflare.Player.Attachments;

namespace Wildflare.Player.Graphics
{
    public class GliderGraphics : MonoBehaviour
    {
        [SerializeField] private Transform glider;
        [SerializeField] private Vector3 gliderOffset;
        private Vector3 gliderStartPos;
        private Vector3 gliderStartRot;
        private Glider gliderLogic;
        [SerializeField] private ProceduralImage gliderProgressBar;
        private bool progressShowing;
        
        void Start()
        {
            gliderLogic = GetComponent<Glider>();
            //Store The Resting Transforms
            gliderStartPos = glider.transform.localPosition;
            gliderStartRot = glider.transform.localEulerAngles;
            
            //Move glider to non-resting transforms
            glider.localPosition = gliderStartPos + gliderOffset;
            glider.localEulerAngles = new Vector3(90, glider.localEulerAngles.y, glider.localEulerAngles.z);

            gliderProgressBar.transform.localPosition = new Vector2(-1020, 0);
        }

        private void Update()
        {
            gliderProgressBar.fillAmount = gliderLogic.currentGlideTime / gliderLogic.glideDuration;

            if (gliderLogic.currentGlideTime == gliderLogic.glideDuration && progressShowing)
            {
                progressShowing = false;
                gliderProgressBar.transform.DOLocalMoveX(-1020, 0.2f).SetEase(Ease.InExpo);
            }
            else if (gliderLogic.currentGlideTime == 0 && !progressShowing)
            {
                progressShowing = true;
                gliderProgressBar.transform.DOLocalMoveX(-894, 0.2f).SetEase(Ease.OutExpo);
            }
        }

        public void StartGliding()
        {
            //Enable glider and move to resting transform
            //glider.gameObject.SetActive(true);
            glider.DOLocalMove(gliderStartPos, 0.3f);
            glider.DOLocalRotate(gliderStartRot, 0.4f); 
        }
        
        public void StopGliding()
        {
            //Enable glider and move to resting transform
            glider.DOLocalMove(gliderStartPos + gliderOffset, 0.3f);
            glider.DOLocalRotate(new Vector3(90, gliderStartRot.y, glider.localEulerAngles.z), 0.4f);
            //StartCoroutine(GliderStopDelay());
        }

        IEnumerator GliderStopDelay()
        {
            //Wait for animation to finish and disable gameobject
            yield return new WaitForSeconds(0.4f);
            glider.gameObject.SetActive(false);
        }
    }
}