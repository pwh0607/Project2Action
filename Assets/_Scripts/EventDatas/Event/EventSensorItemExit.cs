using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent/EventSensorItemExit")]
public class EventSensorItemExit : GameEvent<EventSensorItemExit>
{
    public override EventSensorItemExit Item => this;
    public CharacterControl from;
    public GameObject to;
}

