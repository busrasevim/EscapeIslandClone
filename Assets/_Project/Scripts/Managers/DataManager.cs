using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DataManager : IInitializable, IDisposable
{
    private GameData _gameData;
    public GameData GameData => _gameData;
    private const string DATA_KEY = "GAME_DATA_KEY";
    private ISaveSystem _saveSystem;

    [Inject] private SignalBus _signalBus;
    
    public DataManager(ISaveSystem saveSystem)
    {
        _saveSystem = saveSystem;
    }

    public void Initialize()
    {
        LoadData();
        _signalBus.Subscribe<OnLevelCompletedSignal>(OnLevelCompleted);
    }

    public void Dispose()
    {
        SaveData();
        _signalBus.TryUnsubscribe<OnLevelCompletedSignal>(OnLevelCompleted);
    }


    public void SaveData()
    {
        _saveSystem.Save(DATA_KEY, _gameData);
    }

    private void LoadData()
    {
        if (!_saveSystem.HasKey(DATA_KEY))
        {
            _gameData = new GameData();
            SaveData();
        }
        else if (_saveSystem.TryGet(DATA_KEY, out _gameData))
        {
            Debug.Log("Data is loaded successfully.");
        }
        else
        {
            Debug.LogError("Data cannot be loaded.");
            Application.Quit();
        }
    }
    
    
    private void OnLevelCompleted(OnLevelCompletedSignal args)
    {
        _gameData.currentLevelNumber = args.LevelIndex+1;
        SaveData();
    }
}