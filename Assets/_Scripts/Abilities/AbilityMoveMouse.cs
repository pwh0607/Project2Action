using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class AbilityMoveMouse : Ability<AbilityMoveMouseData>
{
    private Camera camera;
    private NavMeshPath path;
    private Vector3[] corners;
    private int next;
    private float hitDistance;      //hit-point와 캐릭터 간의 거리
    private ParticleSystem marker;


    public AbilityMoveMouse(AbilityMoveMouseData data, CharacterControl owner) : base(data, owner)
    {
        camera = Camera.main;
        path = new();
        owner.isArrived = true;

        this.marker = GameObject.Instantiate(data.marker).GetComponent<ParticleSystem>();
        if(marker == null){
            Debug.LogWarning("Marker is not existed!");
        }
        marker.gameObject.SetActive(false);
    }

    // 물리 연산만!
    public override void FixedUpdate()
    {
        if(owner == null || owner.rb == null) return;
        // InputMouse();
        MoveAnimation();
        FollowPath();
    }


    void SetDestiNation(Vector3 destination){
        if(NavMesh.CalculatePath(owner.transform.position, destination, -1, path) == false) return;
        corners = path.corners;
        next = 1;
        owner.isArrived = false;
    }
    
    Quaternion lookrot;
    float currentVelocity;
    private void FollowPath(){
        if(corners == null || corners.Length <= 0 || owner.isArrived == true) return;

        Vector3 nextTarget = corners[next];
        Vector3 finalTarget = corners[corners.Length-1];

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

        float d = Vector3.Distance(finalTarget, owner.rb.position);

        if(hitDistance > data.runToStopDistance.x && d <= data.stopDistance + data.runToStopDistance.y){ 
            // owner.animator?.CrossFadeInFixedTime(owner._RUNTOSTOP, 0.1f, 0, 0f);
        }
    }

    public override void Activate(InputAction.CallbackContext context){
        Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(Physics.Raycast(ray, out var hit)){
            marker.gameObject.SetActive(true);
            marker.transform.position = hit.point + Vector3.up * 0.1f;
            marker.Play();

            hitDistance = Vector3.Distance(hit.point, owner.rb.position);
            SetDestiNation(hit.point);
        }
    }

    private void MoveAnimation(){
        float a = owner.isArrived? 0 : Mathf.Clamp01(currentVelocity / data.movePerSec);
        float spd = Mathf.Lerp(owner.animator.GetFloat(owner._MOVESPEED), a, Time.deltaTime * 10f);
        owner.animator.SetFloat(owner._MOVESPEED, spd);
    }
}