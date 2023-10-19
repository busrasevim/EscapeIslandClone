using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Stick : MonoBehaviour
{
    private Island.SlotGroup _currentSlotGroup;

    public void PrepareStick(Color color)
    {
        
    }
    
    public void GoNewPlace(Vector3 position, Island.SlotGroup group)
    {
        _currentSlotGroup = group;
        transform.DOMove(position, 1f);
    }
}
