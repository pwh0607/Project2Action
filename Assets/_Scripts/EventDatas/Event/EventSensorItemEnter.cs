using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent/EventSensorItemEnter")]
public class EventSensorItemEnter : GameEvent<EventSensorItemEnter>
{
    public override EventSensorItemEnter Item => this;
    public CharacterControl from;
    public GameObject to;
}

