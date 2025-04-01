using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Space(10)]
    [SerializeField] EventPlayerSpawnBefore eventPlayerSpawnBefore;

    [SerializeField] EventPlayerSpawnAfter eventPlayerSpawnAfter;
    public float radius = 3f;

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
    
        CharacterControl character = Instantiate(e.playerPrefab);
        character.transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(transform.forward));
    
        CursorControl cursor = Instantiate(e.playerCursor);
        cursor.eyePoint = character.eyePoint;

        eventPlayerSpawnAfter.eyePoint = character.eyePoint;
        eventPlayerSpawnAfter.cursorPoint = cursor.CursorPoint;
        Debug.Log("OnEventPlayerSpawnBefore 세팅");
        eventPlayerSpawnBefore?.Raise();
    }

    void OnDrawGizmos(){
        Gizmos.color= Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * radius * 2f);
    }
}