using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StickManager
{
    [Inject] private ObjectPool _objectPool;

    //called from generator
    public void SpawnNewStickGroup(Transform island, Island.SlotGroup group, Color color)
    {
        var stick = new StickGroup(island, 4, group, _objectPool,color);
    }
    
    public struct StickGroup
    {
        public List<Stick> stickGroup;
        public Island.SlotGroup currentSlotGroup;
        public Color stickGroupColor;

        public StickGroup(Transform island, int stickCount, Island.SlotGroup group, ObjectPool pool, Color color)
        {
            stickGroup = new List<Stick>();
            stickGroupColor = color;
            currentSlotGroup = group;
            
            for (int i = 0; i < stickCount; i++)
            {
                var stick = pool.SpawnFromPool(PoolTags.Stick, Vector3.zero, Quaternion.identity).GetComponent<Stick>();
                stick.transform.SetParent(island);
                stick.PrepareStick(stickGroupColor);
                stick.GoNewPlace(currentSlotGroup.slotPositions[i], currentSlotGroup);
                stickGroup.Add(stick);
            }
        }
    }
}
