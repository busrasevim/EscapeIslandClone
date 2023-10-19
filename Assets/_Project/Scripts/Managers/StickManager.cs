using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StickManager : IInitializable
{
    [Inject] private ObjectPool _objectPool;
    
    public void Initialize()
    {
        Debug.Log("Stick manag");
    }

    //called from generator
    public void SpawnNewStickGroup(int groupCount, Island.SlotGroup group, Color color)
    {
        for (int i = 0; i < groupCount; i++)
        {
            var stick = new StickGroup(4, group, _objectPool);
        }
    }
    
    public struct StickGroup
    {
        public List<Stick> stickGroup;
        public Island.SlotGroup currentSlotGroup;
        public Color stickGroupColor;

        public StickGroup(int stickCount, Island.SlotGroup group, ObjectPool pool)
        {
            stickGroup = new List<Stick>();
            stickGroupColor=Color.white;
            currentSlotGroup = group;
            
            for (int i = 0; i < stickCount; i++)
            {
                var stick = pool.SpawnFromPool(PoolTags.Stick, Vector3.zero, Quaternion.identity).GetComponent<Stick>();
                stick.PrepareStick(stickGroupColor);
                stick.GoNewPlace(currentSlotGroup.slotPositions[i], currentSlotGroup);
                stickGroup.Add(stick);
            }
        }
    }
}
