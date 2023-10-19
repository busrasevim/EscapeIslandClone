using System.Collections.Generic;
using UnityEngine;
using Zenject;
/*
public class StateManagerBB : IInitializable, ITickable
{
    private GameState _currentGameState;
    public GameState CurrentGameState => _currentGameState;

    private Dictionary<GameStateType, GameState> _gameStates;
    [Inject] private GameManager _gameManager;

    public void Initialize()
    {
        FillTheStates();
        _gameManager.StartAction += NextState;
    }
    
    public void Tick()
    {
        _currentGameState?.OnUpdate();
    }

    public void NextState()
    {
        if (_currentGameState.toGameState != null)
        {
            SetState(_currentGameState.toGameState);
        }
    }

    private void SetState(GameState state)
    {
        if (_currentGameState != null && state == _currentGameState) return;

        _currentGameState?.OnExit();
        _currentGameState = state;
        _currentGameState?.OnEnter();

        Debug.Log("The state " + state.gameStateType + " started.");
    }

    public void SetStateWithType(GameStateType type)
    {
        if (_gameStates.ContainsKey(type))
        {
            SetState(_gameStates[type]);
        }
        else
        {
            Debug.LogWarning("There is no state that named " + type + ".");
        }
    }

    private void FillTheStates()
    {
        var finishGameState = new FinishGameState(GameStateType.Finish, null);
        var playGameState = new PlayGameState(GameStateType.Play, finishGameState);
        var startGameState = new StartGameState(GameStateType.Start, playGameState);

        _gameStates = new Dictionary<GameStateType, GameState>
        {
            { GameStateType.Start, startGameState },
            { GameStateType.Play, playGameState },
            { GameStateType.Finish, finishGameState }
        };

        SetState(startGameState);
    }
}

public enum GameStateType
{
    Start,
    Play,
    Finish,
}*/