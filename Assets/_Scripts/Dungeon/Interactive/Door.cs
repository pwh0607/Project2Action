using DG.Tweening;
using UnityEngine;

public class Door : InteractiveObject
{
    public Vector3 doorPos {get; private set;}
    [SerializeField] Collider blockDoor;
    [SerializeField] Transform leftDoor;
    [SerializeField] Transform rightDoor;
    public bool isLocked{get; private set;} = true;

    void Start()
    {
        StageLogicManager.I.OnOpenLogicCompleted += OpenDoor;
    }

    void OpenDoor(){
        isLocked = false;
        if(blockDoor.gameObject.activeSelf) Destroy(blockDoor.gameObject);

        leftDoor.DOLocalRotate(new Vector3(0, 100f, 0), 1, RotateMode.FastBeyond360);
        rightDoor.DOLocalRotate(new Vector3(0, -100f, 0), 1, RotateMode.FastBeyond360);
    }

    void CloseDoor(){
        isLocked = true;
    }
}