﻿using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Barji.UI.MenuStates
{
    public class MainMenu : State
    {
        public MainMenu(MenuManager _menuManager) : base(_menuManager) { }

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

        private void Animate(Vector3 _endPos, float _opacity)
        {
            var endPos = _endPos;
            menuManager.mainUI.DOLocalMove(endPos, menuManager.time).SetEase(Ease.InExpo);
            menuManager.mainUI.GetComponent<RendererStore>().TweenOpacity(_opacity, menuManager.opacityTime);
        }

        public override IEnumerator SwitchState(int _desiredState)
        {
            yield return End();
            if (_desiredState == 0)
            {
                menuManager.SetState(new LevelMenu(menuManager));
            }

            if (_desiredState == 1)
            {
                menuManager.SetState(new OptionsMenu(menuManager));
            }
        }
    }
}