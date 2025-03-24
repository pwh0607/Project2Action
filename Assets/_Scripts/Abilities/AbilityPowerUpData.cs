using UnityEngine;

public class AbilityPowerUpData : AbilityData
{
    public override AbilityFlag Flag => AbilityFlag.Jump;
    
    public float jumpForce = 10f;

    public override Ability CreateAbility(CharacterControl owner)
    {
        return new AbilityJump(owner, jumpForce);
    }
}
