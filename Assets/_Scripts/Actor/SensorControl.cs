using CustomInspector;
using UnityEngine;

public class SensorControl : MonoBehaviour
{
    #region Event
    [HorizontalLine("Sensor-Event"), HideInInspector] public bool l_s_1;
    [SerializeField] EventSensorTargetEnter eventSensorTargetEnter;
    [SerializeField] EventSensorTargetExit eventSensorTargetExit;
    [HorizontalLine(color:FixedColor.Cyan), HideInInspector] public bool l_e_1;
    #endregion

    [Tooltip("시야 범위")]
    [SerializeField] float sightRange;
    
    // 자기 자신
    private CharacterControl owner;

    [SerializeField] LayerMask targetLayer;
    [SerializeField] string targetTag;
    
    // target
    [ReadOnly] public CharacterControl target;

    void Start()
    {
        TryGetComponent(out owner);
    }

    [ReadOnly] public CharacterControl _prev;
    void Update()
    {
        /*
            Physics.SphereCast => bool
            Physics.OverlapSphere => Collider[]
        */

        // 1. Layer 필터
        var cols = Physics.OverlapSphere(transform.position, sightRange, targetLayer);

        // 2. 태그 필터
        foreach(var c in cols){
            if(c.CompareTag(targetTag)){
                target = c.GetComponentInParent<CharacterControl>();
                TargetEnter();

                return;
            }
        }

        TargetExit();
    }

    private void TargetEnter()
    {
        if(_prev == target || target == null)
            return;

        _prev = target;
        
        Debug.Log($"Target Enter: {target.Profile.alias}");
        eventSensorTargetEnter.from = owner;
        eventSensorTargetEnter.to = target;
        eventSensorTargetEnter.Raise();
    }

    private void TargetExit()
    {
        if(_prev == null || target == null)
            return;
        
        _prev = null;
        // target = null;
        Debug.Log($"Target Exit: {target.Profile.alias}");
        eventSensorTargetExit.from = owner;
        eventSensorTargetExit.to = target;
        eventSensorTargetExit.Raise();
    }
}

//sensorControl -> EnemyEventControl