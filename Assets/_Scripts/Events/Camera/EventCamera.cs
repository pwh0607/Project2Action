using Unity.Cinemachine;
using UnityEngine;

public class EventCamera : MonoBehaviour
{
    public CinemachineVirtualCameraBase cameraEvent1;           
    void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player") return;
        Debug.Log($"Trigger Enter : {other.name}");       

        var cc = other.GetComponentInParent<CharacterControl>();
        if(cc == null) return;

        cameraEvent1.Priority.Value += 1;

        // 플레이어 무브 어빌리티 제거

        // 이벤트 플레이

        // 이벤트 종료 -> 리버스 플레이
    }

//임시
    void OnTriggerExit(Collider other)
    {
        if(other.tag != "Player") return;
        var cc = other.GetComponentInParent<CharacterControl>();
        
        if(cc == null) return;
        cameraEvent1.Priority.Value -= 1;
    }
}