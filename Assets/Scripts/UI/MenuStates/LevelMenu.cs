using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Barji.UI.MenuStates
{
    public class LevelMenu : State
    {
        public LevelMenu(MenuManager _menuManager) : base(_menuManager) { }

        public override IEnumerator Start()
        {
            //In Animation
            Animate(Vector3.zero, 1);
            yield break;
        }

        public override IEnumerator End()
        {
            //Out Animation
            Animate(new Vector3(-2250, 0, 0), 0);
            yield break;
        }

        private void Animate(Vector3 _endPos, float _opacity)
        {
            var endPos = _endPos;
            menuManager.levelUI.DOLocalMove(endPos, menuManager.time).SetEase(Ease.InExpo);
            menuManager.levelUI.GetComponent<RendererStore>().TweenOpacity(_opacity, menuManager.opacityTime);
        }

        public override IEnumerator SwitchState(int _desiredState)
        {
            yield return End();
            menuManager.SetState(new MainMenu(menuManager));
        }
    }
}