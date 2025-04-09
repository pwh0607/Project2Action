using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent/EventSensorTargetEnter")]
public class EventSensorTargetEnter : GameEvent<EventSensorTargetEnter>
{
    public override EventSensorTargetEnter Item => this;
    
    // From(목격자)
    public CharacterControl from;
    // To(목격 대상)
    public CharacterControl to;
}