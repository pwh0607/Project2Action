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

    void OnEventPlayerSpawnBefore(EventPlayerSpawnBefore e){
        CameraControl camera = Instantiate(e.playerCamera);
    
        CharacterControl character = Instantiate(e.player);
        character.transform.SetPositionAndRotation(spawnPoint.position, Quaternion.LookRotation(transform.forward));

        CursorControl cursor = Instantiate(e.playerCursor);
        cursor.eyePoint = character.eyePoint;

        eventPlayerSpawnAfter.eyePoint = character.eyePoint;
        eventPlayerSpawnAfter.CursorFixedPoint = cursor.CursorFixedPoint;
        eventPlayerSpawnAfter.actorProfile = actorProfile;
        eventPlayerSpawnAfter?.Raise();
    }
}