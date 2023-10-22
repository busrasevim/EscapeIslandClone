using System.Collections;
using System.Collections.Generic;
using Lean.Common;
using Lean.Touch;
using UnityEngine;
using Zenject;

public class PlayGameState : BaseState<MainStateMachine.MainState>
{
    private LeanFingerTap _leanFingerTap;
    
    public override void OnUpdate()
    {
        
    }

    public override void OnEnter()
    {
        _leanFingerTap.enabled = true;
    }

    public override void OnExit()
    {
        _leanFingerTap.enabled = false;
    }

    public override MainStateMachine.MainState GetNextState()
    {
        return _nextStateKey;
    }

    public PlayGameState(MainStateMachine.MainState key, MainStateMachine.MainState nextStateKey) : base(key)
    {
        _nextStateKey = nextStateKey;
        _leanFingerTap = Object.FindObjectOfType<LeanFingerTap>();
        _leanFingerTap.enabled = false;
    }
}
