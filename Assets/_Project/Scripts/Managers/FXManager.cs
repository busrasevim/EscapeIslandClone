using _Project.Scripts.Data;
using _Project.Scripts.Game.Constants;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Managers
{
    public class FXManager : IInitializable
    {
        [Inject] private ParticleLibrary _particleLibrary;
        private ParticleSystemPool _islandCompletePSPool;
        private GameObject _psPoolParent;
    
        public void Initialize()
        {
            _psPoolParent = new GameObject(Constants.LevelGameObjectName);
           // _islandCompletePSPool = new ParticleSystemPool(12, _particleLibrary.islandCompletedPS, _psPoolParent.transform);
        }
    
        public void PlayLevelCompleteFX()
        {
            PlayCelebrationParticleSystem(Vector3.zero);
        }

        public void PlayIslandCompleteFX(Vector3 islandPosition)
        {
            _islandCompletePSPool.Play(islandPosition);
        }
        
        private void PlayCelebrationParticleSystem(Vector3 position)
        {
            _particleLibrary.levelCompletedPS.Play();
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