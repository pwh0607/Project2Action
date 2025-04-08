using CustomInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Trace")]
public class AbilityTraceData : AbilityData
{
    public override AbilityFlag Flag => AbilityFlag.Trace;
    [ReadOnly] public float movePerSec = 5f;
    [ReadOnly] public float rotatePerSec = 1080f;
    public float stopDistance;
    public override Ability CreateAbility(CharacterControl owner) => new AbilityTrace(this, owner);

    [Tooltip("추격 대상")]
    public Transform traceTarget;
}