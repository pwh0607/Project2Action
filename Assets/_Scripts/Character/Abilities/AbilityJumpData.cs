using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Jump")]
public class AbilityJumpData : AbilityData
{
    public override AbilityFlag Flag => AbilityFlag.Jump;
    
    public float jumpForce = 10f;

    public override Ability CreateAbility(CharacterControl owner)
    {
        return new AbilityJump(owner, jumpForce);
    }
}