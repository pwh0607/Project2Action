using System.Collections;
using CustomInspector;
using UnityEngine;

public class Scenario : MonoBehaviour
{
    
    [HorizontalLine("Event-Spawn"), HideField] public bool h_s_01;
    public EventPlayerSpawnBefore playerSpawnerBefore;
    [HorizontalLine(color:FixedColor.Cyan), HideField] public bool h_e_01;
    
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        // playerSpawnerBefore?.Raise();
    }
}