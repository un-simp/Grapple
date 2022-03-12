using System.Collections;

namespace Barji.UI.MenuStates
{
    public abstract class State
    {
        protected MenuManager menuManager;

        public State(MenuManager _menuManager)
        {
            menuManager = _menuManager;
        }

        public virtual IEnumerator Start()
        {
            yield break;
        }

        public virtual IEnumerator End()
        {
            yield break;
        }

        public virtual IEnumerator SwitchState(int _desiredState)
        {
            yield break;
        }
    }
}