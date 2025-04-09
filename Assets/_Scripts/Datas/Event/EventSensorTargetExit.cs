using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent/EventSensorTargetExit")]
public class EventSensorTargetExit : GameEvent<EventSensorTargetExit>
{
    public override EventSensorTargetExit Item => this;
    
    // From(목격자)
    public CharacterControl from;
    // To(목격 대상)
    public CharacterControl to;
}