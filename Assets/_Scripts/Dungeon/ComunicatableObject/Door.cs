using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Collider blockDoor;
    [SerializeField] Transform leftDoor;
    [SerializeField] Transform rightDoor;
    public bool isLocked{get; private set;} = true;

    void Start()
    {
        DoorManager.I.OnOpenLogicCompleted += OpenDoor;
    }

    void OpenDoor(){
        isLocked = false;
        if(blockDoor.gameObject.activeSelf) Destroy(blockDoor.gameObject);
    }
}