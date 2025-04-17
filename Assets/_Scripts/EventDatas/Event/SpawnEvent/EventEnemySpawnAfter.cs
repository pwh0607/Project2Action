using CustomInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent/EventEnemySpawnAfter")]
public class EventEnemySpawnAfter : GameEvent<EventEnemySpawnAfter>
{
    public override EventEnemySpawnAfter Item => this;

    [ReadOnly] public CharacterControl character;
    [ReadOnly] public Transform eyePoint;

    [Tooltip("플레이어 스폰 파티클")]
    public PoolBehaviour spawnParticle;
}