using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class SidebarController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IScrollHandler
{
    private bool isMouseOver;

    [SerializeField] SidebarData data;
    [SerializeField] int slotCount;
    [SerializeField] GameObject slotPrefab;
    List<SidebarSlot> slots = new();
    [ReadOnly] public Item selectedItem;
    [SerializeField, ReadOnly] int focusIndex;
    private CharacterControl owner;
    void Start()
    {
        Debug.Log("사이드바 생성!");
        owner = GetComponentInParent<CharacterControl>();
        isMouseOver = false;
        selectedItem = null;

        InitSlots();
        focusIndex = 0;
        slotPrefab = data.slot.gameObject;
    }

    void InitSlots(){
        for(int i=0;i<slotCount;i++){
            SidebarSlot slot = Instantiate(slotPrefab, transform).GetComponent<SidebarSlot>();
            slot.RegisterEvent(SetSelectedItem);
            slots.Add(slot);
        }
    }

    void SetSelectedItem(Item item){
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

    public bool GetItem(Item item){
        SidebarSlot slot = SearchEmptySlot();

        if(slot == null) return false;

        ItemIcon icon = IconFactory.I.CreateItemIcon(item);
        slot.SetIcon(icon);

        return true;
    }

    SidebarSlot SearchEmptySlot(){
        foreach(var slot in slots){
            if(slot.item == null) return slot;
        }
        return null;
    }

    void Update()
    {
        if(isMouseOver){
            if(Input.GetKeyDown(KeyCode.LeftShift)){                //우클릭시 사용
                UseItem();
            }
        }
    }

    public void UseItem(){          //test public
        //플레이어가 인지한 잠긴 문이 있다면..?
        if(selectedItem is NormalKey key){
            if(key.Use(owner.detectedGate))               //null은 임시 값         
            {
                //알맞은 키를 사용했을 떄.
                slots[focusIndex].RemoveIcon();
            }
        }
    }
}
