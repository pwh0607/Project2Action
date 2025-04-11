using UnityEngine;

public class Pickable : MonoBehaviour, IInterative
{
    public void Apply(CharacterControl owner){
        Debug.Log("아이템 픽!");
        gameObject.transform.parent = owner.transform;
    }

    public void Throw(){
        gameObject.transform.parent = null;
    }
}