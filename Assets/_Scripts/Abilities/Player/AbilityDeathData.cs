using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Death")]
public class AbilityDeathData : AbilityData
{
    public EventDeath eventDeath;
    
    public override AbilityFlag Flag => AbilityFlag.Death;
    public override Ability CreateAbility(CharacterControl owner) => new AbilityDeath(this, owner);
}