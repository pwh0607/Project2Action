using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Jump")]
public class AbilityPlayerJumpData : AbilityData
{
    public override AbilityFlag Flag => AbilityFlag.Jump;
    

    public override Ability CreateAbility(CharacterControl owner) => new AbilityPlayerJump(this, owner);

    public float jumpForce = 10f;

    public float jumpDuration = 1f;
    public AnimationCurve jumpCurve;
}
