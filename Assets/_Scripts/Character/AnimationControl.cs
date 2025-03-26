using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    public PoolableParticle pp;

    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            PoolManager.I.Spawn(pp, Random.onUnitSphere * 0.1f, Quaternion.identity, transform);
        }
    }
}