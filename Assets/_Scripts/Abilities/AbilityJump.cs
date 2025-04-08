using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityJump : Ability<AbilityJumpData>
{
    private bool isJumping = false;
    float elapsedTime = 0f;
    private CharacterControl control;
    public AbilityJump(AbilityJumpData data, IActorControl owner) : base(data, owner) {
        if(owner.Profile == null) return;
        
        control = owner as CharacterControl;

        data.jumpForce = owner.Profile.jumpForce;
        data.jumpDuration = owner.Profile.jumpDuration;
    }

    public override void Activate()
    {
        control.actionInput.Player.Jump.performed += InputJump;
    }

    public override void Deactivate()
    {
        control.actionInput.Player.Jump.performed -= InputJump;
    }

    public override void FixedUpdate()
    {
        if(control.rb == null || !isJumping) return;
        elapsedTime += Time.deltaTime;

        float t = Mathf.Clamp01(elapsedTime / data.jumpDuration);
        
        Vector3 velocity = control.rb.linearVelocity;
        velocity.y = data.jumpCurve.Evaluate(t) * data.jumpForce;
        control.rb.linearVelocity = velocity;

        if(t >= 0.3f && control.isGrounded)
            JumpDown();
    }

    private void JumpUp(){
        if(control.rb == null || control.isGrounded) return;
        isJumping = true;
        elapsedTime = 0;
        control.PlayeAnimation(control._JUMPUP, 0.1f);
    }

    private void JumpDown(){
        isJumping = false;
        control.PlayeAnimation(control._JUMPDOWN, 0.02f);
    }

    private void InputJump(InputAction.CallbackContext context){
        if(context.performed) JumpUp();
    }
}