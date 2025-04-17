using CustomInspector;
using UnityEngine;

public class SensorControl : MonoBehaviour
{
    #region Event
    [HorizontalLine("Sensor-Event"), HideInInspector] public bool l_s_1;

    [SerializeField] EventSensorSightEnter eventSensorSightEnter;
    [SerializeField] EventSensorSightExit eventSensorSightExit;
    
    [SerializeField] EventSensorAttackEnter eventSensorAttackEnter;
    [SerializeField] EventSensorAttackExit eventSensorAttackExit;

    [HorizontalLine(color:FixedColor.Cyan), HideInInspector] public bool l_e_1;
    #endregion

    [Tooltip("시야 범위")]
    [SerializeField] float sightRange = 5f;
    [SerializeField] float attackRange = 1f;
    
    private CharacterControl owner;

    [Space(20)]
    [SerializeField] LayerMask targetLayer;
    [SerializeField] string targetTag;
    [SerializeField] float interval;
    
    [Space(20)]
    [ReadOnly] public CharacterControl target;
    [ReadOnly] public CharacterControl prevSight;
    [ReadOnly] public CharacterControl prevAttack;

    void Start(){
        TryGetComponent(out owner);
        InvokeRepeating("CheckOverlap", 0f, interval);
    }

    void CheckOverlap(){
        var cols = Physics.OverlapSphere(owner.transform.position, sightRange, targetLayer);

        foreach(var c in cols){
            if(c.CompareTag(targetTag)){
                target = c.GetComponentInParent<CharacterControl>();
                TargetEnter();
                
                if(!target.isDamageable) continue;

                float distance = Vector3.Distance(target.transform.position, owner.transform.position);
                if(distance <= attackRange){
                    AttackEnter();
                }
                else{
                    AttackExit();
                }
                return;
            }
        }
        AttackExit();
        TargetExit();
    }
    
    private void TargetEnter()
    {
        if(prevSight == target || target == null)
            return;
            
        prevSight = target;
        
        eventSensorSightEnter.from = owner;
        eventSensorSightEnter.to = target;
        eventSensorSightEnter.Raise();
    }

    private void TargetExit()
    {
        if(prevSight == null || target == null)
            return;

        prevSight = null;
        target = null;

        eventSensorSightExit.from = owner;
        eventSensorSightExit.to = target;
        eventSensorSightExit.Raise();
    }


    #region 공격 범위 체크
    private void AttackEnter(){        
        if(prevAttack == target || prevSight == null || target == null) return;

        prevAttack = target;
        
        eventSensorAttackEnter.from = owner;
        eventSensorAttackEnter.to = target;
        eventSensorAttackEnter.Raise();
    }

    private void AttackExit(){
        if(prevAttack == null || target == null) return;

        prevAttack = null;

        eventSensorAttackExit.from = owner;
        eventSensorAttackExit.to = target;
        eventSensorAttackExit.Raise();
    }
    #endregion
}