using System.Collections;
using UnityEngine;

public class SpawnerEnemy : Spawner
{
    [SerializeField] EventEnemySpawnBefore eventEnemySpawnBefore;
    [SerializeField] EventEnemySpawnAfter eventEnemySpawnAfter;

    void OnEnable()
    {
        // 이벤트가 등록 되면 발동, 등록 안하면 작동 안함.  [트리거]
        eventEnemySpawnBefore?.Register(OnEventEnemySpawn);
    }

    void OnDisable()
    {
        // 이벤트가 등록 되면 발동, 등록 안하면 작동 안함.
        eventEnemySpawnBefore?.UnRegister(OnEventEnemySpawn);
    }

    EnemyControl _enemyControl;

    void OnEventEnemySpawn(EventEnemySpawnBefore e){
        Debug.Log("적 생성!");
        _enemyControl = Instantiate(e.enemyCharacter).GetComponent<EnemyControl>();
        _enemyControl.transform.SetPositionAndRotation(spawnPoint.position, Quaternion.LookRotation(transform.forward));

        StartCoroutine(SpawnAfter());
    }

    IEnumerator SpawnAfter(){
        yield return new WaitForEndOfFrame();

        eventEnemySpawnAfter.eyePoint = _enemyControl.eyePoint;
        eventEnemySpawnAfter.actorProfile = actorProfile;
        eventEnemySpawnAfter?.Raise();
    }
}