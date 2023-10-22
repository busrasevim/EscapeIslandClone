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

    public void GoNewPlace(Line line, Vector3 position, Island.SlotGroup group, int index)
    {
        _currentSlotGroup = group;
        var linePositions = line.GetPositions();
        var positions = new Vector3[linePositions.Length - 2];
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = linePositions[i + 1];
        }

        StartCoroutine(MoveToTargetIsland(positions, index, () =>
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

    private IEnumerator MoveToTargetIsland(Vector3[] roadPositions, int index, Action done)
    {
        var targetIndex = 0;
        var targetPosition = roadPositions[targetIndex];

       // var waitTime = index * 0.4f;
        //if (waitTime > 0.3f)
        {
          //  transform.DOMoveY(0f, 0.3f);
          //  direction = (targetPosition - new Vector3(transform.position.x,0f,transform.position.z)).normalized;
        }

        yield return new WaitForSeconds(index * 0.5f);
        transform.SetParent(_currentSlotGroup.currentIsland.transform);
        var direction = (targetPosition - transform.position).normalized;
        var targetRotation = Quaternion.LookRotation(direction);
        PlayRunAnimation();
        while (true)
        {
            if (Vector3.Distance(transform.position, targetPosition) > 0.025f)
            {
                transform.position += direction * Time.deltaTime * 1.5f;
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation,  9f*Time.deltaTime);

            }
            else
            {
                targetIndex++;
                if (targetIndex >= roadPositions.Length)
                {
                    break;
                }

                targetPosition = roadPositions[targetIndex];
                direction = (targetPosition - transform.position).normalized;
                targetRotation=Quaternion.LookRotation(direction);
            }

            yield return null;
            //await UniTask.Yield(PlayerLoopTiming.Update);
        }

        done.Invoke();
    }
}