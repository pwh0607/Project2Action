using Unity.Cinemachine;
using UnityEngine;

public class EventCamera : MonoBehaviour
{
    public CinemachineVirtualCameraBase cameraEvent1;           // 

    void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player") return;
        Debug.Log($"Trigger Enter : {other.name}");       

        var cc = other.GetComponentInParent<CharacterControl>();
        if(cc == null) return;
        
        cc.mainCamera.Priority.Value -= 1;
        cameraEvent1.Priority.Value += 1;
    }

//임시
    void OnTriggerExit(Collider other)
    {
        if(other.tag != "Player") return;
        var cc = other.GetComponentInParent<CharacterControl>();
        
        if(cc == null) return;
    
        cc.mainCamera.Priority.Value += 1;
        cameraEvent1.Priority.Value -= 1;
    }
}