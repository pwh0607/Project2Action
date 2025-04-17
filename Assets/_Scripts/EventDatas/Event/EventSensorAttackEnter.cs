using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent/EventSensorAttackEnter")]
public class EventSensorAttackEnter : GameEvent<EventSensorAttackEnter>
{
    public override EventSensorAttackEnter Item => this;
    
    // From(목격자)
    public CharacterControl from;
    // To(공격 대상)
    public CharacterControl to;         //target
}