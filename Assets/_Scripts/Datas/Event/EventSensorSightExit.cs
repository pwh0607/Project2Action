using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent/EventSensorSightExit")]
public class EventSensorSightExit : GameEvent<EventSensorSightExit>
{
    public override EventSensorSightExit Item => this;
    
    // From(목격자)
    public CharacterControl from;
    // To(목격 대상)
    public CharacterControl to;
}