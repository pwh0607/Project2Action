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
        Debug.Log("Pick!!");
        pickableItem.Apply(owner);
    }

    private void ThrowItem(){
        if(currentItem != null) return;
        Debug.Log("Throw!!");
        pickableItem.Throw();
    }

    private void CheckItem(){

        Debug.DrawRay(owner.eyePoint.transform.position, owner.transform.forward, Color.red, 3f);
        
        if(currentItem != null) return;

        if(Physics.Raycast(owner.eyePoint.transform.position, owner.transform.forward, out RaycastHit hit, data.pickRange)){
            IInterative interative = hit.collider.GetComponent<IInterative>();
            if(interative is Pickable pickable){
                pickableItem = pickable;
            }
            
        }
        pickableItem = null;       
    }
}