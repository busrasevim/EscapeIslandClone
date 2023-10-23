using _Project.Scripts.Level.Signals;
using _Project.Scripts.SaveSystem;
using _Project.Scripts.State_Machine.States;
using _Project.Scripts.UI;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.State_Machine.State_Machines
{
    public class UIStateMachine : StateManager<UIStateMachine.UIState>
    {
        public enum UIState
        {
            Start,
            InGame,
            LevelEnd,
        }

        private StartUI _startUI;
        private InGameUI _inGameUI;
        private EndUI _endUI;
        [Inject] private DataManager _dataManager;
    
        protected override void Init()
        {
            _startUI = Object.FindObjectOfType<StartUI>();
            _inGameUI = Object.FindObjectOfType<InGameUI>();
            _endUI = Object.FindObjectOfType<EndUI>();
        
            _signalBus.Subscribe<OnLevelEndSignal>(OnLevelEnd);
            _signalBus.Subscribe<OnLevelStartSignal>(OnLevelStart);
        
            _endUI.Hide();
        }

        protected override void SetStates()
        {
            var startUI = new StartUIState(UIState.Start, UIState.InGame, _startUI,_dataManager);
            var inGameUI = new InGameUIState(UIState.InGame, UIState.LevelEnd, _inGameUI);
            var endUI = new LevelEndUIState(UIState.LevelEnd, UIState.Start, _endUI);

            States.Add(UIState.Start, startUI);
            States.Add(UIState.InGame, inGameUI);
            States.Add(UIState.LevelEnd, endUI);

            SetStateWithKey(UIState.Start);
        }
    
        private void OnLevelEnd(OnLevelEndSignal args)
        {
            _endUI.SetWin(args.IsWin);
            SetStateWithKey(UIState.LevelEnd);
        }

        private void OnLevelStart(OnLevelStartSignal args)
        {
            SetStateWithKey(UIState.InGame);
        }
    }
}