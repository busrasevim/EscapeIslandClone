using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGameState : BaseState<MainStateMachine.MainState>
{
    public FinishGameState(MainStateMachine.MainState key, MainStateMachine.MainState nextStateKey) : base(key)
    {
        _nextStateKey = nextStateKey;
    }

    public override void OnEnter()
    {
        
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnExit()
    {
        
    }

    public override MainStateMachine.MainState GetNextState()
    {
        return _nextStateKey;
    }
}
