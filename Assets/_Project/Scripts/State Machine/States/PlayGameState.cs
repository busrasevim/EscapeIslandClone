using _Project.Scripts.State_Machine.State_Machines;
using Lean.Touch;
using UnityEngine;

namespace _Project.Scripts.State_Machine.States
{
    public class PlayGameState : BaseState<MainStateMachine.MainState>
    {
        private LeanFingerTap _leanFingerTap;
    
        public override void OnEnter()
        {
            _leanFingerTap.enabled = true;
        }

        public override void OnExit()
        {
            _leanFingerTap.enabled = false;
        }
    
        public PlayGameState(MainStateMachine.MainState key, MainStateMachine.MainState nextStateKey) : base(key)
        {
            NextStateKey = nextStateKey;
            _leanFingerTap = Object.FindObjectOfType<LeanFingerTap>();
            _leanFingerTap.enabled = false;
        }
    }
}
