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
        // 0 : Left, 1 : Right, 2 : Wheel
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
        Vector3 movement =  direction * data.movePerSec * Time.deltaTime;

        owner.rb.linearVelocity = movement;
        
        if(Vector3.Distance(nextTarget, owner.rb.position) <= data.stopDistance){
            next++;
            if(next >= corners.Length){
                isArrived = true;
                owner.rb.linearVelocity = Vector3.zero;
            }
        }

        //최종 위치 확인.
        if(Vector3.Distance(nextTarget, owner.rb.position) <= data.stopDistance + data.stopOffset){
            owner.animator?.CrossFadeInFixedTime("RUNTOSTOP", 0.1f, 0, 0f);
        }
    }

    private void MoveAnimation(){
        float a = isArrived? 0 : data.movePerSec;
        float spd = Mathf.Lerp(owner.animator.GetFloat("moveSpeed"), a, Time.deltaTime * 10f);
        owner.animator.SetFloat("moveSpeed", spd);
    }
}