using UnityEngine;

public class AbilityJump : Ability<AbilityJumpData>
{
    private bool jumping = false;
    float elapsedTime = 0f;
    public AbilityJump(AbilityJumpData data, CharacterControl owner) : base(data, owner) { }

    public override void Activate()
    {
        if(!owner.isGrounded || owner.rb == null) return;
        jumping = true;
        elapsedTime = 0;

        Debug.Log("Jump up!");
        /*
            CrossFade
            CrossFadeInFixedTime() : 현실 시간 기반.
           
            // layer : animator 상의 layer : baseLayer = 0 (int)
            // offset 
        */
        owner.animator?.CrossFadeInFixedTime("JUMPUP", 0.1f, 0, 0f);
    }

    public override void Deactivate()
    {
        jumping = false;
        
        Debug.Log("Jump down!");
        owner.animator?.CrossFadeInFixedTime("JUMPDOWN", 0.1f, 0, 0f);
    }

    public override void FixedUpdate()
    {
        if(owner.rb == null || !jumping) return;
        elapsedTime += Time.deltaTime;

        float t = elapsedTime / data.jumpDuration;
        
        // *500f? movePerSec와 LinearVelocity 값을 동기화 하기 위한 상수.
        float height = data.jumpCurve.Evaluate(t) * data.jumpForce * 100f;
        Vector3 velocity = owner.rb.linearVelocity;

        velocity.y = height * Time.deltaTime;
        owner.rb.linearVelocity = velocity;
        
        //owner.isLanding
        if(elapsedTime >= data.jumpDuration || (elapsedTime > 0.01f && owner.isGrounded)){
            jumping = false;
            elapsedTime = 0.0f;
            Deactivate();
        }
    }
}