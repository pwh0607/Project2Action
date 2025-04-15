using UnityEngine;

public class Breakable : MonoBehaviour, IInterative
{
    public PoolableParticle particle;
    public GameObject inItem;
    public void Apply(CharacterControl owner){
        Debug.Log("아이템 제거");
        if(inItem != null){
            inItem.transform.SetParent(null);
        }
        Destroy(gameObject);
    }
}