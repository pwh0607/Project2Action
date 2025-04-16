using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/PlayerAttack")]
public class AbilityPlayerAttackData : AbilityData
{
    public override AbilityFlag Flag => AbilityFlag.Attack;
    
    public override Ability CreateAbility(CharacterControl owner) => new AbilityPlayerAttack(this, owner);
}