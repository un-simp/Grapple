using UnityEngine;
using UnityEngine.Serialization;

namespace Wildflare.UI.MenuStates {
    public class MenuManager : StateMachine {

        public Transform mainUI;
        public Transform levelUI;
        public Transform optionsUI;
        public Transform creditsUI;

        public float time;
        [HideInInspector]public float opacityTime => time * 0.8f;
        
        void Start() 
        {
            SetState(new MainMenu(this));
        }
        
        public void OnSwitchStateButton(int _desiredState) 
        {
            StartCoroutine(state.SwitchState(_desiredState));
        }
    }
}