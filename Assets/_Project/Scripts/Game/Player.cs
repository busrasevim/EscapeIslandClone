using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{ 
    public float moveSpeed;
    
    private IInputHandler _inputHandler;
    private GameManager _gameManager;

    [Inject]
    private void Construct(IInputHandler inputHandler, GameManager gameManager, LevelManager levelManager)
    {
        _inputHandler = inputHandler;
        _gameManager = gameManager;
    }
    
    private void Update()
    {
        
    }
}

/*public class Player : MonoBehaviour
{

    private async void Spawner()
    {
        while (true)
        {
            new GameObject();
            await UniTask.Delay(TimeSpan.FromSeconds(2));
        }
    }
}*/
