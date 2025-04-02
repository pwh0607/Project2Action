using System.Collections;
using CustomInspector;
using UnityEngine;

public class AnimationEventList : MonoBehaviour
{
    private CharacterControl controller;
    public PoolableParticle smoke;
    public PoolableParticle jumpSmoke;
    [ReadOnly] public Transform modelRoot;
    [ReadOnly] Transform footLeft, footRight;

    void Awake()
    {
        TryGetComponent(out controller);    
    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => controller.model != null);

    }

    void OnValidate()
    {
        modelRoot = transform.FindSlot("model");

        if(modelRoot == null) Debug.LogWarning("Model Root 없음...");
        
        footLeft = modelRoot.FindSlot("leftfoot");
        footRight = modelRoot.FindSlot("rightfoot");
    }

    public void FootStep(string s)
    {
        if(controller.isArrived || footLeft == null || footRight == null) return;

        PoolManager.I.Spawn(smoke, s == "L" ? footLeft.position : footRight.position, Quaternion.identity, null);
    }

    public void FootStop(string s){
        if(footLeft == null || footRight == null) return;

        PoolManager.I.Spawn(smoke, s == "L" ? footLeft.position : footRight.position, Quaternion.identity, null);
    }

    public void JumpDown(){
        Vector3 offset = Vector3.up * 0.1f;
        PoolManager.I.Spawn(jumpSmoke, modelRoot.position + offset, Quaternion.identity, null);
    }
}