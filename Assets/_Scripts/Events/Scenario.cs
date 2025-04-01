using System.Collections;
using UnityEngine;

public class Scenario : MonoBehaviour
{
    public EventPlayerSpawnBefore playerSpawnerBefore;
    public EventPlayerSpawnAfter playerSpawnerAfter;
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        playerSpawnerBefore?.Raise();
    }
}
