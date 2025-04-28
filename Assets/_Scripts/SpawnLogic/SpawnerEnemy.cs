using System.Collections;
using UnityEngine;

public class SpawnerEnemy : Spawner
{
    [SerializeField] EventEnemySpawnBefore eventEnemySpawnBefore;
    [SerializeField] EventEnemySpawnAfter eventEnemySpawnAfter;

    [Space(10)]
    CharacterControl control;

    void OnEnable()
    {
        eventEnemySpawnBefore?.Register(OnEventEnemySpawnBefore);
    }

    void OnDisable()
    {
        eventEnemySpawnBefore?.Unregister(OnEventEnemySpawnBefore);
    }

    void OnEventEnemySpawnBefore(EventEnemySpawnBefore e){
        control = Instantiate(e.enemyCharacter);
        control.transform.SetPositionAndRotation(spawnPoint.position, Quaternion.LookRotation(transform.forward));
        control.Profile = actorProfile;
        StartCoroutine(SpawnAfter());
    }

    IEnumerator SpawnAfter(){
        yield return new WaitForEndOfFrame();

        eventEnemySpawnAfter.character = control;
        eventEnemySpawnAfter.eyePoint = control.eyePoint;
        eventEnemySpawnAfter?.Raise();
    }
}