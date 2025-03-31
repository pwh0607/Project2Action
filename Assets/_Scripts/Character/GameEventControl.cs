using UnityEngine;

public class GameEventControl : MonoBehaviour
{
    [SerializeField] GameEventCameraSwitch eventCameraSwitch;
    private CharacterControl cc;

    void Start()
    {
        if(!TryGetComponent(out cc)) Debug.LogWarning("GameEventControl - CharacterControl 없음...");
    }

    void OnEnable()
    {
        eventCameraSwitch.Register(OnEventCameraSwitch);
    }

    void OnDisable()
    {
        eventCameraSwitch.UnRegister(OnEventCameraSwitch);
    }

    void OnEventCameraSwitch(GameEventCameraSwitch e){
        if(e.inout){
            cc.ability.Deactivate(AbilityFlag.MoveKeyboard);
        }
        else{
            cc.ability.Activate(AbilityFlag.MoveKeyboard);
        }
    }
}
