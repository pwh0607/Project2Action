using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent/EventSensorItemEnter")]
public class EventSensorItemEnter : GameEvent<EventSensorItemEnter>
{
    public override EventSensorItemEnter Item => this;
    
    // From(목격자)
    public CharacterControl from;
    // To(목격 대상)
    public GameObject to;
}