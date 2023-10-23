using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Game.Interfaces;
using _Project.Scripts.Managers;
using DG.Tweening;
using Lean.Common;
using Lean.Touch;
using UnityEngine;

namespace _Project.Scripts.Game
{
    public class Island : LeanSelectableBehaviour
    {
        [SerializeField] internal Transform leftBorder;
        [SerializeField] internal Transform rightBorder;
        [SerializeField] internal Transform behindBorder;
        [SerializeField] internal Transform frontBorder;
        [SerializeField] private LeanSelectableByFinger leanSelectable;
        
        private float _islandStartYPosition;
        private Tween _selectedTween;
        private List<SlotGroup> _slotGroups;
        private Stack<SlotGroup> _emptySlotGroups;
        private Stack<SlotGroup> _filledSlotGroups;
        private int _slotStickCount;
        private List<Stick> _onRoadSticks;
        
        private StickManager _stickManager;
        private IMatchController _matchController;

        public void Initialize(int slotStickCount, StickManager stickManager, IMatchController matchController)
        {
            _slotGroups = new List<SlotGroup>();
            _emptySlotGroups = new Stack<SlotGroup>();
            _filledSlotGroups = new Stack<SlotGroup>();
            _onRoadSticks = new List<Stick>();

            _slotStickCount = slotStickCount;

            for (int i = 0; i < _slotStickCount; i++)
            {
                var slot = new SlotGroup(_slotStickCount, i, this);
                _slotGroups.Add(slot);
            }
            
            for (int i = _slotStickCount - 1; i >= 0; i--)
            {
                _emptySlotGroups.Push(_slotGroups[i]);
            }
            
            _islandStartYPosition = transform.position.y;
            _stickManager = stickManager;
            _matchController = matchController;
        }

        public bool TryGetEmptySlotGroup(out SlotGroup group)
        {
            if (_emptySlotGroups.Count == 0)
            {
                group = null;
                return false;
            }

            group = _emptySlotGroups.Pop();
            _filledSlotGroups.Push(group);

            return true;
        }
        
        public List<StickGroup> GetAvailableStickGroups(int emptySlotCountOfOtherIsland)
        {
            var color = GetFirstGroupColor();
            var stickGroupList = new List<StickGroup>();
            for (int i = 0; i < emptySlotCountOfOtherIsland; i++)
            {
                if (_filledSlotGroups.Count == 0) break;
                
                var slotGroup = _filledSlotGroups.Peek();
                if (slotGroup.GetSlotGroupColor() == color)
                {
                    stickGroupList.Add(slotGroup.CurrentStickGroup);
                    SetSlotGroupEmpty(slotGroup);
                }
                else
                {
                    break;
                }
            }

            return stickGroupList;
        }

        public void StickGroupTransition(List<StickGroup> stickGroups, Line roadLine)
        {
            var allTransitionStickCount = stickGroups.Count * _slotStickCount;
            for (int i = 0; i < stickGroups.Count; i++)
            {
                var slotGroup = _emptySlotGroups.Pop();
                _filledSlotGroups.Push(slotGroup);
                stickGroups[i].ChangeGroupPosition(slotGroup, roadLine, i, allTransitionStickCount);
            }
        }

        public int GetEmptySlotGroupCount()
        {
            return _emptySlotGroups.Count;
        }

        private bool IsThereEmptySlotGroup()
        {
            return _emptySlotGroups.Count > 0;
        }

        public bool IsIslandEmpty()
        {
            return _filledSlotGroups.Count == 0;
        }
        
        public Color GetFirstGroupColor()
        {
            return _filledSlotGroups.Peek().CurrentStickGroup.GetGroupColor();
        }
        
        public bool IsIslandOkayForMatch(Color otherIslandColor)
        {
            if (IsIslandEmpty())
                return true;

            return otherIslandColor == GetFirstGroupColor() && IsThereEmptySlotGroup();
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
            foreach (var slotGroup in _filledSlotGroups)
            {
                _emptySlotGroups.Push(slotGroup);
            }

            _filledSlotGroups.Clear();

            _onRoadSticks.Clear();
            Deactivate();
        }

        public void CompleteControl()
        {
            if (IsIslandComplete())
            {
                leanSelectable.enabled = false;
                _stickManager.CompleteColor(GetFirstGroupColor(),transform);
            }
        }

        public void AddStickTo(Stick stick)
        {
            _onRoadSticks.Add(stick);
        }

        public void RemoveStickTo(Stick stick)
        {
            _onRoadSticks.Remove(stick);
            if (_onRoadSticks.Count == 0)
            {
                CompleteControl();
            }
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

        private void SetSlotGroupEmpty(SlotGroup slotGroup)
        {
            _emptySlotGroups.Push(slotGroup);
            _filledSlotGroups.Pop();
        }
        
        
        private bool IsIslandComplete()
        {
            if (_emptySlotGroups.Count > 0) return false;

            var color = GetFirstGroupColor();
            return _slotGroups.All(slot => slot.GetSlotGroupColor() == color);
        }
    }
}