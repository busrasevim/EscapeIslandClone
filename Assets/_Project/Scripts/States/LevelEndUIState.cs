using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndUIState : BaseState<UIStateMachine.UIState>
{
    private EndUI _endUI;
    public LevelEndUIState(UIStateMachine.UIState key, UIStateMachine.UIState nextStateKey, EndUI endUI) : base(key)
    {
        _nextStateKey = nextStateKey;
        _endUI = endUI;
    }

    public override void OnEnter()
    {
        _endUI.Show();
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnExit()
    {
        _endUI.Hide();
    }

    public override UIStateMachine.UIState GetNextState()
    {
        return _nextStateKey;
    }
}
