using UnityEngine;
using UnityEngine.AI;

public class AbilityMoveMouse : Ability<AbilityMoveMouseData>
{
    private Camera camera;
    private NavMeshPath path;
    private Vector3[] corners;
    private int next;
    private bool isArrived = true;

    public AbilityMoveMouse(AbilityMoveMouseData data, CharacterControl owner) : base(data, owner)
    {
        camera = Camera.main;
        path = new();
    }

    public override void Update(){
        if(owner == null || owner.rb == null) return;
        
        InputMouse();
        MoveAnimation();
    }

    // 물리 연산만!
    public override void FixedUpdate()
    {
        if(owner == null || owner.rb == null) return;
        FollowPath();
    }

    private void InputMouse(){
        if(Input.GetMouseButtonDown(1)){
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out var hit))
                SetDestiNation(hit.point);
        }
    }

    void SetDestiNation(Vector3 destination){
        if(NavMesh.CalculatePath(owner.transform.position, destination, -1, path) == false) return;
        corners = path.corners;
        next = 1;
        isArrived = false;
    }
    
    Quaternion lookrot;
    float currentVelocity;
    private void FollowPath(){
        if(corners == null || corners.Length <= 0 || isArrived == true) return;

        // 다음 위치
        Vector3 nextTarget = corners[next];   
        // 최종 위치.
        Vector3 finalTarget = corners[corners.Length-1];
        
        // 다음 위치 방향.
        Vector3 direction = (nextTarget - owner.rb.transform.position).normalized;
        direction.y = 0;
        
        // 회전
        if(direction != Vector3.zero){
            lookrot = Quaternion.LookRotation(direction);
        }

        owner.transform.rotation = Quaternion.RotateTowards(owner.transform.rotation, lookrot, data.rotatePerSec * Time.deltaTime);

        //이동
        //linearVelocity : Vector + Scalar
        Vector3 movement =  direction * data.movePerSec * 50f * Time.deltaTime;
        owner.rb.linearVelocity = movement;
        currentVelocity = Vector3.Distance(Vector3.zero, owner.rb.linearVelocity);
        
        if(Vector3.Distance(nextTarget, owner.rb.position) <= data.stopDistance){
            next++;
            if(next >= corners.Length){
                isArrived = true;
                owner.rb.linearVelocity = Vector3.zero;
            }
        }

        if(Vector3.Distance(nextTarget, owner.rb.position) <= data.stopDistance + data.stopOffset){
            owner.animator?.CrossFadeInFixedTime("RUNTOSTOP", 0.1f, 0, 0f);
        }
    }

    private void MoveAnimation(){
        float a = isArrived? 0 : Mathf.Clamp01(currentVelocity / data.movePerSec);
        float spd = Mathf.Lerp(owner.animator.GetFloat("moveSpeed"), a, Time.deltaTime * 10f);
        owner.animator.SetFloat("moveSpeed", spd);
    }
}