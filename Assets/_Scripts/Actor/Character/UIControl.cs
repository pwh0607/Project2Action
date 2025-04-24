using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CustomInspector;
using UnityEditorInternal.Profiling.Memory.Experimental;

public class UIControl : MonoBehaviour
{
    [ReadOnly] public Transform uiRoot;

//  TempCode
    [HorizontalLine("직접 연결 (없으면 미사용)"), HideInInspector] public bool h_s1;
    [SerializeField] TextMeshPro textMesh;
    [SerializeField] Slider sliderHealth;
    [HorizontalLine(color:FixedColor.Cyan), HideInInspector] public bool h_e1;
//  TempCode

    [SerializeField] SidebarController sidebarController;

    void Start()
    {
        uiRoot = transform.Find("_UI_");
        if(uiRoot == null) Debug.LogWarning("UIControl : _UI_ 없음");

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

    public void SetHealth(int current, int max){
        if(sliderHealth == null) return;

        float cur = (float)current / (float)max;
        sliderHealth.value = Mathf.Clamp01(cur);
    }

    public void GetItem(Item item){
        sidebarController.GetItem(item);
    }
}