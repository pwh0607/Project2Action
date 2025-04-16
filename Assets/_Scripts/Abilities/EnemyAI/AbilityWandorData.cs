using CustomInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Wandor")]
public class AbilityWandorData : AbilityData
{
    public override AbilityFlag Flag => AbilityFlag.Wandor;
    [ReadOnly] public float movePerSec = 5f;
    [ReadOnly] public float rotatePerSec = 1080f;
    public float stopDistance;
    public override Ability CreateAbility(CharacterControl owner) => new AbilityWandor(this, owner);

    [Tooltip("배회할 범위")]
    public float wandorRadius = 5f;

    [Tooltip("도착 후 대기 시간")]
    public float wandorStay = 2f;
}