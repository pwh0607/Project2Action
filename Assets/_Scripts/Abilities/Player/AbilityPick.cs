using UnityEngine;

public class AbilityPick : Ability<AbilityPickData>
{    
    public AbilityPick(AbilityPickData data, CharacterControl owner) : base(data, owner) {
        if(owner.Profile == null) return;

        owner.pickableItem = null;
    }

    public override void Activate(object obj)
    {
        owner.currentItem = null;
    }

    public override void Deactivate()
    {
        owner.currentItem = null;
    }

    public override void Update()
    {
        CheckItem();
        InputKeyboard();
    }
    
    public void InputKeyboard(){
        if(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            if(owner.currentItem == null){
                PickItem();
            }
            else{
                ThrowItem();
            }
        }
    }
    
    private void PickItem(){
        if(owner.currentItem != null || owner.pickableItem == null) return;

        if(owner.currentItem.gameObject.GetComponent<Item>() != null){
            Item item = owner.currentItem.gameObject.GetComponent<Item>();
            owner.uiControl.GetItem(item);
        }else{
            owner.pickableItem.Apply(owner);
            owner.currentItem = owner.pickableItem;
        }
    }

    private void ThrowItem(){
        if(owner.currentItem == null) return;

        owner.currentItem.Throw();
        
        owner.currentItem = null;
        
        owner.pickableItem = null;
    }

    private void CheckItem(){
        Debug.DrawRay(owner.eyePoint.transform.position, owner.transform.forward, Color.red, 3f);
        
        if(owner.currentItem != null) return;
        
        var list = Physics.OverlapSphere(owner.transform.position + owner.transform.forward, 2f, LayerMask.GetMask("HeavyObject"));

        if(list.Length <= 0) return;

        if(list[0].tag == "HEAVYOBJECT"){
            owner.pickableItem = list[0].GetComponent<Pickable>();
            //outline 생성하기
            return;
        }else if(list[0].tag == "DOOR"){
            Debug.Log("잠긴 문을 인식했다.");
        }
        
        owner.pickableItem = null;
    }
}