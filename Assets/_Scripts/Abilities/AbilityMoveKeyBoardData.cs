using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Move KeyBoard")]
public class AbilityMoveKeyBoardData : AbilityData
{
    public float movePerSec = 10f;
    public float rotatePerSec = 5f;
    public override AbilityFlag Flag => AbilityFlag.Move;
    public override Ability CreateAbility(CharacterControl owner) => new AbilityMoveKeyBoard(this, owner);
}