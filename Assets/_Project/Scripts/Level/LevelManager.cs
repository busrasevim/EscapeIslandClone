using System;
using _Project.Scripts.Data;
using _Project.Scripts.Game;
using _Project.Scripts.Game.Constants;
using _Project.Scripts.Game.Interfaces;
using _Project.Scripts.Level.Signals;
using _Project.Scripts.Pools;
using _Project.Scripts.SaveSystem;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace _Project.Scripts.Level
{
    public class LevelManager : IInitializable
    {
        public int CurrentLevelNo { get; private set; }
        
        private LevelGenerator _generator;
        private DataManager _dataManager;
        private StickManager _stickManager;
        private IMatchController _matchController;
        private LineManager _lineManager;
        private ObjectPool _objectPool;
        private GameSettings _settings;
        private SignalBus _signalBus;
        
        public void Initialize()
        {
            //level number value, generating etc
            SetInitialLevel();
           
        }

        [Inject]
        private void SpecialInit(DataManager dataManager, StickManager stickManager,IMatchController controller,
            LineManager lineManager, ObjectPool pool, GameSettings settings, SignalBus signal)
        {
            _dataManager = dataManager;
            _stickManager = stickManager;
            _matchController = controller;
            _lineManager = lineManager;
            _objectPool = pool;
            _settings = settings;
            _signalBus = signal;
            
            _generator = new LevelGenerator(this, _stickManager, _settings,
                _matchController, _lineManager, _objectPool);
        }
        
        public void SetUpLevel()
        {
            _generator.GenerateLevel();
        }

        public void NextLevel()
        {
            OnLevelCompleted(CurrentLevelNo);
            CurrentLevelNo++;
        }
        
        private void SetInitialLevel()
        {
            CurrentLevelNo = _dataManager.GameData.currentLevelNumber;
        }
        
        private void OnLevelCompleted(int completedLevelIndex)
        {
            _signalBus.TryFire(new OnLevelCompletedSignal(completedLevelIndex));
        }
    }
}