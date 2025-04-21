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
        // StageLogicManager.I.OnOpenLogicCompleted += OpenGate;
    }

    public void OpenGate(){
        type = GateType.OPEN;
        
        Debug.Log("문 열기");
        if(blockDoor.gameObject.activeSelf) blockDoor.gameObject.SetActive(false);

        leftDoor.DOLocalRotate(new Vector3(0, 100f, 0), 1, RotateMode.FastBeyond360);
        rightDoor.DOLocalRotate(new Vector3(0, -100f, 0), 1, RotateMode.FastBeyond360);
    }
    
    public void CloseGate(){
        type = GateType.LOCK;
        Debug.Log("문 닫기");
        if(!blockDoor.gameObject.activeSelf) blockDoor.gameObject.SetActive(true);

        leftDoor.DOLocalRotate(Vector3.zero, 1, RotateMode.FastBeyond360);
        rightDoor.DOLocalRotate(Vector3.zero, 1, RotateMode.FastBeyond360);
    }
}
