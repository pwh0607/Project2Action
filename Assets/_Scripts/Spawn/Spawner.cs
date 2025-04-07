using CustomInspector;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    [Space(20)]
    [SerializeField, Foldout] protected ActorProfile actorProfile;

    [Space(20)]
    [SerializeField] protected Transform spawnPoint;
}