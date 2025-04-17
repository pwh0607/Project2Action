using CustomInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Attack")]
public class AbilityAttackData : AbilityData
{
    public EventAttackBefore eventAttackBefore;
    public EventAttackAfter eventAttackAfter;
    
    public override AbilityFlag Flag => AbilityFlag.Attack;
    public override Ability CreateAbility(CharacterControl owner) => new AbilityAttack(this, owner);

    [ReadOnly] public CharacterControl target;
}