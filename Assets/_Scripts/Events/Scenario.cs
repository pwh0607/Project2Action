using System.Collections;
using UnityEngine;

public class Scenario : MonoBehaviour
{
    public EventPlayerSpawnBefore playerSpawnerBefore;
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        playerSpawnerBefore?.Raise();
    }
}