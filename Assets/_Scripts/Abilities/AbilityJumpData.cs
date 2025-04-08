using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Jump")]
public class AbilityJumpData : AbilityData
{
    public override AbilityFlag Flag => AbilityFlag.Jump;
    

    public override Ability CreateAbility(IActorControl owner) => new AbilityJump(this, owner);

    public float jumpForce = 10f;

    public float jumpDuration = 1f;
    public AnimationCurve jumpCurve;
}