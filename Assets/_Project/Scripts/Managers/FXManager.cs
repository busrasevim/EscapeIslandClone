using _Project.Scripts.Data;
using _Project.Scripts.Game.Constants;
using _Project.Scripts.Level.Signals;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Managers
{
    public class FXManager : IInitializable
    {
        [Inject] private ParticleLibrary _particleLibrary;
        [Inject] private SignalBus _signalBus;
        private ParticleSystemPool _islandCompletePSPool;
        private ParticleSystem _levelCompletedPS;
        private GameObject _psPoolParent;
        
        
    
        public void Initialize()
        {
            _signalBus.Subscribe<OnLevelEndSignal>(OnLevelEnd);
            _levelCompletedPS = Object.Instantiate(_particleLibrary.levelCompletedPS);
            _levelCompletedPS.transform.SetParent(Camera.main.transform);
            _levelCompletedPS.transform.localPosition = new Vector3(0f, -4.71f, 1.7f);
            _levelCompletedPS.transform.localEulerAngles = new Vector3(270f, 0, 0);
            
            _psPoolParent = new GameObject(Constants.LevelGameObjectName);
            _islandCompletePSPool = new ParticleSystemPool(12, _particleLibrary.islandCompletedPS, _psPoolParent.transform);
        }
    
        private void PlayLevelCompleteFX()
        {
            PlayCelebrationParticleSystem();
        }

        public void PlayIslandCompleteFX(Transform islandTransform)
        {
            var ps = _islandCompletePSPool.Play(islandTransform.position);
            ps.transform.rotation = islandTransform.rotation;
        }
        
        private void PlayCelebrationParticleSystem()
        {
            _levelCompletedPS.Play();
        }

        private void OnLevelEnd(OnLevelEndSignal args)
        {
            if(args.IsWin)
                PlayLevelCompleteFX();
        }
    }

    public class ParticleSystemPool
    {
        private readonly int _poolSize;
        private readonly ParticleSystem[] _particleSystems;

        private int _index;

        public ParticleSystemPool(int poolSize, ParticleSystem particleSystemPrefab, Transform parent)
        {
            _index = 0;
            _poolSize = poolSize;
            _particleSystems = new ParticleSystem[poolSize];

            for (int i = 0; i < _poolSize; i++)
            {
                _particleSystems[i] = Object.Instantiate(particleSystemPrefab, parent);
                _particleSystems[i].Stop();
            }
        }

        public ParticleSystem Play(Vector3 position)
        {
            var ps = _particleSystems[_index];
            ps.transform.position = position;
            ps.Play();

            _index = (_index + 1) % _poolSize;

            return ps;
        }
    }
}