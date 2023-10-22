using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Stick : MonoBehaviour
{
    private Island.SlotGroup _currentSlotGroup;
    [SerializeField] private Animator animator;
    private List<Vector3> _roadPositions;
    private Vector3 _targetPosition;
    private bool _onRoad;
    private Vector3 _localIslandPosition;
    
    private static readonly int Run = Animator.StringToHash("Run");
    public void PrepareStick(Color color)
    {
        GetComponentInChildren<Renderer>().material.color = color;
        _roadPositions = new List<Vector3>();
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

        _localIslandPosition = position;
        if (_onRoad)
        {
            _roadPositions.Clear();
            _roadPositions.Add(_targetPosition);
        }
        
        _roadPositions.AddRange(positions);

        if (_onRoad)
        {
            transform.SetParent(_currentSlotGroup.currentIsland.transform);
            return;
        }
        _onRoad = true;
        
        StartCoroutine(MoveToTargetIsland(index, () =>
        {
            transform.DOLocalMove(_localIslandPosition, 1f).OnComplete(() =>
            {
                StopRunAnimation();
                transform.DORotate(_currentSlotGroup.currentIsland.transform.eulerAngles, 0.5f);
                line.Deactivate();
                _onRoad = false;
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

    private IEnumerator MoveToTargetIsland(int index, Action done)
    {
      //  var targetPosition = roadPositions[targetIndex];
        _targetPosition = _roadPositions[0];

        yield return new WaitForSeconds(index * 0.5f);
        transform.SetParent(_currentSlotGroup.currentIsland.transform);
        var direction = (_targetPosition - transform.position).normalized;
        var targetRotation = Quaternion.LookRotation(direction);
        PlayRunAnimation();
        while (true)
        {
            if (Vector2.Distance(new Vector2(transform.position.x,transform.position.z), new Vector2(_targetPosition.x,_targetPosition.z)) > 0.025f)
            {
                transform.position += direction * Time.deltaTime * 1.5f;
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation,  9f*Time.deltaTime);

            }
            else
            {
                _roadPositions.RemoveAt(0);
                if (_roadPositions.Count==0)
                {
                    break;
                }

                _targetPosition = _roadPositions[0];
                direction = (_targetPosition - transform.position).normalized;
                Debug.Log(direction);
                targetRotation=Quaternion.LookRotation(direction);
            }

            yield return null;
            //await UniTask.Yield(PlayerLoopTiming.Update);
        }

        done.Invoke();
    }
}