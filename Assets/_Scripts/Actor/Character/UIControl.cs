using UnityEngine;
using TMPro;
using CustomInspector;

public class UIControl : MonoBehaviour
{
    [ReadOnly] public Transform uiRoot;

//  TempCode
    [HorizontalLine("직접 연결 (없으면 미사용)"), HideInInspector] public bool h_s1;
    [SerializeField] TextMeshPro textMesh;
    [HorizontalLine(color:FixedColor.Cyan), HideInInspector] public bool h_e1;

    [SerializeField] SidebarController sidebarController;

    void Start()
    {
        uiRoot = transform.Find("_UI_");
        sidebarController = GetComponentInChildren<SidebarController>();
        
        if(uiRoot == null) {
            Debug.LogWarning($"{gameObject.name} : UIControl : _UI_ 없음");
            return;
        }
        
        uiRoot.localScale = Vector3.zero;
        Show(false);
    }

    public void Show(bool on){
        if(uiRoot == null) return;

        if(!on){
            uiRoot.localScale = Vector3.zero;
        }

        uiRoot.gameObject.SetActive(on);
    }

    [ReadOnly] public string state;

    public void Display(string info){
        if(textMesh == null) return;

        textMesh.text = info;
        state = info;
    }

    public void GetItem(Item item){
        if(sidebarController == null) return;
        
        sidebarController.GetItem(item);
    }
}