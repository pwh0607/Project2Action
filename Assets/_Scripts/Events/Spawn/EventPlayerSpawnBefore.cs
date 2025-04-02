using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent/EventPlayerSpawnBefore")]
public class EventPlayerSpawnBefore : GameEvent<EventPlayerSpawnBefore>
{
    public override EventPlayerSpawnBefore Item => this;

    public CharacterControl player;
    public CursorControl playerCursor;
    public CameraControl playerCamera;   
}