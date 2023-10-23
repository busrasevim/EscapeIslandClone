using System;
using System.Collections.Generic;
using System.Threading;
using _Project.Scripts.Data;
using Cysharp.Threading.Tasks;
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

        private GameSettings _settings;

        private CancellationToken _cancellationToken;
        private CancellationTokenSource _moveIslandCancellationSource;

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
            if (_onRoad) _currentSlotGroup.CurrentIsland.RemoveStickTo(this);

            _currentSlotGroup = slotGroup;
            _localIslandPosition = finalLocalPosition;
            _currentSlotGroup.CurrentIsland.AddStickTo(this);

            var linePositions = line.GetPositions();
            var positions = new Vector3[linePositions.Length - 2];
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = linePositions[i + 1];
            }

            if (_onRoad)
            {
                _roadPositions.Clear();
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

            MoveToTargetIsland(_cancellationToken, stickIndex, () => { StickLastMovement(allStickCount, stickIndex); });
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void Reset()
        {
            Deactivate();
            _roadPositions.Clear();
            _onRoad = false;
            CancelMovementToken();
        }

        private void StickLastMovement(int allOnRoadStickCount, int stickIndex)
        {
            transform.DOLocalMove(_localIslandPosition, _settings.lastMoveTime).OnComplete(() =>
            {
                StopRunAnimation();
                transform.DORotate(_currentSlotGroup.CurrentIsland.transform.eulerAngles, _settings.lastRotateTime);
                _onRoad = false;
                TransitionCompleteControl(allOnRoadStickCount, stickIndex);
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

        private async void MoveToTargetIsland(CancellationToken cancellationToken, int index, Action done)
        {
            _moveIslandCancellationSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _targetPosition = _roadPositions[0];

            await UniTask.Delay(TimeSpan.FromSeconds(index * _settings.groupMoveDelayTime),
                    cancellationToken: _moveIslandCancellationSource.Token)
                .SuppressCancellationThrow();
            transform.SetParent(_currentSlotGroup.CurrentIsland.transform);
            var direction = (_targetPosition - transform.position).normalized;
            var targetRotation = Quaternion.LookRotation(direction);
            PlayRunAnimation();
            _onRoad = true;
            while (!_moveIslandCancellationSource.IsCancellationRequested)
            {
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

                await UniTask.Yield(cancellationToken: _moveIslandCancellationSource.Token).SuppressCancellationThrow();
            }

            done.Invoke();
        }


        private void OnDestroy()
        {
            CancelMovementToken();
        }

        private void CancelMovementToken()
        {
            if (_moveIslandCancellationSource is { Token: { IsCancellationRequested: false } })
            {
                _moveIslandCancellationSource.Cancel();
            }
        }

        private void TransitionCompleteControl(int onRoadStickCount, int currentIndex)
        {
            _currentMainLine.RemoveStick(this);
            _currentSlotGroup.CurrentIsland.RemoveStickTo(this);
        }
    }
}