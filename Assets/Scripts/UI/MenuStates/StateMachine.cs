﻿using UnityEngine;

namespace Barji.UI.MenuStates
{
    public abstract class StateMachine : MonoBehaviour
    {
        protected State state;

        public void SetState(State _state)
        {
            state = _state;
            StartCoroutine(state.Start());
        }
    }
}