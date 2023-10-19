using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGameState : BaseState<MainStateMachine.MainState>
{

    public override void OnUpdate()
    {
        
    }

    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {
        
    }

    public override MainStateMachine.MainState GetNextState()
    {
        return _nextStateKey;
    }

    public PlayGameState(MainStateMachine.MainState key, MainStateMachine.MainState nextStateKey) : base(key)
    {
        _nextStateKey = nextStateKey;
    }
}
