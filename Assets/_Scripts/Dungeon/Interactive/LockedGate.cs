using System;
using DG.Tweening;
using UnityEngine;


[Serializable]
public class LockedGate : InterActiveGate
{
    [SerializeField] Collider blockDoor;
    [SerializeField] Transform leftDoor;
    [SerializeField] Transform rightDoor;

    void Start()
    {
        // StageLogicManager.I.OnOpenLogicCompleted += OpenDoor;
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
