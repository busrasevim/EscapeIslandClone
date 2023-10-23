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
            var materials = _settings.stickMaterials;
            var stickCount = _settings.slotStickCount;

            _allSticks = new Dictionary<Color, List<StickGroup>>();
            foreach (var material in materials)
            {
                var sticks = new List<StickGroup>();
                for (int j = 0; j < stickCount; j++)
                {
                    var group = GetStickGroup(material);
                    sticks.Add(group);
                }

                _allSticks.Add(material.color, sticks);
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

            var materials = _settings.stickMaterials.GetRandomElements(levelColorCount);
            var levelColors = new Color[materials.Length];
            for (int i = 0; i < materials.Length; i++)
            {
                levelColors[i] = materials[i].color;
            }
            foreach (var color in levelColors)
            {
                sticks.AddRange(_allSticks[color]);

                _colorCompletion.Add(color, false);
            }

            return sticks;
        }

        private StickGroup GetStickGroup(Material material)
        {
            return new StickGroup(_objectPool, material, _settings);
        }
    }
}