using UnityEngine;

public class AbilityPick : Ability<AbilityPickData>
{
    Pickable currentItem;
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
            }
            else{
                ThrowItem();
            }
        }
    }
    
    private void PickItem(){
        if(currentItem != null || pickableItem == null) return;

        pickableItem.Apply(owner);

        currentItem = pickableItem;
    }

    private void ThrowItem(){
        if(currentItem == null) return;

        currentItem.Throw();
        
        currentItem = null;
        
        pickableItem = null;
    }

    private void CheckItem(){
        Debug.DrawRay(owner.eyePoint.transform.position, owner.transform.forward, Color.red, 3f);
        
        if(currentItem != null) return;
        
        var list = Physics.OverlapSphere(owner.transform.position + owner.transform.forward, 2f, LayerMask.GetMask("HeavyObject"));

        if(list.Length <= 0) return;

        if(list[0].tag == "HEAVYOBJECT"){
            pickableItem = list[0].GetComponent<Pickable>();
            return;
        }
        
        pickableItem = null;
    }
}