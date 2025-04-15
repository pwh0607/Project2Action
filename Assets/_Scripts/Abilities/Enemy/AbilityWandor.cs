using UnityEngine;
using UnityEngine.AI;

public class AbilityWandor : Ability<AbilityWandorData>
{
    private NavMeshPath path;
    private Vector3[] corners;
    
    private int next;
    float currentVelocity;
   
    public AbilityWandor(AbilityWandorData data, CharacterControl owner) : base(data, owner) {
        if(owner.Profile == null) return;

        path = new NavMeshPath();
        owner.isArrived = true;

        data.movePerSec = owner.Profile.moveSpeed;
        data.rotatePerSec = owner.Profile.rotateSpeed;
    }

    float elapsed;
    public override void Activate(object obj)
    {
        RandomPosition();
        
        owner.uiControl.Display($"{data.Flag}");
    }

    public override void Deactivate()
    {
        owner.Stop();
    }

    public override void Update()
    {
        elapsed += Time.deltaTime;

        if(elapsed > data.wandorStay){
            RandomPosition();
            elapsed = 0f;
        }

        MoveAnimation();
    }

    public override void FixedUpdate()
    {
        if(owner == null || owner.rb == null) return;

        FollowPath();
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

        //이동
        //linearVelocity : Vector + Scalar
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

    void SetDestination(Vector3 destination){
        if(!NavMesh.CalculatePath(owner.transform.position, destination, -1, path))
            return;

        corners = path.corners;
        next = 1;
        owner.isArrived = false;
    }


    private void RandomPosition(){
        // 이동할 랜덤 위치 선정.
        // [-1  ~  1]
        
        // 가는 중이다.
        Vector3 randomPos = owner.transform.position + Random.insideUnitSphere * data.wandorRadius;
        randomPos.y = 1f;
    
        SetDestination(randomPos);
    }

    private void MoveAnimation(){
        float a = owner.isArrived ? 0 : Mathf.Clamp01(currentVelocity / data.movePerSec);
        float spd = Mathf.Lerp(owner.animator.GetFloat(AnimationClipHashSet._MOVESPEED), a, Time.deltaTime * 10f);
        owner.animator.SetFloat(AnimationClipHashSet._MOVESPEED, spd);
    }
}