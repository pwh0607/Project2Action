using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityJump : Ability<AbilityJumpData>
{
    private bool jumping = false;
    float elapsedTime = 0f;
    public AbilityJump(AbilityJumpData data, CharacterControl owner) : base(data, owner) { }

    public override void Activate(InputAction.CallbackContext context)
    {
        if(!owner.isGrounded || owner.rb == null) return;
        jumping = true;
        elapsedTime = 0;
        /*
            CrossFade
            CrossFadeInFixedTime() : 현실 시간 기반.
           
            // layer : animator 상의 layer : baseLayer = 0 (int)
            // offset 
        */
        Debug.Log("Jump Up!");
        owner.animator?.CrossFadeInFixedTime("JUMPUP", 0.1f, 0, 0f);
    }

    public override void Deactivate()
    {
        jumping = false;
        Debug.Log("Jump Down!");
        owner.animator?.CrossFadeInFixedTime("JUMPDOWN", 0.02f, 0, 0f);
    }

    public override void FixedUpdate()
    {
        if(owner.rb == null || !jumping) return;
        elapsedTime += Time.deltaTime;

        float t = Mathf.Clamp01(elapsedTime / data.jumpDuration);
        Vector3 velocity = owner.rb.linearVelocity;
        velocity.y = data.jumpCurve.Evaluate(t) * data.jumpForce;
        owner.rb.linearVelocity = velocity;

        if(t >= 0.3f && owner.isGrounded){
            Deactivate();
        }
    }
}