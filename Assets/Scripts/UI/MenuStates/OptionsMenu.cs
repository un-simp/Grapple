using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Wildflare.UI.MenuStates
{
    public class OptionsMenu : State
    {
        public OptionsMenu(MenuManager _menuManager) : base(_menuManager) { }

        public override IEnumerator Start()
        {
            Animate(Vector3.zero, 1);
            yield break;
        }
        
        public override IEnumerator End()
        {
            Animate(new Vector3(2250, 0, 0), 1);
            yield break;
        }
        
        private void Animate(Vector3 _endPos, float _opacity)
        {
            var endPos = _endPos;
            menuManager.optionsUI.DOLocalMove(endPos, menuManager.time).SetEase(Ease.InExpo);
            menuManager.optionsUI.GetComponent<RendererStore>().TweenOpacity(_opacity, menuManager.opacityTime);
        }
        
        public override IEnumerator SwitchState(int _desiredState)
        {
            yield return End();
            menuManager.SetState(new MainMenu(menuManager));
        }
    }
}