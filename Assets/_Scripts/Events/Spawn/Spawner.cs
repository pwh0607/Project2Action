using System.Collections;
using CustomInspector;
using UnityEngine;

public class Spawner : MonoBehaviour
{
#region Events
    [Space(10)]
    [HorizontalLine("Events"), HideField] public bool _h0;
    [SerializeField] EventPlayerSpawnBefore eventPlayerSpawnBefore;
    [SerializeField] EventPlayerSpawnAfter eventPlayerSpawnAfter;
#endregion

    [Space(20)]
    public Transform spawnPoint;

    [SerializeField] ActorProfile actorProfile;

    void OnEnable()
    {
        eventPlayerSpawnBefore.Register(OnEventPlayerSpawnBefore);
    }

    void OnDisable()
    {
        eventPlayerSpawnBefore.UnRegister(OnEventPlayerSpawnBefore);
    }
    CharacterControl _character;
    CursorControl _cursor;
    
    void OnEventPlayerSpawnBefore(EventPlayerSpawnBefore e){
        CameraControl camera = Instantiate(e.playerCamera);
    
        _character = Instantiate(e.player);
        _character.transform.SetPositionAndRotation(spawnPoint.position, Quaternion.LookRotation(transform.forward));

        _cursor = Instantiate(e.playerCursor);
        _cursor.eyePoint = _character.eyePoint;

        StartCoroutine(DelayEvent());
    }

    IEnumerator DelayEvent(){
        yield return new WaitForEndOfFrame();
        
        eventPlayerSpawnAfter.eyePoint = _character.eyePoint;
        eventPlayerSpawnAfter.CursorFixedPoint = _cursor.CursorFixedPoint;
        eventPlayerSpawnAfter.actorProfile = actorProfile;
        eventPlayerSpawnAfter?.Raise();
    }
}