using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

public class LevelManager : IInitializable
{
    private LevelGenerator _generator;
    private GameObject _levelObject;

    private DataManager _dataManager;

    public int CurrentLevelNo { get; private set; }
    public Level CurrentLevel { get; private set; }

    [Inject] private SignalBus _signalBus;
    [Inject] private ObjectPool _objectPool;
    [Inject] private StickManager _stickManager;
    [Inject] private DataHolder _dataHolder;
    

    public void Initialize()
    {
        //level number value, generating etc
        SetInitialLevel();
        _generator = new LevelGenerator(this, _objectPool, _stickManager, _dataHolder.settings);
    }

    [Inject]
    private void SpecialInit(DataManager dataManager)
    {
        _dataManager = dataManager;
    }

    private void SetInitialLevel()
    {
        CurrentLevelNo = _dataManager.GameData.currentLevelNumber;
    }

    public void SetUpLevel()
    {
        _levelObject = new GameObject("Level");

        CurrentLevel = new Level();

        _generator.GenerateLevel();
    }

    public void NextLevel()
    {
        OnLevelCompleted(CurrentLevelNo);
        CurrentLevelNo++;
    }

    private void OnLevelCompleted(int completedLevelIndex)
    {
        _signalBus.TryFire(new OnLevelCompletedSignal(completedLevelIndex));
    }

    public void CleanLevel()
    {
        if (_levelObject)
        {
            Object.Destroy(_levelObject);
        }
    }
}

[Serializable]
public class Level
{
    public Level()
    {
    }
}

public struct OnLevelCompletedSignal
{
    public readonly int LevelIndex;

    public OnLevelCompletedSignal(int levelIndex)
    {
        LevelIndex = levelIndex;
    }
}