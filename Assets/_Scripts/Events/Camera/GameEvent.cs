using UnityEngine;
using UnityEngine.Events;

public abstract class GameEvent<T> : ScriptableObject where T : GameEvent<T>
{
    public abstract T Item{get;}

    //UnityAction은 Action에서 파생된 Unity 전용 Delegate(대리자)
    public UnityAction<T> OnEventRaised;

    public void Raise(){
        Debug.Log($"이벤트 발동 : {Item.name}");
        OnEventRaised?.Invoke(Item);
    }
    
    public void Register(UnityAction<T> listener){
        Debug.Log("Listener Register");
        OnEventRaised += listener;    
    }
    
    public void UnRegister(UnityAction<T> listener){
        Debug.Log("Listener Unregister");
        OnEventRaised -= listener;    
    }

    public void Clear(UnityAction<T> listener){
        OnEventRaised = null;    
    }
}