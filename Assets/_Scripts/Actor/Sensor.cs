using System.Collections.Generic;
using CustomInspector;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Events;

public struct TargetState
{
    public bool isVisible;
    public bool isArrived;
}

public class Sensor : MonoBehaviour
{
    #region Event
    [HorizontalLine("Sensor-Event"), HideInInspector] public bool l_s_1;

    [SerializeField] EventSensorSightEnter eventSensorSightEnter;
    [SerializeField] EventSensorSightExit eventSensorSightExit;
    [SerializeField] EventSensorAttackEnter eventSensorAttackEnter;
    [SerializeField] EventSensorAttackExit eventSensorAttackExit;

    [HorizontalLine(color:FixedColor.Cyan), HideInInspector] public bool l_e_1;
    #endregion



    [Header("Detection Settings")]
    public float interval = 0.5f;
    public float detectionRadius = 5f;
    public float arrivedRadius = 3f; 
    public float fieldOfViewAngle = 60f;
    public LayerMask targetLayer;
    public LayerMask blockLayer;

    public string targetTag = "ENEMY";
    public bool showGizmos = true;
 
    private Dictionary<CharacterControl, TargetState> visibilityStates = new Dictionary<CharacterControl, TargetState>();
    [SerializeField, ReadOnly] private CharacterControl owner, target;


    void Start()
    {
        owner = GetComponentInParent<CharacterControl>();
        if (owner == null)
            Debug.LogWarning("Sensor ] owner - CharacterControl 없음");

        InvokeRepeating("DetectTargets", 0f, interval);
    }

    void DetectTargets()
    {
        HashSet<CharacterControl> currentFrameTargets = new HashSet<CharacterControl>();
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, targetLayer);

        foreach (Collider hit in hits)
        {
            if (!hit.CompareTag(targetTag))
                continue;

            target = hit.GetComponentInParent<CharacterControl>();
            if (target == null)
                Debug.LogWarning("Sensor ] target - CharacterControl 없음");

            Vector3 direction = (target.eyePoint.position - transform.position).normalized;

            float angle = Vector3.Angle(transform.forward, direction);
            if (angle > (fieldOfViewAngle * 0.5f))
                continue;

            currentFrameTargets.Add(target);

            // 타겟과의 거리 체크
            float distance = Vector3.Distance(transform.position, target.eyePoint.position);
            // 타겟과의 장애물 체크
            bool isVisible = !Physics.Raycast(transform.position, direction, distance, blockLayer);
            // 타겟 도착 체크
            bool isArrived = distance <= arrivedRadius;

            //현재 상태 가져오기
            visibilityStates.TryGetValue(target, out TargetState previousState);

            TargetState newState = new TargetState
            {
                isVisible = isVisible,
                isArrived = isArrived
            };

            // 새로운 타겟 출현
            if (!visibilityStates.ContainsKey(target))
            {
                visibilityStates[target] = newState;
                if(isVisible)
                    OnFound();
                else
                    OnBlocked();
                
                if (isArrived)
                    OnArrived();
            }

            // 기존 타겟의 상태 변경
            else if (previousState.isVisible != isVisible || previousState.isArrived != isArrived)
            {
                visibilityStates[target] = newState;
                if(isVisible)
                    OnFound();
                else
                    OnBlocked();

                if (isArrived && !previousState.isArrived)
                    OnArrived();
            }
        }

        // 삭제할 목록 작성
        List<CharacterControl> toRemove = new List<CharacterControl>();
        foreach (var kvp in visibilityStates)
        {
            if (!currentFrameTargets.Contains(kvp.Key))
            {
                toRemove.Add(kvp.Key);
                OnLost();
            }
        }

        // 실제 삭제
        foreach (var t in toRemove)
            visibilityStates.Remove(t);
    }

    
    void OnFound()
    {
        owner.uiControl.Display("FOUND"); 
       
        eventSensorSightEnter.from = owner;
        eventSensorSightEnter.to = target;
        eventSensorSightEnter.Raise();
    }

    void OnBlocked()
    {
        owner.uiControl.Display("BLOCKED");

        eventSensorSightExit.from = owner;
        eventSensorSightExit.to = target;
        eventSensorSightExit.Raise();

        target = null;
    }

    void OnLost()
    {        
        owner.uiControl.Display("LOST");

        eventSensorAttackExit.from = owner;
        eventSensorAttackExit.to = target;
        eventSensorAttackExit.Raise();
    }

    void OnArrived()
    {        
        owner.uiControl.Display("ARRIVED");

        eventSensorAttackEnter.from = owner;
        eventSensorAttackEnter.to = target;
        eventSensorAttackEnter.Raise();
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
            CharacterControl target = pair.Key;
            TargetState state = pair.Value;

            if (target == null) continue;

            Gizmos.color = state.isVisible ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position, target.eyePoint.position);
        }
    }
}