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
    private StickManager _stickManager;
    
    private float _islandStartYPosition;
    private Tween _selectedTween;
    private List<SlotGroup> _slotGroups;
    private Stack<SlotGroup> _emptySlotGroups;
    private Stack<SlotGroup> _filledSlotGroups;

    public void Initialize(GameSettings settings, MatchController controller, StickManager stickManager)
    {
        _slotGroups = new List<SlotGroup>();

        var stickCount = settings.slotStickCount;
        for (int i = 0; i < stickCount; i++)
        {
            var slot = new SlotGroup(stickCount, i, this);
            _slotGroups.Add(slot);
        }

        _emptySlotGroups = new Stack<SlotGroup>();

        for (int i = stickCount - 1; i >= 0; i--)
        {
            _emptySlotGroups.Push(_slotGroups[i]);
        }

        _filledSlotGroups = new Stack<SlotGroup>();

        _matchController = controller;
        _islandStartYPosition = transform.position.y;
        _stickManager = stickManager;
    }

    public bool TryGetEmptySlotGroup(out SlotGroup group)
    {
        if (_emptySlotGroups.Count == 0)
        {
            group = default;
            return false;
        }

        group = _emptySlotGroups.Pop();
        _filledSlotGroups.Push(group);

        return true;
    }

    public void SetSlotEmpty(SlotGroup group)
    {
        _emptySlotGroups.Push(group);
        _filledSlotGroups.Pop();
    }

    public List<StickManager.StickGroup> GetAvailableGroups(int emptySlotCountOfOtherIsland)
    {
        var color = GetFirstColor();
        var groupList = new List<StickManager.StickGroup>();
        for (int i = 0; i < emptySlotCountOfOtherIsland; i++)
        {
            if (_filledSlotGroups.Count == 0) break;
            var slot = _filledSlotGroups.Peek();
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

    public void GroupTransition(List<StickManager.StickGroup> groups, Line line, Action done)
    {
        var controlNumber = 0;
        for (int i = 0; i < groups.Count; i++)
        {
            var slot = _emptySlotGroups.Pop();
            _filledSlotGroups.Push(slot);
            groups[i].ChangeGroupPosition(slot, line, i, () =>
            {
                controlNumber++;
                if (controlNumber == groups.Count)
                    done.Invoke();
            });
        }

        if (IsIslandComplete())
        {
            leanSelectable.enabled = false;
            _stickManager.CompleteColor(GetFirstColor());
        }
    }

    public int GetEmptySlotCount()
    {
        return _emptySlotGroups.Count;
    }

    private bool IsThereEmptySlot()
    {
        return _emptySlotGroups.Count > 0;
    }

    public bool IsIslandEmpty()
    {
        return _filledSlotGroups.Count == 0;
    }

    public bool IsIslandOkay(Island selectedIsland)
    {
        if (IsIslandEmpty())
            return true;

        return selectedIsland.GetFirstColor() == GetFirstColor() && IsThereEmptySlot();
    }

    private Color GetFirstColor()
    {
        return _filledSlotGroups.Peek().slotColor;
    }

    public void Activate()
    {
        leanSelectable.enabled = true;
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

    public void Reset()
    {
        foreach (var slot in _filledSlotGroups)
        {
            _emptySlotGroups.Push(slot);
        }

        _filledSlotGroups.Clear();

        Deactivate();
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

    private bool IsIslandComplete()
    {
        if (_emptySlotGroups.Count > 0) return false;

        var color = GetFirstColor();
        foreach (var slot in _slotGroups)
        {
            if (slot.slotColor != color)
            {
                return false;
            }
        }

        return true;
    }
    
    
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

            var plusXValue = (currentIsland.rightBorder.localPosition.x - currentIsland.leftBorder.localPosition.x) /
                             (slotStickCount - 1);
            var plusZValue = (currentIsland.frontBorder.localPosition.z - currentIsland.behindBorder.localPosition.z) /
                             (slotStickCount - 1);

            var slotZPosition = currentIsland.behindBorder.localPosition.z + slotNumber * plusZValue;
            for (int i = 0; i < slotStickCount; i++)
            {
                var newPositionX = island.leftBorder.localPosition.x + i * plusXValue;
                var newPosition = new Vector3(newPositionX, 0f, slotZPosition);
                slotPositions.Add(newPosition);
            }
        }
    }
}