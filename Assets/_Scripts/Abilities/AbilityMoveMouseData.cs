using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Move Mouse")]
public class AbilityMoveMouseData : AbilityData
{
    public float movePerSec = 5f;
    public float rotatePerSec = 1080f;
     
    public override AbilityFlag Flag => AbilityFlag.Move;
    public override Ability CreateAbility(CharacterControl owner) => new AbilityMoveMouse(this, owner);
}