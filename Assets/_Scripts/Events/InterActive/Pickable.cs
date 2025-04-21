using UnityEngine;

public class Pickable : MonoBehaviour, IInterative
{
    private Rigidbody rb;
    private Collider col;

    void Start()
    {
        TryGetComponent(out rb);
        TryGetComponent(out col);
    }

    public void Apply(CharacterControl owner){
        Debug.Log("아이템 픽!");
        rb.isKinematic = true;
        rb.useGravity = false;
        col.isTrigger = true;
        transform.SetParent(owner.playerHand);
        
        transform.localPosition = Vector3.zero;
    }

    public void Throw(){
        gameObject.transform.parent = null;
        col.isTrigger = false;
        rb.isKinematic = false;
        rb.useGravity = true;
    }
}