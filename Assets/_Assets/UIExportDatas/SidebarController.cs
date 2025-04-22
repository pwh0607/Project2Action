using System.Collections.Generic;
using CustomInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class SidebarController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IScrollHandler
{
    private bool isMouseOver;

    [SerializeField] int slotCount;
    [SerializeField] GameObject slotPrefab;
    List<SidebarSlot> slots = new();
    [ReadOnly] GameObject selectedItem;
    [SerializeField, ReadOnly] int focusIndex;

    void Start()
    {
        isMouseOver = false;
        selectedItem = null;

        InitSlots();
        focusIndex = 0;

        UpdateFocus();
    }

    void InitSlots(){
        for(int i=0;i<slotCount;i++){
            SidebarSlot slot = Instantiate(slotPrefab, transform).GetComponent<SidebarSlot>();
            slot.RegisterEvent(SetSelectedItem);
            slots.Add(slot);
        }
    }

    void SetSelectedItem(GameObject item){
        selectedItem = item;
    }

    #region MouseEvent
    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (isMouseOver)
        {
            float scrollDelta = eventData.scrollDelta.y > 0 ? 1 : -1;
            focusIndex = Mathf.Clamp(focusIndex - (int)scrollDelta, 0, slots.Count - 1);

            UpdateFocus();
        }
    }

    void UpdateFocus(){
        for(int i=0;i<slots.Count;i++){
            if(i == focusIndex){
                slots[i].FocusSlot(true);
            }
            else{
                slots[i].FocusSlot(false);
            }
        }
    }
    #endregion

    public void GetItem(GameObject item){
        SidebarSlot slot = SearchEmptySlot();

        item.transform.SetParent(slot.transform);
    }

    SidebarSlot SearchEmptySlot(){
        foreach(var slot in slots){
            if(slot.item == null) return slot;
        }
        return null;
    }

    public void GetItem(ItemData item){
        
    }
}
