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

    private CharacterControl owner;
    void Start()
    {
        if(!TryGetComponent(out owner)) Debug.LogWarning("GameEventControl - controllerControl 없음...");
        owner.Visible(false);
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
        if(owner != e.character) return;
        StartCoroutine(SpawnAfter(e));
    }

    IEnumerator SpawnAfter(EventEnemySpawnAfter e){
        Debug.Log("Enemy : SpawnAfter");
        yield return new WaitUntil(() => owner.Profile.avatar != null && owner.Profile.models != null);

        owner.Profile = owner.Profile;
        
        if(owner.Profile.models == null) Debug.LogError("CharacterEventControl ] model 없음.");

        var model = owner.Profile.models.Random();
        var clone = Instantiate(model, owner.model);
        
        if(owner.Profile.avatar == null) Debug.LogError("CharacterEventControl ] avatar 없음.");

        owner.animator.avatar = owner.Profile.avatar;
               
        yield return new WaitForSeconds(1f);
        PoolManager.I.Spawn(e.spawnParticle, transform.position, Quaternion.identity, transform);
        
        yield return new WaitForSeconds(0.2f);
        
        owner.Visible(true);
        owner.PlayAnimation("SPAWN", 0f);

        yield return new WaitForSeconds(1f);
        
        foreach(var abilityData in owner.Profile.abilities){
            owner.abilityControl.Add(abilityData);
        }

        yield return new WaitForEndOfFrame();
        owner.uiControl.Show(true);

        if(TryGetComponent(out CursorSelectable sel))
            sel.SetupRenderer();

        yield return new WaitForSeconds(1f);
        owner.abilityControl.Activate(AbilityFlag.Wandor, true, null);
    }
    #endregion

    #region Event-Sensor Sight
    void OnEventSensorSightEnter(EventSensorSightEnter e){
        if(owner != e.from) return;
        owner.abilityControl.Activate(AbilityFlag.Trace, true, e.to);
    }

    void OnEventSensorSightExit(EventSensorSightExit e){
        if(owner != e.from) return;
        owner.abilityControl.Activate(AbilityFlag.Wandor, true, null);
    }
    #endregion

    #region Event-Sensor Attack
    void OnEventSensorAttackEnter(EventSensorAttackEnter e){
        if(owner != e.from) return;
        owner.abilityControl.Activate(AbilityFlag.Attack, true, e.to);
    }

    void OnEventSensorAttackExit(EventSensorAttackExit e){
        if(owner != e.from) return;    
        owner.abilityControl.Activate(AbilityFlag.Trace, false, e.to);
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