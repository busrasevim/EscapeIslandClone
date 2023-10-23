using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Data;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.Game
{
    public class Stick : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Renderer selfRenderer;

        private SlotGroup _currentSlotGroup;
        private List<Vector3> _roadPositions;
        private Vector3 _targetPosition;
        private bool _onRoad;
        private Vector3 _localIslandPosition;
        private Line _currentMainLine;
        private Tween _toSlotTween;
        private GameSettings _settings;

        private Coroutine _moveToIslandCoroutine;

        private bool _isOnTransition;

        private static readonly int Run = Animator.StringToHash(Constants.Constants.StickRunAnimationKey);

        public void PrepareStick(Material material, GameSettings settings)
        {
            selfRenderer.material = material;
            _settings = settings;
            _roadPositions = new List<Vector3>();
        }

        public void GoNewPlace(Line line, Vector3 finalLocalPosition, SlotGroup slotGroup, int stickIndex,
            int allStickCount)
        {
            _toSlotTween?.Kill();
            if (_onRoad)
            {
                _isOnTransition = true;
                _currentSlotGroup.CurrentIsland.RemoveStickTo(this);
            }

            _currentSlotGroup = slotGroup;
            _localIslandPosition = finalLocalPosition;
            _currentSlotGroup.CurrentIsland.AddStickTo(this);

            var linePositions = line.GetPositions();
            var positions = new Vector3[linePositions.Length - 2];
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = linePositions[i + 1];
            }

            _roadPositions.Clear();
            if (_onRoad)
            {
                _roadPositions.Add(_targetPosition);
                _currentMainLine.AddSiblingLine(line);
            }
            else
            {
                _currentMainLine = line;
                _currentMainLine.AddStick(this);
            }

            _roadPositions.AddRange(positions);

            if (_onRoad)
            {
                transform.SetParent(_currentSlotGroup.CurrentIsland.transform);
                return;
            }

            _onRoad = true;

            _moveToIslandCoroutine =
                StartCoroutine(MoveToTargetIsland(stickIndex, () => { StickLastMovement(allStickCount, stickIndex); }));
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            StopRunAnimation();
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void Reset()
        {
            _onRoad = false;
            _isOnTransition = false;
            if (_moveToIslandCoroutine != null)
                StopCoroutine(_moveToIslandCoroutine);
            
            _toSlotTween?.Kill();
            _roadPositions.Clear();
            _currentMainLine = null;
            StopRunAnimation();
            Deactivate();
        }

        private void StickLastMovement(int allOnRoadStickCount, int stickIndex)
        {
            _onRoad = false;
            TransitionCompleteControl(allOnRoadStickCount, stickIndex);
            _toSlotTween = transform.DOLocalMove(_localIslandPosition, _settings.lastMoveTime).OnComplete(() =>
            {
                StopRunAnimation();
                _toSlotTween = transform.DORotate(_currentSlotGroup.CurrentIsland.transform.eulerAngles,
                    _settings.lastRotateTime);
            });
        }

        private void PlayRunAnimation()
        {
            animator.SetBool(Run, true);
        }

        private void StopRunAnimation()
        {
            animator.SetBool(Run, false);
        }

        private IEnumerator MoveToTargetIsland(int index, Action done)
        {
            yield return new WaitForSeconds(index * _settings.groupMoveDelayTime);

            _targetPosition = _roadPositions[0];
            transform.SetParent(_currentSlotGroup.CurrentIsland.transform);
            var direction = (_targetPosition - transform.position).normalized;
            var targetRotation = Quaternion.LookRotation(direction);

            PlayRunAnimation();
            while (true)
            {
                if (_isOnTransition)
                {
                    yield return new WaitForSeconds(0.1f);
                    _isOnTransition = false;
                }

                if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z),
                        new Vector2(_targetPosition.x, _targetPosition.z)) > 0.025f)
                {
                    transform.position += direction * (Time.deltaTime * _settings.stickMoveSpeed);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation,
                        _settings.stickRotateSpeed * Time.deltaTime);
                }
                else
                {
                    _roadPositions.RemoveAt(0);
                    if (_roadPositions.Count == 0)
                    {
                        break;
                    }

                    _targetPosition = _roadPositions[0];
                    direction = (_targetPosition - transform.position).normalized;
                    targetRotation = Quaternion.LookRotation(direction);
                }

                yield return null;
            }

                done.Invoke();
        }


        private void OnDestroy()
        {
            if (_moveToIslandCoroutine != null)
                StopCoroutine(_moveToIslandCoroutine);
        }

        private void TransitionCompleteControl(int onRoadStickCount, int currentIndex)
        {
            _currentSlotGroup.CurrentIsland.RemoveStickTo(this);
            _currentMainLine?.RemoveStick(this);
        }
    }
}