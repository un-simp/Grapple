using System.Collections;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

namespace Wildflare.UI.MenuStates {
    public class MainMenu : State {

        public MainMenu(MenuManager _menuManager) : base(_menuManager) 
        {
            
        }
        
        public override IEnumerator Start() 
        {
            //In Animation
            Animate(Vector3.zero, 1);
            yield break;
        }
        
        public override IEnumerator End() 
        {
            //Out Animation
            Animate(new Vector3(0, -1080, 0), 0);
            yield break;
        }
        
        void Animate(Vector3 _endPos, float _opacity)
        {
            var endPos = _endPos;
            menuManager.mainUI.DOLocalMove(endPos, menuManager.time).SetEase(Ease.InExpo);
            menuManager.mainUI.GetComponent<RendererStore>().TweenOpacity(_opacity, menuManager.opacityTime);
        }

        public override IEnumerator SwitchState(int _desiredState) {
            if (_desiredState == 0) {
                yield return End();
                menuManager.SetState(new LevelMenu(menuManager));
            }
            if (_desiredState == 1) 
            {
                //optionsManager.SetState(new LevelMenu(menuManager));
            }
            yield break;
        }
    }
}