using System.Collections;
using UnityEngine;

public class EnemyEventControl : MonoBehaviour
{
#region Events
    [SerializeField] EventEnemySpawnAfter eventEnemySpawnAfter;
#endregion
    
    private EnemyControl enemyController;
    void Start()
    {
        if(!TryGetComponent(out enemyController)) Debug.LogWarning("GameEventControl - controllerControl 없음...");
        enemyController.Visible(false);
    }

    void OnEnable()
    {
        eventEnemySpawnAfter.Register(OnEventEnemySpawnAfter);
    }

    void OnDisable()
    {
        eventEnemySpawnAfter.UnRegister(OnEventEnemySpawnAfter);
    }


    void OnEventEnemySpawnAfter(EventEnemySpawnAfter e){
        StartCoroutine(SpawnAfter(e));
    }
    
    IEnumerator SpawnAfter(EventEnemySpawnAfter e){
        Debug.Log("Enemy : SpawnAfter");
        yield return new WaitUntil(() => e.actorProfile.avatar != null && e.actorProfile.model != null);

        enemyController.Profile = e.actorProfile;
        
        // Enemy 모델 생성 후 하위 항목인 Model에 설정
        if(e.actorProfile.model == null)
            Debug.LogError("CharacterEventControl ] model 없음.");

        var clone = Instantiate(e.actorProfile.model, enemyController.model);
        
        if(e.actorProfile.avatar == null)
            Debug.LogError("CharacterEventControl ] avatar 없음.");

        enemyController.animator.avatar = e.actorProfile.avatar;
               
        yield return new WaitForSeconds(1f);
        PoolManager.I.Spawn(e.spawnParticle, transform.position, Quaternion.identity, transform);
        
        yield return new WaitForSeconds(0.2f);
        
        enemyController.Visible(true);
        enemyController.PlayeAnimation(AnimationClipHashSet._SPAWN, 0f);

        yield return new WaitForSeconds(1f);
        
        foreach(var abilityData in e.actorProfile.abilities){
            enemyController.ability.Add(abilityData, true);
        }
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