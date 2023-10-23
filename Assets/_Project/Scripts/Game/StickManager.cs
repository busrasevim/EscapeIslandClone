using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Data;
using _Project.Scripts.Managers;
using _Project.Scripts.Pools;
using _Project.Scripts.Utils;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Game
{
    public class StickManager : IInitializable
    {
        [Inject] private ObjectPool _objectPool;
        [Inject] private GameSettings _settings;
        [Inject] private GameManager _gameManager;
        private List<StickGroup> _levelSticks;
        private Dictionary<Color, List<StickGroup>> _allSticks;
        private Dictionary<Color, bool> _colorCompletion;

        public void Initialize()
        {
            GenerateAllSticks();
        }

        public void GenerateSticks(int colorCount)
        {
            GenerateLevelSticks(colorCount);
        }

        public void CompleteColor(Color color)
        {
            _colorCompletion[color] = true;
            var isComplete = _colorCompletion.All(kv => kv.Value);
            if (isComplete)
            {
                _gameManager.EndLevel(true);
            }
        }

        public List<StickGroup> GetLevelSticks()
        {
            return _levelSticks;
        }

        public void ResetSticks()
        {
            if (_levelSticks == null) return;

            foreach (var group in _levelSticks)
            {
                group.ResetSticks();
            }
        }
        
        private void GenerateAllSticks()
        {
            var colors = _settings.stickColors;
            var stickCount = _settings.slotStickCount;

            _allSticks = new Dictionary<Color, List<StickGroup>>();
            foreach (var color in colors)
            {
                var sticks = new List<StickGroup>();
                for (int j = 0; j < stickCount; j++)
                {
                    var group = GetStickGroup(color);
                    sticks.Add(group);
                }

                _allSticks.Add(color, sticks);
            }

            foreach (var stickGroup in _allSticks.SelectMany(kvp => kvp.Value))
            {
                stickGroup.DeactivateSticks();
            }
        }
        
        private void GenerateLevelSticks(int colorCount)
        {
            _colorCompletion = new Dictionary<Color, bool>();
            _levelSticks = GetLevelSticks(colorCount);
            _levelSticks.Shuffle();

            foreach (var stickGroup in _levelSticks)
            {
                stickGroup.ActivateSticks();
            }
        }

        private List<StickGroup> GetLevelSticks(int levelColorCount)
        {
            var sticks = new List<StickGroup>();

            var levelColors = _settings.stickColors.GetRandomElements(levelColorCount);
            foreach (var color in levelColors)
            {
                sticks.AddRange(_allSticks[color]);

                _colorCompletion.Add(color, false);
            }

            return sticks;
        }

        private StickGroup GetStickGroup(Color color)
        {
            return new StickGroup(_objectPool, color, _settings);
        }
    }
}