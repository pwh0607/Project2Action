using UnityEngine;
using CustomInspector;

public class SidebarTester : MonoBehaviour
{
    public SidebarController sidebar;
    public Item item;
    public int index;
    [Button("MakeItem"), HideField] public bool _btn1;
    [Button("UseItem"), HideField] public bool _btn2;

    // Factory 패턴으로 아이템 아이콘 생성 예정
    void MakeItem(){
        sidebar.GetItem(item);
    }

    void UseItem(){
        sidebar.UseItem();
    }
}
