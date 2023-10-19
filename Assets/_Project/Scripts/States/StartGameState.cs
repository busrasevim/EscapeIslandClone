using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartGameState : BaseState<MainStateMachine.MainState>
{
    public StartGameState(MainStateMachine.MainState key, MainStateMachine.MainState nextState) : base(key)
    {
        _nextStateKey = nextState;
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
