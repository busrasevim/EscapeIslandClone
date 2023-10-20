using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelGenerator
{
    private LevelManager _levelManager;
    private ObjectPool _objectPool;
    private Vector3[] _islandPositions;
    private StickManager _stickManager;
    private List<Island> _islands;
    private GameSettings _settings;

    public LevelGenerator(LevelManager levelManager, ObjectPool pool, StickManager stickManager, GameSettings settings)
    {
        _levelManager = levelManager;
        _objectPool = pool;
        _stickManager = stickManager;
        _settings = settings;

        _islandPositions = new Vector3[10];

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                var position = new Vector3(i * 4, 0f, j * 2);
                _islandPositions[i * 5 + j] = position;
            }
        }

        _islands = new List<Island>();
    }

    public void GenerateLevel()
    {
        GenerateIslands();

        for (int i = 0; i < 20; i++)
        {
            var slotGroup = new Island.SlotGroup();
            var island = _islands.GetRandomElement();
            if (island.TryGetEmptySlotGroup(out slotGroup))
            {
                _stickManager.SpawnNewStickGroup(island.transform, slotGroup, _settings.stickColors.GetRandomElement());
            }
        }
        
        Debug.Log("Level is generated.");
    }

    private void GenerateIslands()
    {
        for (int i = 0; i < _islandPositions.Length; i++)
        {
            var rotation = Quaternion.Euler(0f, i >= _islandPositions.Length / 2 ? -90f : 90f, 0f);
            var island = _objectPool.SpawnFromPool(PoolTags.Island, _islandPositions[i], rotation)
                .GetComponent<Island>();

            island.Initialize(_settings);
            _islands.Add(island);
        }
    }
}