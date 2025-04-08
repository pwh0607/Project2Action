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
    float currentVelocity;

    private CharacterControl control;
    public AbilityMoveMouse(AbilityMoveMouseData data, IActorControl owner) : base(data, owner)
    {
        camera = Camera.main;
        path = new();

        marker = GameObject.Instantiate(data.marker);
        
        if(marker == null)
            Debug.LogWarning("Marker is not existed!");
        
        marker.gameObject.SetActive(false);

        if(control.Profile == null) return;

        control = owner as CharacterControl;
        control.isArrived = true;

        data.movePerSec = owner.Profile.moveSpeed;
        data.rotatePerSec = owner.Profile.rotateSpeed;
    }

    public override void Update(){
        if (control == null || control.rb == null)
            return;

        MoveAnimation();
    }

    // 물리 연산만!
    public override void FixedUpdate()
    {
        if(control == null || control.rb == null) return;
        
        FollowPath();
    }

    void SetDestiNation(Vector3 destination){
        if(!NavMesh.CalculatePath(control.transform.position, destination, -1, path)) return;

        corners = path.corners;
        next = 1;
        control.isArrived = false;
    }
    
    Quaternion lookrot;
    private void FollowPath(){
        if(corners == null || corners.Length <= 0 || control.isArrived == true) return;

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

    public override void Activate(){
        control.actionInput.Player.Enable();
        control.actionInput.Player.MoveMouse.performed += InputMove;
    }

    public override void Deactivate()
    {
        control.actionInput.Player.Move.canceled -= InputMove;
        control.actionInput.Player.Disable();
    }

    private void MoveAnimation(){
        float a = control.isArrived ? 0 : Mathf.Clamp01(currentVelocity / data.movePerSec);
        float spd = Mathf.Lerp(control.animator.GetFloat(control._MOVESPEED), a, Time.deltaTime * 10f);
        control.animator.SetFloat(control._MOVESPEED, spd);
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