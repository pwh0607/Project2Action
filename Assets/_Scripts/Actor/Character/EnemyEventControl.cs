using System.Collections;
using UnityEngine;

public class EnemyEventControl : MonoBehaviour
{
#region Events
    [SerializeField] EventEnemySpawnAfter eventEnemySpawnAfter;

    [SerializeField] EventSensorSightEnter eventSensorSightEnter;
    [SerializeField] EventSensorSightExit eventSensorSightExit;

    [SerializeField] EventSensorAttackEnter eventSensorAttackEnter;
    [SerializeField] EventSensorAttackExit eventSensorAttackExit;
#endregion
    
    [Space(10)]

    private CharacterControl control;
    void Start()
    {
        if(!TryGetComponent(out control)) Debug.LogWarning("GameEventControl - controllerControl 없음...");
        control.Visible(false);
    }

    void OnEnable()
    {
        eventEnemySpawnAfter.Register(OnEventEnemySpawnAfter);

        eventSensorSightEnter.Register(OnEventSensorSightEnter);
        eventSensorSightExit.Register(OnEventSensorSightExit);

        eventSensorAttackEnter.Register(OnEventSensorAttackEnter);
        eventSensorAttackExit.Register(OnEventSensorAttackExit);
    }

    void OnDisable()
    {
        eventEnemySpawnAfter.Unregister(OnEventEnemySpawnAfter);

        eventSensorSightEnter.Unregister(OnEventSensorSightEnter);
        eventSensorSightExit.Unregister(OnEventSensorSightExit);

        eventSensorAttackEnter.Unregister(OnEventSensorAttackEnter);
        eventSensorAttackExit.Unregister(OnEventSensorAttackExit);
    }

    #region Event-Spawn After
    void OnEventEnemySpawnAfter(EventEnemySpawnAfter e){
        if(control != e.character) return;
        StartCoroutine(SpawnAfter(e));
    }

    IEnumerator SpawnAfter(EventEnemySpawnAfter e){
        Debug.Log("Enemy : SpawnAfter");
        yield return new WaitUntil(() => control.Profile.avatar != null && control.Profile.models != null);

        control.Profile = control.Profile;
        
        // Enemy 모델 생성 후 하위 항목인 Model에 설정
        if(control.Profile.models == null)
            Debug.LogError("CharacterEventControl ] model 없음.");

        var model = control.Profile.models.Random();
        var clone = Instantiate(model, control.model);
        
        if(control.Profile.avatar == null)
            Debug.LogError("CharacterEventControl ] avatar 없음.");

        control.animator.avatar = control.Profile.avatar;
               
        yield return new WaitForSeconds(1f);
        PoolManager.I.Spawn(e.spawnParticle, transform.position, Quaternion.identity, transform);
        
        yield return new WaitForSeconds(0.2f);
        
        control.Visible(true);
        control.PlayeAnimation(AnimationClipHashSet._SPAWN, 0f);

        yield return new WaitForSeconds(1f);
        
        foreach(var abilityData in control.Profile.abilities){
            control.abilityControl.Add(abilityData);
        }

        yield return new WaitForEndOfFrame();

        if(TryGetComponent(out CursorSelectable sel))
            sel.SetupRenderer();

        yield return new WaitForSeconds(1f);
        control.abilityControl.Activate(AbilityFlag.Wandor, true, null);
    }
    #endregion

    #region Event-Sensor Sight
    void OnEventSensorSightEnter(EventSensorSightEnter e){
        if(control != e.from) return;
        Debug.Log("시야에 들어왔다.");
        control.abilityControl.Activate(AbilityFlag.Trace, true, e.to);
    }

    void OnEventSensorSightExit(EventSensorSightExit e){
        if(control != e.from) return;    
        Debug.Log("시야에서 벗어났다.");
        control.abilityControl.Activate(AbilityFlag.Wandor, true, e.to);
    }
    #endregion

    #region Event-Sensor Attack
    void OnEventSensorAttackEnter(EventSensorAttackEnter e){
        if(control != e.from)
            return;
        Debug.Log("OnEventSensorAttackEnter!");
        control.abilityControl.Activate(AbilityFlag.Attack, true, e.to);
    }

    void OnEventSensorAttackExit(EventSensorAttackExit e){
        if(control != e.from) return;    
        
        Debug.Log("OnEventSensorAttackExit!");
        control.abilityControl.Activate(AbilityFlag.Trace, true, e.to);
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