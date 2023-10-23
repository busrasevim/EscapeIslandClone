using System.Collections.Generic;
using _Project.Scripts.Data;
using _Project.Scripts.Game;
using _Project.Scripts.Game.Interfaces;
using _Project.Scripts.Pools;
using _Project.Scripts.Utils;
using UnityEngine;

namespace _Project.Scripts.Level
{
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
        private LineManager _lineManager;
        private IMatchController _matchController;

        private int _currentLevelIslandCount;
        private int _currentLevelColorCount;
        private bool _isBonusLevel;


        public LevelGenerator(LevelManager levelManager, StickManager stickManager, GameSettings settings,
            IMatchController controller, LineManager lineManager, ObjectPool pool)
        {
            _levelManager = levelManager;
            _objectPool = pool;
            _stickManager = stickManager;
            _settings = settings;
            _matchController = controller;
            _lineManager = lineManager;

            _islandPositions = new Vector3[12];
            _islands = new List<Island>();

            SetNormalLevelIslandPositions();
            SetBonusLevelIslandPositions();
            GenerateIslands();
        }

        public void GenerateLevel()
        {
            ResetLevel();

            var state = Random.state;
            Random.InitState(_levelManager.CurrentLevelNo);
            SetLevelIslandAndColorCount(_levelManager.CurrentLevelNo);
            GenerateLevelIslands();
            _stickManager.GenerateSticks(_currentLevelColorCount);

            var sticks = _stickManager.GetLevelSticks();
            var islandIndex = 0;

            _levelIslands.Shuffle();
            var tutorialFactor = 0;
            for (int i = 0; i < sticks.Count; i++)
            {
                SlotGroup group = null;

                if (_levelManager.CurrentLevelNo == 0)
                {
                    tutorialFactor = i % 2;
                }

                if (_levelIslands[islandIndex + tutorialFactor].TryGetEmptySlotGroup(out group))
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

        private void ResetLevel()
        {
            ResetIslands();
            _stickManager.ResetSticks();
            _matchController.DeselectAll();
            _lineManager.ResetLines();
        }

        private void ResetIslands()
        {
            foreach (var island in _islands)
            {
                island.Reset();
            }
        }

        //generating all islands for the game
        private void GenerateIslands()
        {
            for (int i = 0; i < _islandPositions.Length; i++)
            {
                var rotation = Quaternion.Euler(0f, i % 2 == 0 ? 90f : -90f, 0f);
                var island = _objectPool.SpawnFromPool(PoolTags.Island, _islandPositions[i], rotation)
                    .GetComponent<Island>();
                island.Initialize(_settings.slotStickCount, _stickManager, _matchController);
                island.Deactivate();
                _islands.Add(island);
            }

            _matchController.SetIslands(_islands.ToArray());
        }
        
        //generating islands which is used on level
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

            _levelIslands.Shuffle();

            if (_isBonusLevel)
            {
                for (int i = 0; i < _levelIslands.Count; i++)
                {
                    _levelIslands[i].transform.position = _bonusLevelIslandPositions[i];
                    _levelIslands[i].transform.LookAt(Vector3.forward * _settings.zCenterPosition);
                }
            }
        }

        //Count Generation
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

            switch (currentLevelNumber)
            {
                case 0:
                    _currentLevelColorCount = 1;
                    _currentLevelIslandCount = 2;
                    break;
                case 1:
                    _currentLevelColorCount = 2;
                    _currentLevelIslandCount = 3;
                    break;
                case < 4:
                    _currentLevelColorCount = 3;
                    _currentLevelIslandCount = 5;
                    break;
                case < 8:
                    _currentLevelColorCount = 4;
                    _currentLevelIslandCount = 6;
                    break;
                case < 15:
                    _currentLevelColorCount = 5;
                    _currentLevelIslandCount = 8;
                    break;
                case < 22:
                    _currentLevelColorCount = 5;
                    _currentLevelIslandCount = 7;
                    break;
                case < 32:
                    _currentLevelColorCount = 6;
                    _currentLevelIslandCount = 9;
                    break;
                default:
                    _currentLevelColorCount = 7;
                    _currentLevelIslandCount = 9;
                    break;
            }
        }

        //after bonus level
        private void FixIslandPositions()
        {
            for (int i = 0; i < _islands.Count; i++)
            {
                var rotation = Quaternion.Euler(0f, i % 2 == 0 ? 90f : -90f, 0f);
                _islands[i].transform.position = _islandPositions[i];
                _islands[i].transform.rotation = rotation;
            }
        }

        //normal level island positions
        private void SetNormalLevelIslandPositions()
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    var position = new Vector3(j * _settings.oppositeIslandsDistance - _settings.normalIslandOffset, 0f,
                        i * _settings.nearIslandsDistance);
                    _islandPositions[i * 2 + j] = position;
                }
            }
        }
        //Set circle positions
        private void SetBonusLevelIslandPositions()
        {
            var numberOfIslands = _settings.bonusLevelIslandCount;
            var unitAngle = 360f / numberOfIslands;
            var radius = _settings.bonusLevelRadius;
            var zPlus = _settings.zCenterPosition;

            _bonusLevelIslandPositions = new Vector3[numberOfIslands];
            for (int i = 0; i < numberOfIslands; i++)
            {
                var angle = i * unitAngle;
                var radians = Mathf.Deg2Rad * angle;

                var x = radius * Mathf.Cos(radians);
                var z = radius * Mathf.Sin(radians) + zPlus;

                var position = new Vector3(x, 0, z);

                _bonusLevelIslandPositions[i] = position;
            }
        }
    }
}