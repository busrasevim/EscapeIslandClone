using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStateMachine : StateManager<UIStateMachine.UIState>
{
    public enum UIState
    {
        Start,
        InGame,
        LevelCompleted,
        LevelFailed,
    }

    protected override void SetStates()
    {
        
    }
}
