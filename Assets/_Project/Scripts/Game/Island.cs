using System;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Common;
using Lean.Touch;
using UnityEngine;

public class Island : LeanSelectableBehaviour
{
    [SerializeField] private Transform leftBorder;
    [SerializeField] private Transform rightBorder;
    [SerializeField] private Transform behindBorder;
    [SerializeField] private Transform frontBorder;
    [SerializeField] private LeanSelectableByFinger leanSelectable;
    private MatchController _matchController;
    private float _islandStartYPosition;
    private Tween _selectedTween;

    [Serializable]
    public class SlotGroup
    {
        public List<Vector3> slotPositions;
        public Island currentIsland;
        public Color slotColor;
        public StickManager.StickGroup currentGroup;

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

    private List<SlotGroup> _slots;
    private Stack<SlotGroup> _emptySlots;
    private Stack<SlotGroup> _filledSlots;

    public void Initialize(GameSettings settings, MatchController controller)
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

        _filledSlots = new Stack<SlotGroup>();

        _matchController = controller;
        _islandStartYPosition = transform.position.y;
    }
    
    public bool TryGetEmptySlotGroup(out SlotGroup group)
    {
        if (_emptySlots.Count == 0)
        {
            group = default;
            return false;
        }

        group = _emptySlots.Pop();
        _filledSlots.Push(group);
        
        return true;
    }

    public void SetSlotEmpty(SlotGroup group)
    {
        _emptySlots.Push(group);
        _filledSlots.Pop();
    }

    public List<StickManager.StickGroup> GetAvailableGroups(int emptySlotCountOfOtherIsland)
    {
        var color = GetFirstColor();
        var groupList = new List<StickManager.StickGroup>();
        for (int i = 0; i < emptySlotCountOfOtherIsland; i++)
        {
            if(_filledSlots.Count==0) break;
            var slot = _filledSlots.Peek();
            if (slot.slotColor == color)
            {
                groupList.Add(slot.currentGroup);
                SetSlotEmpty(slot);
            }
            else
            {
                break;
            }
        }

        return groupList;
    }
    
    public void GroupTransition(List<StickManager.StickGroup> groups, Line line)
    {
        foreach (var group in groups)
        {
            var slot = _emptySlots.Pop();
            _filledSlots.Push(slot);
            group.ChangeGroupPosition(slot, line);
        }
    }

    public int GetEmptySlotCount()
    {
        return _emptySlots.Count;
    }
    
    private bool IsThereEmptySlot()
    {
        return _emptySlots.Count > 0;
    }

    public bool IsIslandEmpty()
    {
        return _filledSlots.Count == 0;
    }

    public bool IsIslandOkay(Island selectedIsland)
    {
        if (IsIslandEmpty())
            return true;

        return selectedIsland.GetFirstColor() == GetFirstColor() && IsThereEmptySlot();
    }

    private Color GetFirstColor()
    {
        return _filledSlots.Peek().slotColor;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Deselect()
    {
        leanSelectable.Deselect();
    }

    protected override void OnSelected(LeanSelect select)
    {
        _selectedTween?.Kill();
        _selectedTween = transform.DOMoveY(_islandStartYPosition + 0.2f, 0.1f);
        _matchController.SelectIsland(this);
    }

    protected override void OnDeselected(LeanSelect select)
    {
        _selectedTween?.Kill();
        _selectedTween = transform.DOMoveY(_islandStartYPosition, 0.1f);
    }
}
