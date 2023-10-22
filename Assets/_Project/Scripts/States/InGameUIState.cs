using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIState : BaseState<UIStateMachine.UIState>
{
    private InGameUI _inGameUI;
    public InGameUIState(UIStateMachine.UIState key, UIStateMachine.UIState nextStateKey,InGameUI inGameUI) : base(key)
    {
        _nextStateKey = nextStateKey;
        _inGameUI = inGameUI;
    }

    public override void OnEnter()
    {
        _inGameUI.Show();
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnExit()
    {
        _inGameUI.Hide();
    }

    public override UIStateMachine.UIState GetNextState()
    {
        return _nextStateKey;
    }
}
