using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TestScript : MonoBehaviour
{
    [Inject]
    private void Construct(ISaveSystem saveSystem,
        IInputHandler inputHandler,
        DataManager dataManager,
        MainStateMachine mainStateMachine,
        GameManager gameManager)
    {
        Debug.Log(saveSystem);
        Debug.Log(inputHandler);
        Debug.Log(dataManager);
        Debug.Log(dataManager.GameData.currentLevelNumber);
        Debug.Log(mainStateMachine);
        Debug.Log(gameManager);
    }
}
