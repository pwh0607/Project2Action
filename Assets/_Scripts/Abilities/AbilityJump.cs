using UnityEngine;

public class AbilityJump : Ability<AbilityJumpData>
{
    private bool jumping = false;
    public AbilityJump(AbilityJumpData data, CharacterControl owner) : base(data, owner) { }

    public override void Activate()
    {
        if(owner.cc.isGrounded || owner.cc == null) return;
        jumping = true;
    }

    public override void DeActivate()
    {
        jumping = false;
        elapsedTime = 0f;
    }

    float elapsedTime;
    public override void Update()
    {
        if(owner.cc == null || !jumping) return;
        elapsedTime += Time.deltaTime;

        float t = elapsedTime / data.jumpDuration;
        float height = data.jumpCurve.Evaluate(t) * data.jumpForce;
        owner.cc.Move(Vector3.up * height * Time.deltaTime);
        
        if(elapsedTime >= data.jumpDuration || (elapsedTime > 0.1f && owner.cc.isGrounded)){
            jumping = false;
            elapsedTime = 0.0f;
        }
    }
}