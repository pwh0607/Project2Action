using System;
using System.Threading.Tasks;
using UnityEngine;

public class GameEventRaiserCameraSwitch : MonoBehaviour
{
    [SerializeField] EventCameraSwitch eventCameraSwitch;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player") return;

        WaitForSeconds(2);
        // eventCameraSwitch.inout = true;
        // eventCameraSwitch?.Raise();
    }

    // void OnTriggerExit(Collider other)
    // {
    //     if(other.tag != "Player") return;
        
    //     eventCameraSwitch.inout = false;
    //     eventCameraSwitch?.Raise();
    // }

    //비동기[병렬 코드]
    async void WaitForSeconds(float t){
        try{
            eventCameraSwitch.inout = false;
            eventCameraSwitch?.Raise();

            // 단위 밀리 세컨드.            1000 ms = 1 s
            // 2초 후에 그다음 코드를 수행한다.
            await Task.Delay(2000);

            eventCameraSwitch.inout = true;
            eventCameraSwitch?.Raise();
        }
        catch(SystemException e){
            Debug.LogException(e);
        }
        finally{
            
        }
    }
}
