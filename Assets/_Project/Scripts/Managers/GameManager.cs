using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : IInitializable, IDisposable
{
    public event Action StartAction;
    public event Action<bool> EndAction;
    
        [Inject] private ISaveSystem _saveSystem;
        [Inject] private MainStateMachine _mainStateMachine;
        [Inject] private UIStateMachine _uIStateMachine;
        [Inject] private LevelManager _levelManager;
        [Inject] private ObjectPool _objectPool;
        private FXManager _fxManager;
        
        public void Initialize()
        {
            //like normal initialize method, before all awakes
            _objectPool.PreparePools();
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

         //   _uIStateMachine.SetStateWithKey(UIStateMachine.UIState.Start);
        }

        public void RestartLevel()
        {
            //will be filled...
            
            SetUpLevel();
        }

        #region Game State Events

        public void StartLevel()
        {
            StartAction?.Invoke();
        }

        public void EndLevel(bool isWin)
        {
            _mainStateMachine.SetStateWithKey(MainStateMachine.MainState.Finish);

            if (isWin)
            {
                LevelCompleted();
                EndAction?.Invoke(true);
                return;
            }

            LevelFailed();
            EndAction?.Invoke(false);
        }
        
        private void LevelCompleted()
        {
            _levelManager.NextLevel();

            _fxManager.PlayLevelCompleteFX();

        //    _uIStateMachine.SetStateWithKey(UIStateMachine.UIState.LevelCompleted);
        }

        private void LevelFailed()
        {
           // _uIStateMachine.SetStateWithKey(UIStateMachine.UIState.LevelFailed);
        }

        #endregion

        public void TestMethod()
        {
            Debug.Log("test is successfull");
        }
    }

