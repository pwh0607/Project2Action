using System.Collections;
using NodeCanvas.Tasks.Actions;
using UnityEngine;

public class EnemyEventControl : MonoBehaviour
{
#region Events
    [SerializeField] EventEnemySpawnAfter eventEnemySpawnAfter;
    [SerializeField] EventSensorTargetEnter eventSensorTargetEnter;
    [SerializeField] EventSensorTargetExit eventSensorTargetExit;

#endregion
    
    private CharacterControl control;
    void Start()
    {
        if(!TryGetComponent(out control)) Debug.LogWarning("GameEventControl - controllerControl 없음...");
        control.Visible(false);
    }

    void OnEnable()
    {
        eventEnemySpawnAfter.Register(OnEventEnemySpawnAfter);
        eventSensorTargetEnter.Register(OnEventSensorTargetEnter);
        eventSensorTargetExit.Register(OnEventSensorTargetExit);
    }

    void OnDisable()
    {
        eventEnemySpawnAfter.Unregister(OnEventEnemySpawnAfter);
        eventSensorTargetEnter.Unregister(OnEventSensorTargetEnter);
        eventSensorTargetExit.Unregister(OnEventSensorTargetExit);
    }

    #region Event-Spawn After
    void OnEventEnemySpawnAfter(EventEnemySpawnAfter e){
        if(control != e.character) return;
        StartCoroutine(SpawnAfter(e));
    }

    IEnumerator SpawnAfter(EventEnemySpawnAfter e){
        Debug.Log("Enemy : SpawnAfter");
        yield return new WaitUntil(() => e.actorProfile.avatar != null && e.actorProfile.model != null);

        control.Profile = e.actorProfile;
        
        // Enemy 모델 생성 후 하위 항목인 Model에 설정
        if(e.actorProfile.model == null)
            Debug.LogError("CharacterEventControl ] model 없음.");

        var clone = Instantiate(e.actorProfile.model, control.model);
        
        if(e.actorProfile.avatar == null)
            Debug.LogError("CharacterEventControl ] avatar 없음.");

        control.animator.avatar = e.actorProfile.avatar;
               
        yield return new WaitForSeconds(1f);
        PoolManager.I.Spawn(e.spawnParticle, transform.position, Quaternion.identity, transform);
        
        yield return new WaitForSeconds(0.2f);
        
        control.Visible(true);
        control.PlayeAnimation(AnimationClipHashSet._SPAWN, 0f);

        yield return new WaitForSeconds(1f);
        
        foreach(var abilityData in e.actorProfile.abilities){
            control.abilityControl.Add(abilityData);
        }

        yield return new WaitForEndOfFrame();

        if(TryGetComponent(out CursorSelectable sel))
            sel.SetupRenderer();

// =TEMP CODE=
        yield return new WaitForEndOfFrame();
        control.abilityControl.Activate(AbilityFlag.Wandor);

// =TEMP CODE=
    
    }
    #endregion

    #region Event-Sensor
    void OnEventSensorTargetEnter(EventSensorTargetEnter e){
        if(control != e.from) return;
        control.abilityControl.Activate(AbilityFlag.Trace, true);
    }

    void OnEventSensorTargetExit(EventSensorTargetExit e){
        if(control != e.from) return;    
        control.abilityControl.Activate(AbilityFlag.Wandor, true);
    }
    #endregion
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