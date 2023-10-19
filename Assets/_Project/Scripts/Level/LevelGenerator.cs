using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelGenerator
{
    public LevelGenerator(LevelManager levelManager)
    {
        
    }
    
    public void GenerateLevel()
    {
        Debug.Log("Level is generated.");
    }
}
