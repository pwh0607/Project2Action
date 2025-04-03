using Unity.Cinemachine;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // 타겟 그룹 알기
    [SerializeField] EventPlayerSpawnAfter eventPlayerSpawnAfter;
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
        main.Object = e.eyePoint;
        main.Weight= 0.8f;

        CinemachineTargetGroup.Target sub = new CinemachineTargetGroup.Target();
        sub.Object = e.CursorFixedPoint;        
        sub.Weight= 0.2f;

        targetGroup.Targets.Add(main);
        targetGroup.Targets.Add(sub);
    }
}
