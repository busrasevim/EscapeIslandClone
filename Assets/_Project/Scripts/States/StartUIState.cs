using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUIState : BaseState<UIStateMachine.UIState>
{
    private StartUI _startUI;
    
    public StartUIState(UIStateMachine.UIState key, UIStateMachine.UIState nextStateKey, StartUI startUI) : base(key)
    {
        _nextStateKey = nextStateKey;
        _startUI = startUI;
    }

    public override void OnEnter()
    {
        _startUI.Show();
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnExit()
    {
        _startUI.Hide();
    }

    public override UIStateMachine.UIState GetNextState()
    {
        return _nextStateKey;
    }
}
