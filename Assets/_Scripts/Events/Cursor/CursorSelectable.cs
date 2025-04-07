using UnityEngine;

public class CursorSelectable : MonoBehaviour
{
    public CursorType type;
    public MeshRenderer meshRenderer;
    public void Select(bool on)
    {
        if(meshRenderer == null) return;
        
        string layername = on ? "Outline" : "Default";
        meshRenderer.gameObject.layer = LayerMask.NameToLayer(layername);
    }
}