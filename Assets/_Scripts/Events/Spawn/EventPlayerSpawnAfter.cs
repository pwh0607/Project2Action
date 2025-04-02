using CustomInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent/EventPlayerSpawnAfter")]
public class EventPlayerSpawnAfter : GameEvent<EventPlayerSpawnAfter>
{
    public override EventPlayerSpawnAfter Item => this;   

    [ReadOnly] public Transform eyePoint, cursorPoint;
}