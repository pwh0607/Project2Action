using CustomInspector;
using TMPro;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    [ReadOnly] public Transform uiRoot;
    [SerializeField] TextMeshPro textMesh;

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

    public void Display(string info){
        if(textMesh == null) return;

        textMesh.text= info;
    }

}
