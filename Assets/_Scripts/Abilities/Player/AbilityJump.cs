using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityJump : Ability<AbilityJumpData>
{
    private bool isJumping = false;
    float elapsedTime = 0f;
    public AbilityJump(AbilityJumpData data, CharacterControl owner) : base(data, owner) {
        if(owner.Profile == null) return;
        
        data.jumpForce = owner.Profile.jumpForce;
        data.jumpDuration = owner.Profile.jumpDuration;
    }

    public override void Activate()
    {        
        if(!owner.TryGetComponent<InputControl>(out var input)) return;
        input.actionInput.Player.Jump.performed += InputJump;
    }

    public override void Deactivate()
    {
        if(!owner.TryGetComponent<InputControl>(out var input)) return;
        input.actionInput.Player.Jump.performed -= InputJump;
    }

    public override void FixedUpdate()
    {
        if(owner.rb == null || !isJumping) return;
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
        isJumping = true;
        elapsedTime = 0;
        owner.PlayeAnimation(AnimationClipHashSet._JUMPUP, 0.1f);
    }

    private void JumpDown(){
        isJumping = false;
        owner.PlayeAnimation(AnimationClipHashSet._JUMPDOWN, 0.02f);
    }

    private void InputJump(InputAction.CallbackContext context){
        if(context.performed) JumpUp();
    }
}