using CustomInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent/EventAttackBefore")]
public class EventAttackBefore : GameEvent<EventAttackBefore>
{
    public override EventAttackBefore Item => this;
    [ReadOnly] public CharacterControl from;
    [ReadOnly] public CharacterControl to;

    [ReadOnly] public int damage;
    
    public PoolableParticle particleHit;
    public PoolableFeedback feedbackFloatingText;   
}