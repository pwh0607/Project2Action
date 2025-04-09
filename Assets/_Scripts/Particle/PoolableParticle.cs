using UnityEngine;

public class PoolableParticle : PoolBehaviour
{
    private ParticleSystem ps;

    void Awake()
    {
        if(!TryGetComponent(out ps))
            Debug.LogWarning($"PoolableParticle {gameObject.name} ] ParticleSystem 없음...");
    }

    void OnEnable()
    {
        ps.Play();
    }

    void OnDisable()
    {
        Despawn();
    }
}