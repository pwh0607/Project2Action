using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class AbilityWandor : Ability<AbilityWandorData>
{
    private NavMeshPath path;
    private Vector3[] corners;
    
    private int next;
    float currentVelocity;
    
    private EnemyControl control;
    public AbilityWandor(AbilityWandorData data, IActorControl owner) : base(data, owner) {
        control = owner as EnemyControl;
        if(control.Profile == null) return;

        path = new NavMeshPath();
        control.isArrived = true;

        if(control.Profile == null) return;
        data.movePerSec = control.Profile.moveSpeed;
        data.rotatePerSec = control.Profile.rotateSpeed;
    }

    float elapsed;
    public override void Activate()
    {
        // RandomPosition();
    }

    public override void Deactivate()
    {
    }

    public override void Update()
    {
        elapsed += Time.deltaTime;

        if(elapsed > data.wandorStay){
            elapsed = 0f;
            RandomPosition();
        }
    }

    public override void FixedUpdate()
    {
        if(control == null || control.rb == null) return;

        FollowPath();
    }

    private void RandomPosition(){
        // 이동할 랜덤 위치 선정.
        // [-1  ~  1]
        
        // 가는 중이다.
        Vector3 randomPos = control.transform.position + Random.insideUnitSphere * data.wandorRadius;
        randomPos.y = 1f;
    
        SetDestination(randomPos);
    }


    void SetDestination(Vector3 destination){
        if(!NavMesh.CalculatePath(control.transform.position, destination, -1, path))
            return;

        corners = path.corners;
        next = 1;
        control.isArrived = false;
    }
    
    Quaternion lookrot;
    private void FollowPath(){
        if(corners == null || corners.Length <= 0 || control.isArrived) return;

        Vector3 nextTarget = corners[next];

        // 다음 위치 방향.
        Vector3 direction = (nextTarget - control.rb.transform.position).normalized;
        direction.y = 0;
        
        // 회전
        if(direction != Vector3.zero) lookrot = Quaternion.LookRotation(direction);
        control.transform.rotation = Quaternion.RotateTowards(control.transform.rotation, lookrot, data.rotatePerSec * Time.deltaTime);

        //이동
        //linearVelocity : Vector + Scalar
        Vector3 movement =  direction * data.movePerSec * 50f * Time.deltaTime;
        control.rb.linearVelocity = movement;
        currentVelocity = Vector3.Distance(Vector3.zero, control.rb.linearVelocity);
        
        if(Vector3.Distance(nextTarget, control.rb.position) <= data.stopDistance){
            next++;
            if(next >= corners.Length){
                control.isArrived = true;
                control.rb.linearVelocity = Vector3.zero;
            }
        }
    }

    private void MoveAnimation(){
        float a = control.isArrived ? 0 : Mathf.Clamp01(currentVelocity / data.movePerSec);
        float spd = Mathf.Lerp(control.animator.GetFloat(AnimationClipHashSet._MOVESPEED), a, Time.deltaTime * 10f);
        control.animator.SetFloat(AnimationClipHashSet._MOVESPEED, spd);
    }
}