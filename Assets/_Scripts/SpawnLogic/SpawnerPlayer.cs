using System.Collections;
using UnityEngine;

public class SpawnerPlayer : Spawner
{   
    //플레이어 스폰 이벤트
    [SerializeField] EventPlayerSpawnBefore eventPlayerSpawnBefore;
    [SerializeField] EventPlayerSpawnAfter eventPlayerSpawnAfter;
    
    [Space(10)]
    CharacterControl _character;
    CursorControl _cursor;
    
    void OnEnable()
    {
        // 이벤트가 등록 되면 발동, 등록 안하면 작동 안함.  [트리거]
        eventPlayerSpawnBefore?.Register(OnEventPlayerSpawnBefore);
    }

    void OnDisable()
    {
        // 이벤트가 등록 되면 발동, 등록 안하면 작동 안함.
        eventPlayerSpawnBefore?.Unregister(OnEventPlayerSpawnBefore);
    }
  
    void OnEventPlayerSpawnBefore(EventPlayerSpawnBefore e){
        //플레이어용 카메라 생성
        CameraControl camera = Instantiate(e.playerCamera);
    
        //플레이어 캐릭터 생성
        _character = Instantiate(e.player);

        // 플레이어 캐릭터 위치 및 회전  설정.
        _character.transform.SetPositionAndRotation(spawnPoint.position, Quaternion.LookRotation(transform.forward));

        // 플레이어 캐릭터에 프로파일 연결
        _character.Profile = actorProfile;

        // 커서 세팅
        _cursor = Instantiate(e.playerCursor);
        _cursor.eyePoint = _character.eyePoint;
        
        StartCoroutine(SpawnAfter());
    }
    
  
    IEnumerator SpawnAfter(){
        yield return new WaitForEndOfFrame();
        
        eventPlayerSpawnAfter.eyePoint = _character.eyePoint;
        eventPlayerSpawnAfter.CursorFixedPoint = _cursor.CursorFixedPoint;
        eventPlayerSpawnAfter?.Raise();
    }

}