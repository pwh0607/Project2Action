using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityMoveKeyBoard : Ability<AbilityMoveKeyBoardData>
{
    float horz, vert;
    private Transform cameraTransform;
    private Vector3 camForward, camRight;
    private Vector3 direction;
    private float _velocity;
    private InputAction.CallbackContext context;
    public AbilityMoveKeyBoard(AbilityMoveKeyBoardData data, CharacterControl owner) : base(data, owner)
    {
        cameraTransform = Camera.main.transform; 
        _velocity = data.rotatePerSec;   
    }
    
    public override void FixedUpdate()
    {
        Rotate();
        Movement();
    }

    public override void Activate(InputAction.CallbackContext context)
    {
        this.context= context;
        owner.isArrived = context.canceled;
        // context가 canceled...? => 키 up => 도착했다.
        InputKeyboard(context);
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }

    void InputKeyboard(InputAction.CallbackContext context)
    {
        owner.isArrived = !context.performed;
        if(context.canceled)            
            owner.isArrived = true;

        var axis = context.ReadValue<Vector2>();
        camForward = cameraTransform.forward;
        camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        direction = (camForward * axis.y + camRight * axis.x).normalized;
    }

    void Movement()
    {
        // *50f? movePerSec와 LinearVelocity 값을 동기화 하기 위한 상수.
        Vector3 movement = direction * data.movePerSec * 50f * Time.deltaTime;
        Vector3 velocity = new Vector3(movement.x, owner.rb.linearVelocity.y, movement.z);

        owner.rb.linearVelocity = velocity;
        
        if(!owner.isGrounded) return;
        
        float v = Vector3.Distance(Vector3.zero, owner.rb.linearVelocity);
        float targetSpeed = Mathf.Clamp01(v / data.movePerSec);
        float moveSpeed = Mathf.Lerp(owner.animator.GetFloat(owner._MOVESPEED), targetSpeed, Time.deltaTime * 30f);
        
        owner.animator?.SetFloat(owner._MOVESPEED, moveSpeed);
    }

    void Rotate()
    {
        if (direction == Vector3.zero) return;
        
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float smoothAngle = Mathf.SmoothDampAngle(owner.transform.eulerAngles.y, angle, ref _velocity, 0.1f);
        owner.transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }
}