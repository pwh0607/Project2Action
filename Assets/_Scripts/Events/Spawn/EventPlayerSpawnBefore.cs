using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent/EventPlayerSpawnBefore")]
public class EventPlayerSpawnBefore : GameEvent<EventPlayerSpawnBefore>
{
    public override EventPlayerSpawnBefore Item => this;

    
    [Space(20)]
    public CharacterControl player;
    public CursorControl playerCursor;
    public CameraControl playerCamera;   
}