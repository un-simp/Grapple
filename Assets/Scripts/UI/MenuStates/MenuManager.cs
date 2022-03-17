using UnityEngine;

namespace Barji.UI.MenuStates
{
    public class MenuManager : StateMachine
    {
        public Transform mainUI;
        public Transform levelUI;
        public Transform optionsUI;

        public float time;
        [HideInInspector] public float opacityTime => time * 0.8f;

        private void Start()
        {
            SetState(new MainMenu(this));
        }

        public void OnSwitchStateButton(int _desiredState)
        {
            StartCoroutine(state.SwitchState(_desiredState));
        }
    }
}