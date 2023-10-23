using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Game
{
    [Serializable]
    public class SlotGroup
    {
        public List<Vector3> SlotPositions { get; private set; }
        public Island CurrentIsland { get; private set; }
        public StickGroup CurrentStickGroup { get; private set; }

        public SlotGroup(int slotStickCount, int slotNumber, Island island)
        {
            SlotPositions = new List<Vector3>();
            CurrentIsland = island;

            var plusXValue = (CurrentIsland.rightBorder.localPosition.x - CurrentIsland.leftBorder.localPosition.x) /
                             (slotStickCount - 1);
            var plusZValue = (CurrentIsland.frontBorder.localPosition.z - CurrentIsland.behindBorder.localPosition.z) /
                             (slotStickCount - 1);

            var slotZPosition = CurrentIsland.behindBorder.localPosition.z + slotNumber * plusZValue;
            for (int i = 0; i < slotStickCount; i++)
            {
                var newPositionX = island.leftBorder.localPosition.x + i * plusXValue;
                var newPosition = new Vector3(newPositionX, 0f, slotZPosition);
                SlotPositions.Add(newPosition);
            }
        }
        
        public Color GetSlotGroupColor()
        {
            if (CurrentStickGroup != null)
                return CurrentStickGroup.GetGroupColor();

            return default;
        }

        public void SetCurrentStickGroup(StickGroup stickGroup)
        {
            CurrentStickGroup = stickGroup;
        }
    }
}
