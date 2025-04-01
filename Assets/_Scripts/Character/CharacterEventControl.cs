using UnityEngine;
using Cysharp.Threading.Tasks;

public class CharacterEventControl : MonoBehaviour
{
    #region Event
    [SerializeField] EventCameraSwitch eventCameraSwitch;
    [SerializeField] EventPlayerSpawnAfter eventPlayerSpawnAfter;
    #endregion
    
    private CharacterControl character;
    void Start()
    {
        if(!TryGetComponent(out character)) Debug.LogWarning("GameEventControl - CharacterControl 없음...");
    }

    void OnEnable()
    {

        eventPlayerSpawnAfter.Register(OnEventPlayerSpawnAfter);
        eventCameraSwitch.Register(OnEventCameraSwitch);
    }

    void OnDisable()
    {

        eventPlayerSpawnAfter.UnRegister(OnEventPlayerSpawnAfter);
        eventCameraSwitch.UnRegister(OnEventCameraSwitch);
    }


    void OnEventCameraSwitch(EventCameraSwitch e){
        if(e.inout){
            character.ability.Deactivate(AbilityFlag.MoveKeyboard);
        }
        else{
            character.ability.Activate(AbilityFlag.MoveKeyboard);
        }
    }

    void OnEventPlayerSpawnAfter(EventPlayerSpawnAfter e){
        GameManager.I.DelayCallAsync(1000, ()=> character.Visible(true)).Forget();             // 비동기 호출 : 동기호출.Forget();
    }
}

// 비동기(async)
/*
    1. 코루틴   
    2. Invoke
    3. async / await
    4. Awaitable
    5. CySharp - Unitask
    6. DoTween - DoVirtual.Delay(3f, () => {...})
*/
// 유니티는 단일 스레드 엔진이다.
// => 비동기라고 표현하지만 사실방 코어가 빠르게 번갈아가며 처리하는 것! [병렬 수행하는 것처럼 보이게 하는 것.]