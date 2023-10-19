using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FXManager : MonoBehaviour
{
    private TimeScaleManager _timeScaleManager;

    private ParticleLibrary _particleLibrary;

    [Inject]
    private void Initialize(TimeScaleManager timeScaleManager)
    {
        _timeScaleManager = timeScaleManager;
    }

    public void PlayLevelCompleteFX()
    {
        PlayCelebrationParticleSystem(Vector3.zero);
    }

    private void PlayCelebrationParticleSystem(Vector3 position)
    {
        _particleLibrary.celebrationPS.Play();
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
