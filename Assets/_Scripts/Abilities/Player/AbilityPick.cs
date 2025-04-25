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

        // OnLost();
        if(detectiveItem.gameObject.GetComponent<Item>() != null){           //보유할 수 있는 
            Item item = detectiveItem.gameObject.GetComponent<Item>();
            owner.uiControl.GetItem(item);
            item.gameObject.SetActive(false);//DisableItem();
        }else{
            currentItem = detectiveItem;
            detectiveItem.GetComponent<Pickable>().Apply(owner);
        }
    }

    private void ThrowItem(){
        if(currentItem == null) return;

        currentItem.GetComponent<Pickable>().Throw();
        
        currentItem = null;
        detectiveItem = null;
    }

    private void CheckItem(){
        
        Debug.DrawRay(owner.transform.position + owner.transform.up * 0.2f, owner.transform.forward, Color.red, data.pickRange);
        
        if(currentItem != null) return;

        if(Physics.Raycast(owner.transform.position + owner.transform.up * 0.2f, owner.transform.forward, out RaycastHit hit, data.pickRange)){
            if (hit.transform.tag == "HEAVYOBJECT"|| hit.transform.tag == "ITEM")
            {
                detectiveItem = hit.collider.gameObject;
                OnFound();
                return;
            }
            else if(hit.transform.tag == "LOCKEDGATE"){
                Debug.Log("Door 인지");
                detectiveItem = hit.collider.gameObject;
                owner.detectedGate = detectiveItem.GetComponentInParent<LockedGate>();
                return;
            }
        }
       OnLost();
       detectiveItem = null;
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
        detectiveItem = null;
    }
}