using UnityEngine;

public class RoomEdge : MonoBehaviour
{
    public Vector2Int roomNumber;
    public Vector3 edgePosition;

    void Awake()
    {
        edgePosition = transform.position;
    }
}