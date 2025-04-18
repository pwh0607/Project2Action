using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class AbilityPlayerMoveMouse : Ability<AbilityPlayerMoveMouseData>
{
    private Camera camera;
    private NavMeshPath path;
    private Vector3[] corners;
    private int next;
    private ParticleSystem marker;
    float currentVelocity;

    private CursorControl cursor;

    public AbilityPlayerMoveMouse(AbilityPlayerMoveMouseData data, CharacterControl owner) : base(data, owner)
    {
        camera = Camera.main;
        cursor = GameObject.FindFirstObjectByType<CursorControl>();
        if(cursor == null)
            Debug.LogWarning("AbilityMoveMouse ] CursorControl is null...");

        path = new();

        marker = GameObject.Instantiate(data.marker);
        
        if(marker == null)
            Debug.LogWarning("Marker is not existed!");
        
        marker.gameObject.SetActive(false);

        owner.isArrived = true;

        if(owner.Profile == null) return;
        data.movePerSec = owner.Profile.moveSpeed;
        data.rotatePerSec = owner.Profile.rotateSpeed;
    }

    public override void Update(){
        if (owner == null || owner.rb == null)
            return;
        
        RotateByCursor();
        MoveAnimation();
    }

    // 물리 연산만!
    public override void FixedUpdate()
    {
        if(owner == null || owner.rb == null) return;
        
        FollowPath();
    }

    void SetDestiNation(Vector3 destination){
        if(!NavMesh.CalculatePath(owner.transform.position, destination, -1, path)) return;

        corners = path.corners;
        next = 1;
        owner.isArrived = false;
    }
    
    Quaternion lookrot;
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

    public override void Activate(object obj){
        if(owner.TryGetComponent<InputControl>(out var input)){
            input.actionInput.Player.MoveMouse.performed += InputMove;
        }
    }

    public override void Deactivate(){
        if(owner.TryGetComponent<InputControl>(out var input)){
            input.actionInput.Player.Move.canceled -= InputMove;
        }
    }

    private void MoveAnimation(){
        float a = owner.isArrived ? 0 : Mathf.Clamp01(currentVelocity / data.movePerSec);
        float spd = Mathf.Lerp(owner.animator.GetFloat("MOVESPEED"), a, Time.deltaTime * 10f);
        owner.animator.SetFloat("MOVESPEED", spd);
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

    void RotateByCursor(){
        if(cursor == null) return;

        Vector3 cursorPoint = cursor.CursorFixedPoint.position;

        // 커서와 캐릭터의 y 높이를 같게 처리하기.
        cursorPoint.y = owner.transform.position.y;
        Vector3 direction = cursorPoint - owner.transform.position;

        // 바라볼 방향으로 회전
        Quaternion rot =Quaternion.LookRotation(direction);
        owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, rot, Time.deltaTime * 10f);           //RotateTowards : 회전 보정
    }

}