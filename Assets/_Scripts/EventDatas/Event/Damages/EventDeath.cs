using CustomInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent/EventDeath")]
public class EventDeath : GameEvent<EventDeath>
{
    public override EventDeath Item => this;
    
    // 사망한 캐릭터.
    [ReadOnly] public CharacterControl target;
}