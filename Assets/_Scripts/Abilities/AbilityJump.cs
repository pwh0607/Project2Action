using UnityEngine;

public class AbilityJump : Ability<AbilityJumpData>
{
    private bool isJumping = false;
    public AbilityJump(AbilityJumpData data, CharacterControl owner) : base(data, owner) { }

    public override void Activate()
    {
        if(owner.characterController.isGrounded || owner.characterController == null) return;
        isJumping = true;
    }

    public override void Deactivate()
    {
        isJumping = false;
        elapsedTime = 0f;
    }

    float elapsedTime;
    public override void Update()
    {
        if(owner.characterController == null || !isJumping) return;
        elapsedTime += Time.deltaTime;        
        float t = elapsedTime / data.jumpDuration;
        float height = data.jumpCurve.Evaluate(t) * data.jumpForce;

        owner.characterController.Move(Vector3.up * height * Time.deltaTime);

        if(elapsedTime >= data.jumpDuration || (elapsedTime > 0.1f && owner.characterController.isGrounded)){
            isJumping = false;
            elapsedTime = 0.0f;
        }
    }
}