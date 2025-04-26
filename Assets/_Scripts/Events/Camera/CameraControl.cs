using CustomInspector;
using Unity.Cinemachine;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    
    [HorizontalLine("Event-Spawn"), HideField] public bool h_s_01;
    [SerializeField] EventPlayerSpawnAfter eventPlayerSpawnAfter;
    [HorizontalLine(color:FixedColor.Cyan), HideField] public bool h_e_01;
    [SerializeField] CinemachineTargetGroup targetGroup;
    
    void OnEnable()
    {
        eventPlayerSpawnAfter.Register(OnEventPlayerSpawnAfter);
    }

    void OnDisable()
    {
        eventPlayerSpawnAfter.Unregister(OnEventPlayerSpawnAfter);
    }

    void OnEventPlayerSpawnAfter(EventPlayerSpawnAfter e){
        targetGroup = GetComponentInChildren<CinemachineTargetGroup>();
        targetGroup.Targets.Clear();

        CinemachineTargetGroup.Target main = new CinemachineTargetGroup.Target();
        main.Object = e.eyePoint;
        main.Weight= 0.8f;

        targetGroup.Targets.Add(main);
    }
}
