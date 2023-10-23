using _Project.Scripts.State_Machine.State_Machines;
using _Project.Scripts.UI;

namespace _Project.Scripts.State_Machine.States
{
    public class InGameUIState : BaseState<UIStateMachine.UIState>
    {
        private InGameUI _inGameUI;
        public InGameUIState(UIStateMachine.UIState key, UIStateMachine.UIState nextStateKey,InGameUI inGameUI) : base(key)
        {
            NextStateKey = nextStateKey;
            _inGameUI = inGameUI;
        }

        public override void OnEnter()
        {
            _inGameUI.Show();
        }
    
        public override void OnExit()
        {
            _inGameUI.Hide();
        }
    }
}
