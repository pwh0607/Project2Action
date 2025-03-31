using Unity.Cinemachine;
using UnityEngine;

public class GameEventListenerCameraSwitch : MonoBehaviour
{
    [SerializeField] GameEventCameraSwitch eventCameraSwitch;

    [SerializeField] CinemachineVirtualCameraBase virtualCamera;

    void Start()
    {
        virtualCamera.Priority.Value = 0;   
    }

    void OnEnable(){
        eventCameraSwitch.Register(OnEventCameraSwitch);
    }

    void OnDisable(){
        eventCameraSwitch.UnRegister(OnEventCameraSwitch);
    }

    private void OnEventCameraSwitch(GameEventCameraSwitch e){
        Debug.Log(" 카메라 이벤트...");
        SwtichCamera(e.inout);
    }  

    void SwtichCamera(bool on){
        if(on)
            virtualCamera.Priority.Value++;
        else
            virtualCamera.Priority.Value--;
    }
}