using UnityEngine;
using CustomInspector;

[CreateAssetMenu(menuName = "Abilities/Move KeyBoard")]
public class AbilityMoveKeyBoardData : AbilityData
{
    [ReadOnly] public float movePerSec = 10f;
    [ReadOnly] public float rotatePerSec = 5f;
    public override AbilityFlag Flag => AbilityFlag.MoveKeyboard;
    public override Ability CreateAbility(IActorControl owner) => new AbilityMoveKeyBoard(this, owner);
}