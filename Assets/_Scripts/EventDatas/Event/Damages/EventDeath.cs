using CustomInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent/EventDeath")]
public class EventDeath : GameEvent<EventDeath>
{
    public override EventDeath Item => this;

    [ReadOnly] public CharacterControl target;
}