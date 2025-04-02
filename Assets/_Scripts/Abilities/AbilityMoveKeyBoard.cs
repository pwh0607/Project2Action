using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityMoveKeyBoard : Ability<AbilityMoveKeyBoardData>
{
    float horz, vert;
    private Transform cameraTransform;
    private Vector3 camForward, camRight;
    private Vector3 direction;
    private float _velocity;
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

    public override void Activate()
    {
        owner.actionInput.Player.Move.performed += InputMove;
        owner.actionInput.Player.Move.performed += InputStop;
    }

    public override void Deactivate()
    {
        owner.actionInput.Player.Move.performed -= InputMove;
        owner.actionInput.Player.Move.performed -= InputStop;
    }

    void InputMove(InputAction.CallbackContext context)
    {
        owner.isArrived = !context.performed;
        
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
        owner.isArrived = context.canceled;
        Stop();
    }

    void Stop(){
        if(owner.isArrived) {
            direction = Vector3.zero;
            owner.rb.linearVelocity = Vector3.zero;
            owner.animator?.SetFloat(owner._MOVESPEED, 0);
        }
    }

    void Movement()
    {
        Vector3 movement = direction * data.movePerSec *150f * Time.deltaTime;
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