using Project2Action;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityJump : Ability<AbilityJumpData>
{
    float elapsedTime = 0f;
    public AbilityJump(AbilityJumpData data, CharacterControl owner) : base(data, owner) { }

    public override void Activate()
    {
        owner.actionInput.Player.Jump.performed += InputJump;
    }

    public override void Deactivate()
    {
        owner.actionInput.Player.Jump.performed -= InputJump;
    }

    public override void FixedUpdate()
    {
        if(owner.rb == null || !owner.isJumping) return;
        elapsedTime += Time.deltaTime;

        float t = Mathf.Clamp01(elapsedTime / data.jumpDuration);
        Vector3 velocity = owner.rb.linearVelocity;
        velocity.y = data.jumpCurve.Evaluate(t) * data.jumpForce;
        owner.rb.linearVelocity = velocity;

        if(t >= 0.3f && owner.isGrounded)
            JumpDown();
    }

    private void JumpUp(){
        if(owner.rb == null || !owner.isGrounded) return;
        owner.isJumping = true;
        elapsedTime = 0;
        owner.PlayeAnimation(owner._JUMPUP, 0.1f, 0);
    }

    private void JumpDown(){
        owner.isJumping = false;
        owner.PlayeAnimation(owner._JUMPDOWN, 0.02f, 0);
    }

    private void InputJump(InputAction.CallbackContext context){
        if(context.performed) JumpUp();
    }
}