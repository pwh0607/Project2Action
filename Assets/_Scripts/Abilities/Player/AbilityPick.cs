using System.Linq;
using UnityEngine;

public class AbilityPick : Ability<AbilityPickData>
{
    Pickable currentItem;              //현재 잡고 있는 아이템
    Pickable pickableItem;
    
    public AbilityPick(AbilityPickData data, CharacterControl owner) : base(data, owner) {
        if(owner.Profile == null) return;

        pickableItem = null;
    }

    public override void Activate(object obj)
    {        
        currentItem = null;
    }

    public override void Deactivate()
    {
        currentItem = null;
    }

    public override void Update()
    {
        CheckItem();
        InputKeyboard();
    }
    
    public void InputKeyboard(){
        if(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            if(currentItem == null){
                PickItem();
            }else{
                ThrowItem();
            }
        }
    }
    
    private void PickItem(){
        if(currentItem != null || pickableItem == null) return;             // 이미 아이템을 가지고 있다면 무시.

        Debug.Log("Take!!");
        pickableItem.Apply(owner);
        currentItem = pickableItem;
    }

    private void ThrowItem(){
        if(currentItem != null) return;
        Debug.Log("Throw!!");
        pickableItem.Throw();
        
        currentItem = null;
    }

    private void CheckItem(){
        Debug.DrawRay(owner.eyePoint.transform.position, owner.transform.forward, Color.red, 3f);
        
        if(currentItem != null) return;
        
        var list = Physics.OverlapSphere(owner.transform.position + owner.transform.forward, 2f, LayerMask.GetMask("HeavyObject"));

        Debug.Log($"list sz : {list.Length}");

        if(list[0].tag == "HEAVYOBJECT"){
            Debug.Log(" 무거운 오브젝트;'");
            pickableItem = list[0].GetComponent<Pickable>();

            return;
        }
        
        pickableItem = null;       
    }
}