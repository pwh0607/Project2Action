using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class AbilityTrace : Ability<AbilityTraceData>
{
    private NavMeshPath path;
    private Vector3[] corners;
    
    private int next;
    float currentVelocity;
   
    private CancellationTokenSource cts;
    
    public AbilityTrace(AbilityTraceData data, CharacterControl owner) : base(data, owner) {
        if(owner.Profile == null) return;

        path = new NavMeshPath();
        owner.isArrived = true;
        data.movePerSec = owner.Profile.moveSpeed;
        data.rotatePerSec = owner.Profile.rotateSpeed;

        cts = new CancellationTokenSource(); 
    }

    public override void Activate(object obj)
    {
        if(obj is CharacterControl control)
            data.target = control;

        if(data.target == null) return;

        owner.uiControl.Display($"{data.Flag}");
    }

    public override void Deactivate()
    {
        owner.Stop();
    }

    public override void Update()
    {
        TargetPosition();
        MoveAnimation();
    }

    public override void FixedUpdate()
    {
        if(owner == null || owner.rb == null) return;

        FollowPath();
    }

    private void TargetPosition(){

        if(data.target == null || !owner.isArrived) return;
        SetDestination(data.target.transform.position);
    }


    void SetDestination(Vector3 destination){
        if(!NavMesh.CalculatePath(owner.transform.position, destination, -1, path))
            return;

        corners = path.corners;
        next = 1;
        owner.isArrived = false;
    }
    
    Quaternion lookrot;
    private void FollowPath(){
        
        if(corners == null || corners.Length <= 0 || owner.isArrived) return;

        Vector3 nextTarget = corners[next];

        // 다음 위치 방향.
        Vector3 direction = (nextTarget - owner.rb.transform.position).normalized;
        direction.y = 0;
        
        // 회전
        if(direction != Vector3.zero) lookrot = Quaternion.LookRotation(direction);
        
        owner.transform.rotation = Quaternion.RotateTowards(owner.transform.rotation, lookrot, data.rotatePerSec * Time.deltaTime);

        Vector3 movement =  direction * data.movePerSec * 50f * Time.deltaTime;
        owner.rb.linearVelocity = movement;
        currentVelocity = Vector3.Distance(Vector3.zero, owner.rb.linearVelocity);
        
        if(Vector3.Distance(nextTarget, owner.rb.position) <= data.stopDistance){
            next++;
            if(next >= corners.Length){
                owner.Stop();
            }
        }
    }

    private void MoveAnimation(){
        float a = owner.isArrived ? 0 : Mathf.Clamp01(currentVelocity / data.movePerSec);
        owner.AnimateMoveSpeed(a);
    }
}


// EQS
/*
    Environment Query Systems
    Player의 주변환경 파악하기.
*/