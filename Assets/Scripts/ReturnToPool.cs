using UnityEngine;
using UnityEngine.Pool;

public class ReturnToPool : MonoBehaviour
{
    public ParticleSystem system;
    public AudioSource audioSource;
    public IObjectPool<ParticleSystem> pool;

    void Start()
    {
        var main = system.main;
        main.stopAction = ParticleSystemStopAction.Callback;
        pool = LevelManager.deathParticlePool;
    }

    private void OnEnable()
    {
        audioSource.Play();
    }

    private void OnParticleSystemStopped()
    {
        pool.Release(system);
    }


}
