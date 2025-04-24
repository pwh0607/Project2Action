using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent/EventSensorItemExit")]
public class EventSensorItemExit : GameEvent<EventSensorItemExit>
{
    public override EventSensorItemExit Item => this;
    
    // From(목격자)
    public CharacterControl from;
    // To(목격 대상)
    public GameObject to;
}