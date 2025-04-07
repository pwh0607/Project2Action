using System.Collections;
using UnityEngine;

public class SpawnerPlayer : Spawner
{   
    //플레이어 스폰 이벤트
    [SerializeField] EventPlayerSpawnBefore eventPlayerSpawnBefore;
    [SerializeField] EventPlayerSpawnAfter eventPlayerSpawnAfter;
    
    void OnEnable()
    {
        // 이벤트가 등록 되면 발동, 등록 안하면 작동 안함.  [트리거ㄴ]
        eventPlayerSpawnBefore?.Register(OnEventPlayerSpawnBefore);
    }

    void OnDisable()
    {
        // 이벤트가 등록 되면 발동, 등록 안하면 작동 안함.
        eventPlayerSpawnBefore?.UnRegister(OnEventPlayerSpawnBefore);
    }
  
    void OnEventPlayerSpawnBefore(EventPlayerSpawnBefore e){
        CameraControl camera = Instantiate(e.playerCamera);
    
        _character = Instantiate(e.player);
        _character.transform.SetPositionAndRotation(spawnPoint.position, Quaternion.LookRotation(transform.forward));

        _cursor = Instantiate(e.playerCursor);
        _cursor.eyePoint = _character.eyePoint;

        StartCoroutine(SpawnAfter());
    }
    
    CharacterControl _character;
    CursorControl _cursor;
  
    IEnumerator SpawnAfter(){
        yield return new WaitForEndOfFrame();
        
        eventPlayerSpawnAfter.eyePoint = _character.eyePoint;
        eventPlayerSpawnAfter.CursorFixedPoint = _cursor.CursorFixedPoint;
        eventPlayerSpawnAfter.actorProfile = actorProfile;
        eventPlayerSpawnAfter?.Raise();
    }

}