using CustomInspector;
using UnityEngine;

public class CursorSelectable : MonoBehaviour
{
    public CursorType type;
    public Renderer rd;               //Renderer - MeshRenderer
    
    [Tooltip("Outline Material")]
    public Material selectableMaterial;

    [Tooltip("Outline Thickness")]
    public float selectableThickness = 0.05f;

    [ReadOnly] public bool on;
    
    void Update()
    {
        
    }

    public void SetupRenderer(){
        if(rd != null) return;

        rd = GetComponentInChildren<SkinnedMeshRenderer>();

        if(rd == null){
            rd = GetComponentInChildren<MeshRenderer>();
        }
    }

    public void Select(bool on)
    {
        if(rd == null) return;
        
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