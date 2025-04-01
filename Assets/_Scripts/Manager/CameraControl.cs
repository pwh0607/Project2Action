using Unity.Cinemachine;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // 타겟 그룹 알기
    EventPlayerSpawnAfter eventPlayerSpawnAfter;
    [SerializeField] CinemachineTargetGroup targetGroup;

    void OnEnable()
    {
        eventPlayerSpawnAfter.Register(OnEventPlayerSpawnAfter);
    }

    void OnDisable()
    {
        
        eventPlayerSpawnAfter.UnRegister(OnEventPlayerSpawnAfter);
    }

    void OnEventPlayerSpawnAfter(EventPlayerSpawnAfter e){
        targetGroup.Targets.Clear();

        CinemachineTargetGroup.Target main = new CinemachineTargetGroup.Target();
        // main.Object = e.Eye
        
        CinemachineTargetGroup.Target sub = new CinemachineTargetGroup.Target();
    }
}
