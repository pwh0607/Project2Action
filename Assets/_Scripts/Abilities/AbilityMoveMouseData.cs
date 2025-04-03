using CustomInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Move Mouse")]
public class AbilityMoveMouseData : AbilityData
{
    public float movePerSec = 5f;
    public float rotatePerSec = 1080f;
    public ForceMode forceMode;
    public float stopDistance;

    [Tooltip("min : runtoStop 모션 발동 지점, max : runtoStop 무시 지점")]
    [AsRange(0, 15)] public Vector2 runToStopDistance;

    public override AbilityFlag Flag => AbilityFlag.MoveMouse;
    public override Ability CreateAbility(CharacterControl owner) => new AbilityMoveMouse(this, owner);

    [Space(10)]
    public ParticleSystem marker;           //3d 피킹 마커 오브젝트.
}