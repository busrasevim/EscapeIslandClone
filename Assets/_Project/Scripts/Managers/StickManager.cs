using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StickManager: IInitializable
{
    [Inject] private ObjectPool _objectPool;
    [Inject] private DataHolder _dataHolder;
    private GameSettings _settings;
    private List<StickGroup> _levelSticks;
    private Dictionary<Color, List<StickGroup>> _allSticks;
    
    public void Initialize()
    {
        _settings = _dataHolder.settings;
        GenerateAllSticks();
    }

    private void GenerateAllSticks()
    {
        var colors = _settings.stickColors;
        var stickCount = _settings.slotStickCount;
        
        _allSticks = new Dictionary<Color, List<StickGroup>>();
        for (int i = 0; i < colors.Length; i++)
        {
            var sticks = new List<StickGroup>();
            for (int j = 0; j < stickCount; j++)
            {
                var group = GetStickGroup(stickCount, colors[i]);
                sticks.Add(group);
            }
            
            _allSticks.Add(colors[i], sticks);
        }

        foreach (KeyValuePair<Color,List<StickGroup>> kvp in _allSticks)
        {
            foreach (var stickGroup in kvp.Value)
            {
                stickGroup.DeactivateSticks();
            }
        }
    }

    public void GenerateSticks(int colorCount)
    {
        GenerateLevelSticks(colorCount);
    }

    public List<StickGroup> GetLevelSticks()
    {
        return _levelSticks;
    }
    
    private void GenerateLevelSticks(int colorCount)
    {
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

      //  Debug.Log(_settings);
       // Debug.Log(_settings.stickColors);
        var levelColors = _dataHolder.settings.stickColors.GetRandomElements(levelColorCount);
        foreach (var color in levelColors)
        {
            foreach (var stickGroup in _allSticks[color])
            {
                sticks.Add(stickGroup);
            }
        }
        
        return sticks;
    }

    private StickGroup GetStickGroup(int stickCount, Color color)
    {
        return new StickGroup(stickCount, _objectPool, color);
    }
    
    public class StickGroup
    {
        private List<Stick> _stickGroup;
        private Island.SlotGroup _currentSlotGroup;
        private Color _stickGroupColor;

        public StickGroup(int stickCount,ObjectPool pool, Color color)
        {
            _stickGroup = new List<Stick>();
            _stickGroupColor = color;
            
            for (int i = 0; i < stickCount; i++)
            {
                var stick = pool.SpawnFromPool(PoolTags.Stick, Vector3.zero, Quaternion.identity).GetComponent<Stick>();
                stick.PrepareStick(_stickGroupColor);
                _stickGroup.Add(stick);
            }
        }

        public void ChangeGroupPosition(Island.SlotGroup group)
        {
            _currentSlotGroup = group;
            for (int i = 0; i < _stickGroup.Count; i++)
            {
                _stickGroup[i].transform.SetParent(_currentSlotGroup.currentIsland.transform);
                _stickGroup[i].GoNewPlace(_currentSlotGroup.slotPositions[i], _currentSlotGroup);
            }
        }
        public void ActivateSticks()
        {
            foreach (var stick in _stickGroup)
            {
                stick.Activate();
            }
        }

        public void DeactivateSticks()
        {
            foreach (var stick in _stickGroup)
            {
                stick.Deactivate();
            }
        }


        public Color GetGroupColor()
        {
            return _stickGroupColor;
        }
    }

}
