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
            gliderStartPos = glider.localPosition;
            var localEulerAngles = glider.localEulerAngles;
            gliderStartRot = localEulerAngles;
            
            //Move glider to non-resting transforms
            glider.localPosition = gliderStartPos + gliderOffset;
            localEulerAngles = new Vector3(90, localEulerAngles.y, localEulerAngles.z);
            glider.localEulerAngles = localEulerAngles;

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
            glider.DOLocalMove(gliderStartPos, 0.3f);
            glider.DOLocalRotate(gliderStartRot, 0.4f); 
            
            //Bring In graphic
            gliderProgressBar.transform.DOLocalMoveX(-894, 0.2f).SetEase(Ease.OutExpo);
        }
        
        public void StopGliding()
        {
            //Enable glider and move to resting transform
            glider.DOLocalMove(gliderStartPos + gliderOffset, 0.3f);
            glider.DOLocalRotate(new Vector3(90, gliderStartRot.y, glider.localEulerAngles.z), 0.4f);
        }
    }
}