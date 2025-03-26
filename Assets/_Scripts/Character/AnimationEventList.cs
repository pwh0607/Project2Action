using UnityEngine;

public class AnimationEventList : MonoBehaviour
{
    private CharacterControl controller;
    public Transform Lfoot, Rfoot;
    public PoolableParticle smoke;
    public PoolableParticle jumpSmoke;
    void Awake()
    {
        TryGetComponent(out controller);    
    }

    public void FootStep(string s)
    {
        if(controller.isArrived) return;

        PoolManager.I.Spawn(smoke, s == "L" ? Lfoot.position : Rfoot.position, Quaternion.identity, null);
    }

    public void FootStop(string s){
        if(!controller.isArrived) return;

        PoolManager.I.Spawn(smoke, s == "L" ? Lfoot.position : Rfoot.position, Quaternion.identity, null);
    }

    public void JumpDown(){
        PoolManager.I.Spawn(jumpSmoke, Rfoot.position, Quaternion.identity, null);
    }
}