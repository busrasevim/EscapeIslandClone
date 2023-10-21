using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Stick : MonoBehaviour
{
    private Island.SlotGroup _currentSlotGroup;
    [SerializeField] private Animator animator;
    private static readonly int Run = Animator.StringToHash("Run");

    public void PrepareStick(Color color)
    {
        GetComponentInChildren<Renderer>().material.color = color;
    }

    public void GoNewPlace(Line line, Vector3 position, Island.SlotGroup group)
    {
        _currentSlotGroup = group;
        var linePositions = line.GetPositions();
        var positions = new Vector3[linePositions.Length - 2];
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = linePositions[i + 1];
        }

        PlayRunAnimation();
        StartCoroutine(MoveToTargetIsland(positions, () =>
        {
            transform.DOLocalMove(position, 1f).OnComplete(() =>
            {
                StopRunAnimation();
                transform.DORotate(group.currentIsland.transform.eulerAngles, 0.5f);
                line.Deactivate();
            });
        }));
    }

    private void PlayRunAnimation()
    {
        animator.SetBool(Run, true);
    }

    private void StopRunAnimation()
    {
        animator.SetBool(Run, false);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    private IEnumerator MoveToTargetIsland(Vector3[] roadPositions, Action done)
    {
        var targetIndex = 0;
        var targetPosition = roadPositions[targetIndex];
        var direction = (targetPosition - transform.position).normalized;

        while (true)
        {
            if (Vector3.Distance(transform.position, targetPosition) > 0.025f)
            {
                transform.position += direction * Time.deltaTime;
                // transform.localPosition = Vector3.MoveTowards(transform.position, targetPosition, 0.1f * Time.deltaTime);
            }
            else
            {
                targetIndex++;
                if (targetIndex >= roadPositions.Length)
                {
                    break;
                }

                targetPosition = roadPositions[targetIndex];
                direction = targetPosition - transform.position;
            }

            yield return null;
            //await UniTask.Yield(PlayerLoopTiming.Update);
        }

        done.Invoke();
    }
}