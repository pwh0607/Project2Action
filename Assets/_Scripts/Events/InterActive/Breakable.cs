using UnityEngine;

public class Breakable : MonoBehaviour, IInterative
{
    public PoolableParticle particle;

    public void Apply(CharacterControl owner){
        Debug.Log("아이템 제거");
        Destroy(gameObject);
    }
    
    
    // 이 아이템 내부에 다른 아이템이 존재하는 지... => 추후에 예정
}
