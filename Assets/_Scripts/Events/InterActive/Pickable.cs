using UnityEngine;

public class Pickable : MonoBehaviour, IInterative
{
    public void Apply(CharacterControl owner){
        Debug.Log("아이템 픽!");
        transform.parent = owner.transform;
        transform.localPosition = Vector3.zero;
    }

    public void Throw(){
        gameObject.transform.parent = null;
    }
}