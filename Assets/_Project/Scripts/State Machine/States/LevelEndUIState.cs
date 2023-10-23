using _Project.Scripts.State_Machine.State_Machines;
using _Project.Scripts.UI;

namespace _Project.Scripts.State_Machine.States
{
    public class LevelEndUIState : BaseState<UIStateMachine.UIState>
    {
        private EndUI _endUI;
        public LevelEndUIState(UIStateMachine.UIState key, UIStateMachine.UIState nextStateKey, EndUI endUI) : base(key)
        {
            NextStateKey = nextStateKey;
            _endUI = endUI;
        }

        public override void OnEnter()
        {
            _endUI.Show();
        }

        public override void OnExit()
        {
            _endUI.Hide();
        }
    }
}
