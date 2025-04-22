using UnityEngine;
using UnityEngine.Events;

public class SidebarSlot : MonoBehaviour
{
    public GameObject item;
    private UnityAction<GameObject> OnSlotFocused;
    [SerializeField] GameObject focusFrame;

    void Start(){
        item = null;
    }
    
    public void FocusSlot(bool on){
        if(on) OnSlotFocused?.Invoke(item);

        focusFrame.SetActive(on);
    }

    public void RegisterEvent(UnityAction<GameObject> action){
        OnSlotFocused += action;
    }

    public void UnregisterEvent(UnityAction<GameObject> action){
        OnSlotFocused -= action;
    }
}
