using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityMoveKeyBoard : Ability<AbilityMoveKeyBoardData>
{
    float horz, vert;
    private Transform cameraTransform;
    private Vector3 camForward, camRight;
    private Vector3 direction;

    private CharacterControl control;

    public AbilityMoveKeyBoard(AbilityMoveKeyBoardData data, IActorControl control) : base(data, control)
    {
        cameraTransform = Camera.main.transform;

        if(control.Profile == null) return;
        control = control as CharacterControl;
        data.movePerSec = control.Profile.moveSpeed;
        data.rotatePerSec = control.Profile.rotateSpeed;
    }
    
    public override void FixedUpdate()
    {
        Rotate();
        Movement();
    }

    public override void Activate()
    {
        control.actionInput.Player.Enable();
        control.actionInput.Player.Move.performed += InputMove;
        control.actionInput.Player.Move.performed += InputStop;
    }

    public override void Deactivate()
    {
        control.actionInput.Player.Move.performed -= InputMove;
        control.actionInput.Player.Move.performed -= InputStop;
        control.actionInput.Player.Disable();
    }

    void InputMove(InputAction.CallbackContext context)
    {
        control.isArrived = !context.performed;
        
        var axis = context.ReadValue<Vector2>();
        
        camForward = cameraTransform.forward;
        camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        direction = (camForward * axis.y + camRight * axis.x).normalized;
    }

    void InputStop(InputAction.CallbackContext context){
        control.isArrived = context.canceled;
        Stop();
    }

    void Stop(){
        if(control.isArrived) {
            direction = Vector3.zero;
            control.rb.linearVelocity = Vector3.zero;
            control.animator?.SetFloat(AnimationClipHashSet._MOVESPEED, 0);
        }
    }

    void Movement()
    {
        Vector3 movement = direction * data.movePerSec *150f * Time.deltaTime;
        Vector3 velocity = new Vector3(movement.x, control.rb.linearVelocity.y, movement.z);

        control.rb.linearVelocity = velocity;
        
        if(!control.isGrounded) return;
        
        float v = Vector3.Distance(Vector3.zero, control.rb.linearVelocity);
        float targetSpeed = Mathf.Clamp01(v / data.movePerSec);
        float moveSpeed = Mathf.Lerp(control.animator.GetFloat(AnimationClipHashSet._MOVESPEED), targetSpeed, Time.deltaTime * 30f);
        
        control.animator?.SetFloat(AnimationClipHashSet._MOVESPEED, moveSpeed);
    }

    void Rotate()
    {
        if (direction == Vector3.zero) return;
        
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float smoothAngle = Mathf.SmoothDampAngle(control.transform.eulerAngles.y, angle, ref data.rotatePerSec, 0.1f);
        control.transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }
}