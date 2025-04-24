using CustomInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent/EventAttackAfter")]
public class EventAttackAfter : GameEvent<EventAttackAfter>
{
    public override EventAttackAfter Item => this;
    [ReadOnly] public CharacterControl from;
    [ReadOnly] public CharacterControl to;
    
    public PoolableParticle particleHit;
    public PoolableFeedback feedbackFloatingText;   
}