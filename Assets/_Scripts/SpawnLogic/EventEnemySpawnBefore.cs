using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent/EventEnemySpawnBefore")]
public class EventEnemySpawnBefore : GameEvent<EventEnemySpawnBefore>
{
    public override EventEnemySpawnBefore Item => this;
    
    [Space(20)]
    public CharacterControl enemyCharacter;
}