using UnityEngine;

public class KeyFactory : BehaviourSingleton<KeyFactory>
{
    protected override bool IsDontDestroy() => true;

    // [SerializeField] 
}
