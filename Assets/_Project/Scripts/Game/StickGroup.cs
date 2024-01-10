using System.Collections.Generic;
using _Project.Scripts.Data;
using _Project.Scripts.Pools;
using UnityEngine;

namespace _Project.Scripts.Game
{
    public class StickGroup
    {
        private readonly List<Stick> _stickGroup;
        private SlotGroup _currentSlotGroup;
        private readonly Color _stickGroupColor;

        public StickGroup(ObjectPool pool, Material material, GameSettings settings)
        {
            _stickGroup = new List<Stick>();
            _stickGroupColor = material.color;
            var stickCount = settings.slotStickCount;

            for (int i = 0; i < stickCount; i++)
            {
                var stick = pool.SpawnFromPool(PoolTags.Stick, Vector3.zero, Quaternion.identity).GetComponent<Stick>();
                stick.PrepareStick(material, settings.stickMovementSettings);
                _stickGroup.Add(stick);
            }
        }
        
        public Color GetGroupColor()
        {
            return _stickGroupColor;
        }
        
        public void SetGroupPosition(SlotGroup group)
        {
            _currentSlotGroup = group;
            for (int i = 0; i < _stickGroup.Count; i++)
            {
                _stickGroup[i].transform.SetParent(_currentSlotGroup.CurrentIsland.transform);
                _stickGroup[i].transform.localPosition = _currentSlotGroup.SlotPositions[i];
                _stickGroup[i].transform.localRotation = Quaternion.identity;
            }

            group.SetCurrentStickGroup(this);
        }

        public void ChangeGroupPosition(SlotGroup group, Line line, int groupIndex, int allOnRoadStickCount)
        {
            _currentSlotGroup = group;
            for (int i = 0; i < _stickGroup.Count; i++)
            {
                var stickIndex = i + groupIndex * _stickGroup.Count;
                _stickGroup[i].GoNewPlace(line, _currentSlotGroup.SlotPositions[i], _currentSlotGroup,
                    stickIndex, allOnRoadStickCount);
            }

            group.SetCurrentStickGroup(this);
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

        public void ResetSticks()
        {
            foreach (var stick in _stickGroup)
            {
                stick.Reset();
            }
        }

    }
}
