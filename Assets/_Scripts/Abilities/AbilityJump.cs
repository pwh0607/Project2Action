using UnityEngine;

public class AbilityJump : Ability<AbilityJumpData>
{
    private bool jumping = false;
    public AbilityJump(AbilityJumpData data, CharacterControl owner) : base(data, owner) { }

    public override void Activate()
    {
        if(!owner.cc.isGrounded || owner.cc == null) return;
        jumping = true;
        elapsedTime = 0;
        owner.animator?.SetTrigger("JumpUp");
    }

    public override void Deactivate()
    {
        jumping = false;
        elapsedTime = 0;
        owner.animator?.SetTrigger("JumpDown");
    }

    float elapsedTime = 0f;
    public override void FixedUpdate()
    {
        Debug.Log($"JumpState : {jumping}");
        if(owner.cc == null || !jumping) return;

        elapsedTime += Time.deltaTime;

        float t = elapsedTime / data.jumpDuration;
        float height = data.jumpCurve.Evaluate(t) * data.jumpForce;
        owner.cc.Move(Vector3.up * height * Time.deltaTime);
        
        if(elapsedTime > data.jumpDuration || (elapsedTime > 0.1f && owner.cc.isGrounded)){
            jumping = false;
            elapsedTime = 0.0f;
        }
    }
}