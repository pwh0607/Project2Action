using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class AbilityMoveMouse : Ability<AbilityMoveMouseData>
{
    private Camera camera;
    private NavMeshPath path;
    private Vector3[] corners;
    private int next;
    private ParticleSystem marker;


    public AbilityMoveMouse(AbilityMoveMouseData data, CharacterControl owner) : base(data, owner)
    {
        camera = Camera.main;
        path = new();
        owner.isArrived = true;

        marker = GameObject.Instantiate(data.marker);
        
        if(marker == null){
            Debug.LogWarning("Marker is not existed!");
        }
        marker.gameObject.SetActive(false);
    }
    public override void Update(){
        if ( owner == null || owner.rb == null)
            return;

        MoveAnimation();
    }

    // 물리 연산만!
    public override void FixedUpdate()
    {
        if(owner == null || owner.rb == null) return;
        
        FollowPath();
    }

    void SetDestiNation(Vector3 destination){
        if(!NavMesh.CalculatePath(owner.transform.position, destination, -1, path)){
            Debug.Log($"길 못찾음.");
        }
        corners = path.corners;
        next = 1;
        owner.isArrived = false;
    }
    
    Quaternion lookrot;
    float currentVelocity;
    private void FollowPath(){
        if(corners == null || corners.Length <= 0 || owner.isArrived == true) return;

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

    public override void Activate(){
        owner.actionInput.Player.Enable();
        owner.actionInput.Player.MoveMouse.performed += InputMove;
    }

    public override void Deactivate()
    {
        owner.actionInput.Player.Move.canceled -= InputMove;
        owner.actionInput.Player.Disable();
    }

    private void MoveAnimation(){
        float a = owner.isArrived? 0 : Mathf.Clamp01(currentVelocity / data.movePerSec);
        float spd = Mathf.Lerp(owner.animator.GetFloat(owner._MOVESPEED), a, Time.deltaTime * 10f);
        owner.animator.SetFloat(owner._MOVESPEED, spd);
    }

    void InputMove(InputAction.CallbackContext context){
        Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(Physics.Raycast(ray, out var hit)){
            marker.gameObject.SetActive(true);
            marker.transform.position = hit.point + Vector3.up * 0.1f;
            marker.Play();
            SetDestiNation(hit.point);
        }
    }
}