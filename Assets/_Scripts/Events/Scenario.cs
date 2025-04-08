using System.Collections;
using CustomInspector;
using UnityEngine;

public class Scenario : MonoBehaviour
{
    
    [HorizontalLine("Event-Spawn"), HideField] public bool h_s_01;
    public EventPlayerSpawnBefore playerSpawnerBefore;
    
    public EventEnemySpawnBefore enemySpawn;
    [HorizontalLine(color:FixedColor.Cyan), HideField] public bool h_e_01;
    
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        // 캐릭터 스폰
        playerSpawnerBefore?.Raise();
    
        // 적 스폰
        enemySpawn?.Raise();

        //아이템 스폰
    }
}