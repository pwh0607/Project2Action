using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

public class CharacterSensor : MonoBehaviour
{
#region EVENTS
    [HorizontalLine("EVENTS"),HideField] public bool _h0;

    [SerializeField] EventSensorItemEnter eventSensorItemEnter;
    [SerializeField] EventSensorItemExit eventSensorItemExit;
    
    [Space(10), HorizontalLine(color:FixedColor.Cyan),HideField] public bool _h1;
#endregion

    public float interval = 0.5f; 
    public float detectionRadius = 5f;
    public float attackRange = 3f; 
    public float fieldOfViewAngle = 60f;

    private Dictionary<GameObject, TargetState> visibilityStates = new Dictionary<GameObject, TargetState>();

    public string targetTag;
    public bool showGizmos = true;

    GameObject target;
    CharacterControl owner;
    void OnEnable()
    {
        owner = GetComponentInParent<CharacterControl>();
    }

    void Update()
    {
        CheckItem();
    }

    private void CheckItem(){
        Debug.DrawRay(owner.eyePoint.transform.position, owner.transform.forward, Color.red, 3f);
        
        if(owner.currentItem != null) return;
        
        var list = Physics.OverlapSphere(owner.transform.position + owner.transform.forward, 2f, LayerMask.GetMask("HeavyObject"));

        if(list.Length <= 0) return;

        if(list[0].tag == "HEAVYOBJECT"){
            owner.pickableItem = list[0].GetComponent<Pickable>();
            //outline 생성하기
            return;
        }else if(list[0].tag == "DOOR"){
            Debug.Log("잠긴 문을 인식했다.");
        }
        
        owner.pickableItem = null;
    }
 

    void OnFound()
    {
        Debug.Log($"Found: {target.name}");
        eventSensorItemEnter.from = owner;
        eventSensorItemEnter.to = target;
        eventSensorItemEnter.Raise();
    }

    void OnLost()
    {
        Debug.Log($"Lost: {target.name}");
        eventSensorItemExit.from = owner;
        eventSensorItemExit.to = target;
        eventSensorItemExit.Raise();
    }

    void OnDrawGizmosSelected()
    {
        if (!showGizmos) return;

        if (visibilityStates == null)
            return;

        if (fieldOfViewAngle > 0)
        {
            Gizmos.color = Color.cyan;

            Vector3 forwardDir = transform.forward.normalized;
            Vector3 forwardEnd = transform.position + forwardDir * detectionRadius;

            Gizmos.DrawLine(transform.position, forwardEnd);

            float halfAngle = fieldOfViewAngle * 0.5f;
            Vector3 rightDir = Quaternion.AngleAxis(-halfAngle, Vector3.up) * forwardDir;
            Vector3 leftDir = Quaternion.AngleAxis(halfAngle, Vector3.up) * forwardDir;

            Vector3 leftEnd = transform.position + leftDir * detectionRadius;
            Vector3 rightEnd = transform.position + rightDir * detectionRadius;

            Gizmos.DrawLine(transform.position, leftEnd);
            Gizmos.DrawLine(transform.position, rightEnd);

            Gizmos.DrawLine(leftEnd, rightEnd);
        }

        foreach (var pair in visibilityStates)
        {
            GameObject target = pair.Key;
            TargetState state = pair.Value;

            if (target == null) continue;

            Gizmos.color = state.isVisible ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position, target.transform.position);
        }
    }
}