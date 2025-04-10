using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent/EventSensorSightEnter")]
public class EventSensorSightEnter : GameEvent<EventSensorSightEnter>
{
    public override EventSensorSightEnter Item => this;
    
    // From(목격자)
    public CharacterControl from;
    // To(목격 대상)
    public CharacterControl to;
}