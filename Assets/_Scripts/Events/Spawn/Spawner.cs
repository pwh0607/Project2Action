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
    
    public float radius = 3f;
    public Transform spawnPosition;

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
        character.transform.SetPositionAndRotation(spawnPosition.position, Quaternion.LookRotation(transform.forward));

        CursorControl cursor = Instantiate(e.playerCursor);
        cursor.eyePoint = character.eyePoint;

        eventPlayerSpawnAfter.eyePoint = character.eyePoint;
        eventPlayerSpawnAfter.cursorPoint = cursor.CursorPoint;
        eventPlayerSpawnAfter?.Raise();            
    }

    void OnDrawGizmos(){
        Gizmos.color= Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * radius * 2f);
    }
}