using System.Collections;
using CustomInspector;
using UnityEngine;

public class AnimationEventListener : MonoBehaviour
{
    private CharacterControl controller;
    public PoolableParticle smoke;
    public PoolableParticle jumpSmoke;
    [ReadOnly] public Transform modelRoot;
    [ReadOnly] Transform footLeft, footRight;

    [HorizontalLine("Event-Spawn"), HideField] public bool h_s_01;
    [SerializeField] EventPlayerSpawnAfter eventPlayerSpawnAfter;

    [HorizontalLine(color:FixedColor.Cyan), HideField] public bool h_e_01;


    void OnEnable()
    {
        eventPlayerSpawnAfter.Register(OnEventPlayerSpawnAfter);
    }

    void OnDisable()
    {
        eventPlayerSpawnAfter.UnRegister(OnEventPlayerSpawnAfter);
    }

    void OnEventPlayerSpawnAfter(EventPlayerSpawnAfter e){
        modelRoot = transform.FindSlot("_model_");

        if(modelRoot == null) Debug.LogWarning("Model Root 없음...");
        StartCoroutine(WaitFind());
    }
    
    IEnumerator WaitFind(){
        yield return new WaitForSeconds(1f);
        
        footLeft = modelRoot.FindSlot("leftfoot");
        footRight = modelRoot.FindSlot("rightfoot");
    }

    void Awake()
    {
        TryGetComponent(out controller);    
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
        PoolManager.I.Spawn(jumpSmoke, controller.model.position + offset, Quaternion.identity, null);
    }
}