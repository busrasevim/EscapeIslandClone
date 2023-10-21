using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class LevelGenerator
{
    private LevelManager _levelManager;
    private ObjectPool _objectPool;
    private Vector3[] _islandPositions;
    private StickManager _stickManager;
    private List<Island> _islands;
    private List<Island> _levelIslands;
    private GameSettings _settings;
    private MatchController _matchController;

    private int _currentLevelIslandCount;
    private int _currentLevelColorCount;

    public LevelGenerator(LevelManager levelManager, ObjectPool pool, StickManager stickManager, GameSettings settings,
        MatchController controller)
    {
        _levelManager = levelManager;
        _objectPool = pool;
        _stickManager = stickManager;
        _settings = settings;
        _matchController = controller;

        _islandPositions = new Vector3[12];

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                var position = new Vector3(j * 4, 0f, i * 1.2f);
                _islandPositions[i * 2 + j] = position;
            }
        }

        _islands = new List<Island>();
        GenerateIslands();
    }

    public void GenerateLevel()
    {
        SetLevelIslandAndColorCount(_levelManager.CurrentLevelNo);
        GenerateLevelIslands();
        _stickManager.GenerateSticks(_currentLevelColorCount);

        var sticks = _stickManager.GetLevelSticks();
        var islandIndex = 0;
        for (int i = 0; i < sticks.Count; i++)
        {
            Island.SlotGroup group = null;

            if (_levelIslands[islandIndex].TryGetEmptySlotGroup(out group))
            {
                sticks[i].ChangeGroupPosition(group);
            }
            else
            {
                islandIndex++;
                if (_levelIslands[islandIndex].TryGetEmptySlotGroup(out group))
                {
                    sticks[i].ChangeGroupPosition(group);
                }
            }
        }

        Debug.Log("Level is generated.");
    }

    private void GenerateIslands()
    {
        for (int i = 0; i < _islandPositions.Length; i++)
        {
            var rotation = Quaternion.Euler(0f, i % 2 == 0 ? 90f : -90f, 0f);
            var island = _objectPool.SpawnFromPool(PoolTags.Island, _islandPositions[i], rotation)
                .GetComponent<Island>();

            island.Initialize(_settings, _matchController);
            island.Deactivate();
            _islands.Add(island);
        }
    }

    private void GenerateLevelIslands()
    {
        var allIslands = new List<Island>();
        allIslands.AddRange(_islands);
        _levelIslands = new List<Island>();
        
        for (int i = 0; i < _currentLevelIslandCount; i++)
        {
            var count = allIslands.Count;
            var index = Mathf.RoundToInt(count / 2);
            allIslands[index].Activate();
            _levelIslands.Add(allIslands[index]);
            allIslands.RemoveAt(index);
        }
    }

    private void SetLevelIslandAndColorCount(int currentLevelNumber)
    {
        if (currentLevelNumber < 3)
        {
            _currentLevelColorCount = 2;
            _currentLevelIslandCount = 3;
        }
        else if (currentLevelNumber < 6)
        {
            _currentLevelColorCount = 3;
            _currentLevelIslandCount = 5;
        }
        else if (currentLevelNumber < 10)
        {
            _currentLevelColorCount = 4;
            _currentLevelIslandCount = 6;
        }
        else if (currentLevelNumber < 17)
        {
            _currentLevelColorCount = 5;
            _currentLevelIslandCount = 8;
        }
        else if (currentLevelNumber < 22)
        {
            _currentLevelColorCount = 5;
            _currentLevelIslandCount = 7;
        }
        else if (currentLevelNumber < 32)
        {
            _currentLevelColorCount = 6;
            _currentLevelIslandCount = 9;
        }
        else
        {
            _currentLevelColorCount = 7;
            _currentLevelIslandCount = 9;
        }
    }
}