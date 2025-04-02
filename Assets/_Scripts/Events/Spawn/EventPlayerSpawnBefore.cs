using UnityEngine;
using CustomInspector;

[CreateAssetMenu(menuName = "GameEvent/EventPlayerSpawnBefore")]
public class EventPlayerSpawnBefore : GameEvent<EventPlayerSpawnBefore>
{
    public override EventPlayerSpawnBefore Item => this;

    public CharacterControl playerPrefab;
    public CursorControl playerCursor;
    public CameraControl playerCamera;   
}