using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Stick : MonoBehaviour
{
    private Island.SlotGroup _currentSlotGroup;
    [SerializeField] private Animator animator;
    private const string RUN_ANIMATION_BOOL_KEY = "Run";

    public void PrepareStick(Color color)
    {
        GetComponentInChildren<Renderer>().material.color = color;
    }
    
    public void GoNewPlace(Vector3 position, Island.SlotGroup group)
    {
        _currentSlotGroup = group;
        animator.SetBool(RUN_ANIMATION_BOOL_KEY, true);
        transform.DOLocalMove(position, 1f).OnComplete(() =>
        {
            animator.SetBool(RUN_ANIMATION_BOOL_KEY, false);
            transform.DORotate(group.currentIsland.transform.eulerAngles, 0.5f);
        });
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }
}
