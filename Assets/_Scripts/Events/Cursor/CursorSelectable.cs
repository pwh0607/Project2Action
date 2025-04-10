using System.Linq;
using CustomInspector;
using UnityEngine;

public class CursorSelectable : MonoBehaviour
{
    public CursorType type;
    public Renderer rd;               //Renderer - MeshRenderer
    public Renderer[] rds;
    [Tooltip("Outline Material")]
    public Material selectableMaterial;

    [Tooltip("Outline Thickness")]
    public float selectableThickness = 0.05f;

    [ReadOnly] public bool on;
    

    //메쉬 합성하기. => 제거 예정.
    #region MeshCombine
    // 메쉬 컴바인 참조.
    #endregion
    
    void Update()
    {
        
    }

    public void SetupRenderer(){
        if(rds.Length <=0 ) return;

        rds = GetComponentsInChildren<SkinnedMeshRenderer>();
        if(rds == null){
            rds = GetComponentsInChildren<MeshRenderer>();
        }

        Transform meshT = transform.Find("STANDARDMESH");

        if(meshT == null) {
            Debug.Log("메쉬를 찾지 못했다.");
            return;
        }

        rd = GetComponentInChildren<SkinnedMeshRenderer>();

        if(rd == null){
            rd = GetComponentInChildren<MeshRenderer>();
        }
    }

    public void Select(bool on)
    {
        if(rd == null){
            Debug.Log("렌더러를 찾지 못했다.");
            return;
        } 
        string layername = on ? "Outline" : "Default";
        
        //Debugging
        if(on){
            Debug.Log($"{gameObject.name}");
        }
        
        this.on = on;
        rd.gameObject.layer = LayerMask.NameToLayer(layername);

        if(selectableMaterial != null)
            selectableMaterial.SetFloat("_Thickness", selectableThickness);
    }
}