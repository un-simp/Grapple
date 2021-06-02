using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Wildflare.UI.MenuStates {
    public class LevelMenu : State {
        public override IEnumerator Start() 
        {
            //In Animation
            Animate(Vector3.zero, 1);
            yield break;
        }
        
        public override IEnumerator End()
        {
            //Out Animation
            Animate(new Vector3(-1920, 0, 0), 0);
            yield break;
        }

        void Animate(Vector3 _endPos, float _opacity)
        {
            var endPos = _endPos;
            menuManager.levelUI.DOLocalMove(endPos, menuManager.time).SetEase(Ease.InExpo);
            menuManager.levelUI.GetComponent<RendererStore>().TweenOpacity(_opacity, menuManager.opacityTime);
        }

        public override IEnumerator SwitchState(int _desiredState) {
            yield return End();
            menuManager.SetState(new MainMenu(menuManager));
            yield break;
        }

        public LevelMenu(MenuManager _menuManager) : base(_menuManager) 
        {
            
        }
    }
}