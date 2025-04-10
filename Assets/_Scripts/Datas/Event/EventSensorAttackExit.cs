using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent/EventSensorAttackExit")]
public class EventSensorAttackExit : GameEvent<EventSensorAttackExit>
{
    public override EventSensorAttackExit Item => this;
    
    // From(목격자)
    public CharacterControl from;
    // To(목격 대상)
    public CharacterControl to;
}