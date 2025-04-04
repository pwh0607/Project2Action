using UnityEngine;
using DG.Tweening;

public class InterActiveGate : InteractiveObject
{
    public GateType type;
    public Vector3 doorPos {get; private set;}
    [SerializeField] Collider blockDoor;
    [SerializeField] Transform leftDoor;
    [SerializeField] Transform rightDoor;

    void Start()
    {
        StageLogicManager.I.OnOpenLogicCompleted += OpenDoor;
    }

    void OpenDoor(){
        type = GateType.OPEN;
        if(blockDoor.gameObject.activeSelf) Destroy(blockDoor.gameObject);

        leftDoor.DOLocalRotate(new Vector3(0, 100f, 0), 1, RotateMode.FastBeyond360);
        rightDoor.DOLocalRotate(new Vector3(0, -100f, 0), 1, RotateMode.FastBeyond360);
    }

    void CloseDoor(){
        type = GateType.LOCK;
    }
}