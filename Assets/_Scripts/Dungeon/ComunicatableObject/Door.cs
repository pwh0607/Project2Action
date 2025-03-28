using DG.Tweening;
using UnityEngine;


public class Door : MonoBehaviour
{
    [SerializeField] Collider blockDoor;
    [SerializeField] Transform leftDoor;
    [SerializeField] Transform rightDoor;
    public bool isLocked{get; private set;}

    void Start()
    {
        DoorManager.I.OnOpenLogicCompleted += OpenDoor;
    }

    void OpenDoor(){
        isLocked = true;

        //문 열기 로직.
        // 오른쪽 100도, 왼쪽 -100도.
        leftDoor.DOLocalRotate(new Vector3(0, -100f, 0), 1f, RotateMode.Fast);              //수정 필요
        rightDoor.DOLocalRotate(new Vector3(0, 100f, 0), 1f, RotateMode.Fast);              //수정 필요
        Destroy(blockDoor.gameObject);
    }
}
