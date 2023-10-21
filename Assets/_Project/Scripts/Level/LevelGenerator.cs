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
    private Vector3[] _bonusLevelIslandPositions;
    private StickManager _stickManager;
    private List<Island> _islands;
    private List<Island> _levelIslands;
    private GameSettings _settings;
    private MatchController _matchController;

    private int _currentLevelIslandCount;
    private int _currentLevelColorCount;
    private bool _isBonusLevel;

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
        SetBonusLevelIslandPositions();
    }

    public void GenerateLevel()
    {
        var state = Random.state;
        Random.InitState(_levelManager.CurrentLevelNo);
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
                sticks[i].SetGroupPosition(group);
            }
            else
            {
                islandIndex++;
                if (_levelIslands[islandIndex].TryGetEmptySlotGroup(out group))
                {
                    sticks[i].SetGroupPosition(group);
                }
            }
        }

        Random.state = state;
        Debug.Log("Level is generated.");
    }

    private void GenerateIslands()
    {
        for (int i = 0; i < _islandPositions.Length; i++)
        {
            var rotation = Quaternion.Euler(0f, i % 2 == 0 ? 90f : -90f, 0f);
            var island = _objectPool.SpawnFromPool(PoolTags.Island, _islandPositions[i], rotation)
                .GetComponent<Island>();

            island.Initialize(_settings, _matchController, _stickManager);
            island.Deactivate();
            _islands.Add(island);
        }
        
        _matchController.SetIslands(_islands.ToArray());
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

        if (_isBonusLevel)
        {
            for (int i = 0; i < _levelIslands.Count; i++)
            {
                _levelIslands[i].transform.position = _bonusLevelIslandPositions[i];
                _levelIslands[i].transform.LookAt(Vector3.zero);
            }
        }
    }

    private void SetLevelIslandAndColorCount(int currentLevelNumber)
    {
        if (currentLevelNumber > 3 && currentLevelNumber % 3 == 0)
        {
            _isBonusLevel = true;
            _currentLevelColorCount = 7;
            _currentLevelIslandCount = 9;
            return;
        }

        if (_isBonusLevel)
        {
            FixIslandPositions();
        }
        
        _isBonusLevel = false;
        
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

    private void FixIslandPositions()
    {
        for (int i = 0; i < _islands.Count; i++)
        {
            _islands[i].transform.position = _islandPositions[i];
        }
    }

    private void SetBonusLevelIslandPositions()
    {
        var numberOfIslands = _settings.bonusLevelIslandCount;
        var unitAngle = 360f / numberOfIslands;
        var radius = 2f;
        _bonusLevelIslandPositions = new Vector3[numberOfIslands];
        for (int i = 0; i < numberOfIslands; i++)
        {
            var angle = i * unitAngle;
            var radians = Mathf.Deg2Rad * angle;

            var x = radius * Mathf.Cos(radians);
            var z = radius * Mathf.Sin(radians);

            var position = new Vector3(x, 0, z);

            _bonusLevelIslandPositions[i] = position;
        }
    }
}