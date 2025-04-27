using UnityEngine;

public class Breakable : MonoBehaviour
{
    public PoolableParticle particle;
    public GameObject inItem;
    public void Apply(CharacterControl owner){
        if(inItem != null){
            inItem.transform.SetParent(null);
        }
        Destroy(gameObject);
    }
}