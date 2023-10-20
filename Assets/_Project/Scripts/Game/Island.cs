using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Island : MonoBehaviour
{
    [SerializeField] private Transform leftBorder;
    [SerializeField] private Transform rightBorder;
    [SerializeField] private Transform behindBorder;
    [SerializeField] private Transform frontBorder;

    public GameSettings settings;

    [Serializable]
    public struct SlotGroup
    {
        public List<Vector3> slotPositions;
        public Island currentIsland;

        public SlotGroup(int slotStickCount, int slotNumber, Island island)
        {
            slotPositions = new List<Vector3>();
            currentIsland = island;
            
            var plusXValue = (currentIsland.rightBorder.localPosition.x - currentIsland.leftBorder.localPosition.x) / (slotStickCount - 1);
            var plusZValue= (currentIsland.frontBorder.localPosition.z - currentIsland.behindBorder.localPosition.z) / (slotStickCount - 1);

            var slotZPosition = currentIsland.behindBorder.localPosition.z + slotNumber * plusZValue;
            for (int i = 0; i < slotStickCount; i++)
            {
                var newPositionX = island.leftBorder.localPosition.x + i * plusXValue;
                var newPosition = new Vector3(newPositionX, 0f, slotZPosition);
                slotPositions.Add(newPosition);
            }
        }
    }

    private List<SlotGroup> _slots = new List<SlotGroup>();
    private Stack<SlotGroup> _emptySlots;

    public void Initialize(GameSettings settings)
    {
        _slots = new List<SlotGroup>();
        
        var stickCount = settings.slotStickCount;
        for (int i = 0; i < stickCount; i++)
        {
            var slot = new SlotGroup(stickCount, i, this);
            _slots.Add(slot);
        }
        
        _emptySlots = new Stack<SlotGroup>();

        for (int i = stickCount-1; i >= 0; i--)
        {
            _emptySlots.Push(_slots[i]);
        }
    }

    public bool TryGetEmptySlotGroup(out SlotGroup group)
    {
        if (_emptySlots.Count == 0)
        {
            group = default;
            return false;
        }

        group = _emptySlots.Pop();
        
        return true;
    }

    public void SetSlotEmpty(SlotGroup group)
    {
        _emptySlots.Push(group);
    }
}
