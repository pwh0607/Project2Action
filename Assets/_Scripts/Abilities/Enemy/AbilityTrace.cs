using UnityEngine;
using UnityEngine.AI;

public class AbilityTrace : Ability<AbilityTraceData>
{
    private NavMeshPath path;
    private Vector3[] corners;
    
    private int next;
    float currentVelocity;
   
    public AbilityTrace(AbilityTraceData data, CharacterControl owner) : base(data, owner) {
        if(owner.Profile == null) return;

        path = new NavMeshPath();
        owner.isArrived = true;

        if(owner.Profile == null) return;
        data.movePerSec = owner.Profile.moveSpeed;
        data.rotatePerSec = owner.Profile.rotateSpeed;
    }

    float elapsed;
    public override void Activate()
    {
        GameObject _player = GameObject.FindGameObjectWithTag("Player");

        if(_player == null) return;

        data.traceTarget = GameObject.FindGameObjectWithTag("Player").transform;
        
        owner.Display($"{data.Flag}");
    }

    public override void Deactivate()
    {

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

        if(data.traceTarget == null) return;

        Vector3 destination = data.traceTarget.position;
        SetDestination(destination);
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
        // if(Vector3.Distance(data.traceTarget.transform.position, owner.transform.position) <= 2f){
        //     Debug.Log("추격 종료!");
        //     Deactivate();
        //     return;
        // }

        Vector3 nextTarget = corners[next];

        // 다음 위치 방향.
        Vector3 direction = (nextTarget - owner.rb.transform.position).normalized;
        direction.y = 0;
        
        // 회전
        if(direction != Vector3.zero) lookrot = Quaternion.LookRotation(direction);
        owner.transform.rotation = Quaternion.RotateTowards(owner.transform.rotation, lookrot, data.rotatePerSec * Time.deltaTime);

        //이동
        //linearVelocity : Vector + Scalar
        Vector3 movement =  direction * data.movePerSec * 50f * Time.deltaTime;
        owner.rb.linearVelocity = movement;
        currentVelocity = Vector3.Distance(Vector3.zero, owner.rb.linearVelocity);
        
        if(Vector3.Distance(nextTarget, owner.rb.position) <= data.stopDistance){
            next++;
            if(next >= corners.Length){
                owner.isArrived = true;
                owner.rb.linearVelocity = Vector3.zero;
            }
        }
    }

    private void MoveAnimation(){
        float a = owner.isArrived ? 0 : Mathf.Clamp01(currentVelocity / data.movePerSec);
        float spd = Mathf.Lerp(owner.animator.GetFloat(AnimationClipHashSet._MOVESPEED), a, Time.deltaTime * 10f);
        owner.animator.SetFloat(AnimationClipHashSet._MOVESPEED, spd);
    }
}