using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Move Mouse")]
public class AbilityMoveMouseData : AbilityData
{
    public float movePerSec = 5f;
    public float rotatePerSec = 1080f;
    public ForceMode forceMode;
    public float stopDistance;
    public float stopOffset;
    public override AbilityFlag Flag => AbilityFlag.Move;
    public override Ability CreateAbility(CharacterControl owner) => new AbilityMoveMouse(this, owner);

    [Space(10)]
    public GameObject marker;           //3d 피킹 마커 오브젝트.
}