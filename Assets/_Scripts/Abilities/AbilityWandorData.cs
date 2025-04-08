using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Wandor")]
public class AbilityWandorData : AbilityData
{
    public override AbilityFlag Flag => AbilityFlag.Wandor;
    
    public override Ability CreateAbility(IActorControl owner) => new AbilityWandor(this, owner);

    public float jumpForce = 10f;

    public float jumpDuration = 1f;
    public AnimationCurve jumpCurve;
}