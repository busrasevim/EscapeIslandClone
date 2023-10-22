using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class GameManager : IInitializable, IDisposable
{
    [Inject] private ISaveSystem _saveSystem;
    [Inject] private MainStateMachine _mainStateMachine;
    [Inject] private UIStateMachine _uIStateMachine;
    [Inject] private LevelManager _levelManager;
    [Inject] private ObjectPool _objectPool;
    
    [Inject] private SignalBus _signalBus;
    private FXManager _fxManager;

    public void Initialize()
    {
        SetUpLevel();
    }

    public void Dispose()
    {
        //like onDestroy, after all destroy methods
    }

    private void SetUpLevel()
    {
        _mainStateMachine.SetStateWithKey(MainStateMachine.MainState.Start);

        _levelManager.SetUpLevel();

        _uIStateMachine.SetStateWithKey(UIStateMachine.UIState.Start);
    }
    
    private async void RestartLevelAfter(float delay)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delay));
        RestartLevel();
    }
    
    public void RestartLevel()
    {
        //will be filled...

        SetUpLevel();
    }

    #region Game State Events

    public void StartLevel()
    {
        
        _signalBus.TryFire(new OnLevelStartSignal(_levelManager.CurrentLevelNo));
    }

    public void EndLevel(bool isWin)
    {
        _mainStateMachine.SetStateWithKey(MainStateMachine.MainState.Finish);

        if (isWin)
        {
            LevelCompleted();
            _signalBus.TryFire(new OnLevelEndSignal(true));
            return;
        }

        LevelFailed();
        _signalBus.TryFire(new OnLevelEndSignal(false));
    }

    private void LevelCompleted()
    {
        _levelManager.NextLevel();

      //  _fxManager.PlayLevelCompleteFX();

        Debug.Log("Level completed.");
    }

    private void LevelFailed()
    {
        Debug.Log("Level failed.");
    }

    #endregion
}

public struct OnLevelEndSignal
{
    public readonly bool IsWin;

    public OnLevelEndSignal(bool isWin)
    {
        IsWin = isWin;
    }
}

public struct OnLevelStartSignal
{
    public readonly int StartingLevelIndex;

    public OnLevelStartSignal(int startingLevelIndex)
    {
        StartingLevelIndex = startingLevelIndex;
    }
}