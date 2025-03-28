using UnityEngine.Events;

public class GameEvent : BehaviourSingleton<GameEvent>
{
    protected override bool IsDontDestroy()=> true;

#region event 선언
    public UnityAction eventCameraEvent;
#endregion

#region event 호출
    public void TriggerCameraEvent() => eventCameraEvent?.Invoke();
#endregion
}