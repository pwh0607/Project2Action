using UnityEngine;
using UnityEngine.Events;

public class SidebarSlot : MonoBehaviour
{
    public Item item;
    private UnityAction<Item> OnSlotFocused;
    [SerializeField] GameObject focusFrame;

    void Start(){
        item = null;
    }
    
    public void FocusSlot(bool on){
        if(on) OnSlotFocused?.Invoke(item);
        focusFrame.SetActive(on);
    }

    public void RegisterEvent(UnityAction<Item> action){
        OnSlotFocused += action;
    }

    public void UnregisterEvent(UnityAction<Item> action){
        OnSlotFocused -= action;
    }

    public void SetIcon(ItemIcon icon){
        if(icon == null){
            Destroy(icon.gameObject);
            icon = null;
            return;
        }
        icon.transform.SetParent(transform);
        icon.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        
        this.item = icon.item;

        if(focusFrame.activeSelf){
            OnSlotFocused?.Invoke(icon.item);
        }
    }
}
