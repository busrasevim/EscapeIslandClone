using System;
using _Project.Scripts.Level;
using _Project.Scripts.Level.Signals;
using _Project.Scripts.Pools;
using _Project.Scripts.State_Machine.State_Machines;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Managers
{
    public class GameManager : IInitializable
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

        private void SetUpLevel()
        {
            _mainStateMachine.SetStateWithKey(MainStateMachine.MainState.Start);
            _uIStateMachine.SetStateWithKey(UIStateMachine.UIState.Start);

            _levelManager.SetUpLevel();
        }
    
        private async void RestartLevelAfter(float delay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            RestartLevel();
        }
    
        public void RestartLevel()
        {
            SetUpLevel();
        }

        #region Game State Events

        public void StartLevel()
        {
            _signalBus.TryFire(new OnLevelStartSignal(_levelManager.CurrentLevelNo));
        }

        public void EndLevel(bool isWin)
        {
            if(isWin)
                LevelCompleted();
            else
                LevelFailed();
       
            _signalBus.TryFire(new OnLevelEndSignal(isWin));
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
}