using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUIState : BaseState<UIStateMachine.UIState>
{
    private StartUI _startUI;
    private DataManager _dataManager;

    public StartUIState(UIStateMachine.UIState key, UIStateMachine.UIState nextStateKey, StartUI startUI,
        DataManager dataManager) : base(key)
    {
        _nextStateKey = nextStateKey;
        _startUI = startUI;
        _dataManager = dataManager;
    }

    public override void OnEnter()
    {
        _startUI.Show();
        _startUI.SetLevelText(_dataManager.GameData.currentLevelNumber + 1);
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