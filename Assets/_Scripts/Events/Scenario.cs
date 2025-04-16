using System.Collections;
using CustomInspector;
using UnityEngine;

public class Scenario : MonoBehaviour
{
    
    [HorizontalLine("Event-Spawn"), HideField] public bool h_s_01;
    
    public EventPlayerSpawnBefore playerSpawnerBefore;
    public EventEnemySpawnBefore enemySpawn;
    
    public EventDeath eventDeath;

    [HorizontalLine(color:FixedColor.Cyan), HideField] public bool h_e_01;
    
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        // 캐릭터 스폰
        playerSpawnerBefore?.Raise();
    
        // 적 스폰
        enemySpawn?.Raise();

        //아이템 스폰

        yield return new WaitForSeconds(1f);
        GameManager.I.ShowInfo("Escape Dungeon", 3f);
    }
    
    void OnEnable()
    {
        eventDeath.Register(OnEventPlayerDeath);
    }

    void OnDisable()
    {
        eventDeath.Unregister(OnEventPlayerDeath);
    }

    void OnEventPlayerDeath(EventDeath e){
        if(e.target.Profile.actorType == ActorType.PLAYER){
            GameManager.I.ShowInfo("PLAYER DIED", 5f);
        }
    }
}