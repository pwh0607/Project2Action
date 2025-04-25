using CustomInspector;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
#region EVENTS
    [HorizontalLine("EVENTS"),HideField] public bool _h0;

    [SerializeField] EventSensorItemEnter eventSensorItemEnter;
    [SerializeField] EventSensorItemExit eventSensorItemExit;
    
    [Space(10), HorizontalLine(color:FixedColor.Cyan),HideField] public bool _h1;
#endregion

    public Renderer[] rds;
    [Tooltip("Outline Material")]
    public Material selectableMaterial;

    [Tooltip("Outline Thickness")]
    public float selectableThickness = 0.05f;

    [ReadOnly] public bool on;

    void Start()
    {
        SetupRenderer();
    }

    void OnEnable(){
        eventSensorItemEnter?.Register(OnEventSensorItemEnter);
        eventSensorItemExit?.Register(OnEventSensorItemExit);
    }

    void OnDisable(){
        eventSensorItemEnter?.Unregister(OnEventSensorItemEnter);
        eventSensorItemExit?.Unregister(OnEventSensorItemExit);
    }

    void OnEventSensorItemEnter(EventSensorItemEnter e){
        if(e.to != this.gameObject || e.to.GetComponent<InteractiveObject>() == null) return;

        Select(true);
    }

    void OnEventSensorItemExit(EventSensorItemExit e){
        if(e.to != this.gameObject || e.to.GetComponent<InteractiveObject>() == null) return;

        Select(false);
    }

    void Select(bool on)
    {
        if(rds == null || rds.Length <=0) return;

        foreach(var rd in rds){
            string layername = on ? "Outline" : "Default";
            
            this.on = on;
            rd.gameObject.layer = LayerMask.NameToLayer(layername);

            if(selectableMaterial != null)
                selectableMaterial.SetFloat("_Thickness", selectableThickness);
        }
    }

    public void SetupRenderer(){
        if(rds.Length > 0) return;

        rds = GetComponentsInChildren<SkinnedMeshRenderer>();

        if(rds.Length <= 0)
            rds = GetComponentsInChildren<MeshRenderer>();
    }
}