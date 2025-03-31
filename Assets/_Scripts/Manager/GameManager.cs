using Unity.AI.Navigation;
using UnityEngine.Events;
using UnityEngine.Rendering;

// 관리, 이벤트 송출.
public class GameManager : BehaviourSingleton<GameManager>
{
    protected override bool IsDontDestroy() => true;

// Events
    public UnityAction<Ability> eventAbilityAdded;
    public UnityAction<Ability> eventAbilityRemoved;
    public UnityAction<Ability> eventAbilityUsed;

// Trigger

    public void TriggerAbilityAdd(Ability a){
        eventAbilityAdded?.Invoke(a);
    }
}