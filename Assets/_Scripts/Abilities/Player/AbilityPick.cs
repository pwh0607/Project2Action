using UnityEngine;

public class AbilityPick : Ability<AbilityPickData>
{    
    GameObject detectiveItem;
    GameObject currentItem;
    
    public AbilityPick(AbilityPickData data, CharacterControl owner) : base(data, owner) {
        if(owner.Profile == null) return;

        detectiveItem = null;
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
        if(detectiveItem != null && detectiveItem.GetComponent<LockedGate>() != null) return;

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
        if(currentItem != null || detectiveItem == null) return;

        if(detectiveItem.gameObject.GetComponent<Item>() != null){           
            Item item = detectiveItem.gameObject.GetComponent<Item>();
            owner.uiControl.GetItem(item);
            item.gameObject.SetActive(false);
            
            detectiveItem = null;
            currentItem = null;
        }else{
            currentItem = detectiveItem;
            
            if(detectiveItem.tag == "LOCKEDGATE") return;
            detectiveItem.GetComponent<Pickable>().Apply(owner);
            owner.animator.SetBool("PICK", true);
        }
    }

    private void ThrowItem(){
        if(currentItem == null) return;

        currentItem.GetComponent<Pickable>().Throw();
        
        currentItem = null;
        detectiveItem = null;
        owner.animator.SetBool("PICK", false);
    }

    private void CheckItem(){
        
        Debug.DrawRay(owner.transform.position + owner.transform.up * 0.2f, owner.transform.forward, Color.red, data.pickRange);
        
        if(currentItem != null) return;

        if(Physics.Raycast(owner.transform.position + owner.transform.up * 0.2f, owner.transform.forward, out RaycastHit hit, data.pickRange)){
            detectiveItem = hit.collider.gameObject;

            if (detectiveItem.tag == "HEAVYOBJECT" || detectiveItem.tag == "ITEM")
            {
                OnFound();
                return;
            }
            else if(detectiveItem.tag == "LOCKEDGATE"){
                owner.detectedGate = detectiveItem.GetComponentInParent<LockedGate>();
                return;
            }else{
                detectiveItem = null;
                owner.detectedGate = null;
                return;
            }
        }

        OnLost();
        detectiveItem = null;
        owner.detectedGate = null;
    }
    void OnFound()
    {
        data.eventSensorItemEnter.from = owner;
        data.eventSensorItemEnter.to = detectiveItem.gameObject;
        data.eventSensorItemEnter.Raise();
    }

    void OnLost()
    {
        if(detectiveItem == null) return;
        data.eventSensorItemExit.from = owner;
        data.eventSensorItemExit.to = detectiveItem.gameObject;
        data.eventSensorItemExit.Raise();
    }
}