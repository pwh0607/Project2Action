using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
using CustomInspector;

public class AnimationEventListener : MonoBehaviour
{
    public PoolableParticle footSmoke, jumpSmoke, swing1;

    [HorizontalLine("Event-Spawn"), HideField] public bool h_s_01;
    
    [SerializeField] EventPlayerSpawnAfter eventPlayerSpawnAfter;
    [SerializeField] EventEnemySpawnAfter eventEnemySpawnAfter;
    [SerializeField] EventAttackBefore eventAttackBefore;
    
    [HorizontalLine(color:FixedColor.Cyan), HideField] public bool h_e_01;
    
    [Space(20)]
    private CharacterControl owner;
    [ReadOnly] public Transform modelRoot;
    
    [ReadOnly] public Transform footLeft, footRight;
    [ReadOnly] public Transform handLeft, handRight;
    

    public VisualEffect vfxSwing;

    void Awake()
    {
        TryGetComponent(out owner);    

        modelRoot = transform.FindSlot("_model_");
        if(modelRoot == null) Debug.LogWarning("AnimationEventListener ] Model Root 없음...");
    }

    void OnEnable()
    {

        eventPlayerSpawnAfter.Register(OnEventPlayerSpawnAfter);

        eventEnemySpawnAfter.Register(OnEventEnemySpawnAfter);
    }

    void OnDisable()
    {
        eventPlayerSpawnAfter.Unregister(OnEventPlayerSpawnAfter);

        eventEnemySpawnAfter.Unregister(OnEventEnemySpawnAfter);
    }

    void OnEventPlayerSpawnAfter(EventPlayerSpawnAfter e){
        if(owner != e.character)
            return;
        StartCoroutine(DelayFind());
    }

    public void OnEventEnemySpawnAfter(EventEnemySpawnAfter e){
        if (owner != e.character)
            return;
        
        StartCoroutine(DelayFind());
    }
    
    IEnumerator DelayFind(){
        yield return new WaitForEndOfFrame();

        footRight = modelRoot.FindSlot("rightfoot", "Ball_R", "r foot", "Rfoot");
        footLeft = modelRoot.FindSlot("leftfoot", "Ball_L", "l foot", "Lfoot");
        handLeft = modelRoot.FindSlot("L Hand","LeftHand");
        handRight = modelRoot.FindSlot("R Hand","LeftHand");
    }


    public void FootStep(string s)
    {
        if(owner.isArrived == true || footLeft == null || footRight == null) return;
        PoolManager.I.Spawn(footSmoke, s == "L" ? footLeft.position : footRight.position, Quaternion.identity, null);
    }

    public void FootStop(string s){
        if(footLeft == null || footRight == null) return;

        PoolManager.I.Spawn(footSmoke, s == "L" ? footLeft.position : footRight.position, Quaternion.identity, null);
    }

    public void JumpDown(){
        Vector3 offset = Vector3.up * 0.1f + Random.insideUnitSphere * 0.2f;
        PoolManager.I.Spawn(jumpSmoke, owner.model.position + offset, Quaternion.identity, null);
    }

    public void Attack(string s){
        eventAttackBefore.from = owner;
        eventAttackBefore.Raise();
        
        Debug.Log("Enemy 공격!");

        int rnd = Random.Range(0,3);
        if(rnd <1) return;

        var rot = Quaternion.LookRotation(owner.transform.forward);
        rot.eulerAngles = new Vector3(-90f, rot.eulerAngles.y, 0f);
        
        PoolManager.I.Spawn(swing1, s == "L" ? handLeft.position : handRight.position, rot, null);
    }

    // string => on : 시작, off : 종료

    /*

    */
    public void Swing(string on){
        if(vfxSwing == null) return;

        if(on == "on"){
            vfxSwing.Play();
        }else{
            vfxSwing.Stop();
        }

    }
}