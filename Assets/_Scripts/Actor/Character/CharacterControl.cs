using UnityEngine;
using CustomInspector;
using TMPro;

// GAS (Game Ability System) : 언리얼.
// 32bit = 4byte ( int )
// 0000 .... 0000 0000
public class CharacterControl : MonoBehaviour, IActorControl
{
    [Header("Ability")]
    [HideInInspector] public AbilityControl abilityControl;

    [ReadOnly, SerializeField] private ActorProfile profile;
    public ActorProfile Profile { 
        get => profile;
        set => profile = value; 
    }
    
    [Header("Physics")]   
    [ReadOnly] public Rigidbody rb;
    [ReadOnly] public Animator animator;

    [ReadOnly] public Transform eyePoint;
    [ReadOnly] public Transform model;

    [Header("flag")]   
    [ReadOnly] public bool isGrounded;
    [ReadOnly] public bool isArrived = true;
    [ReadOnly] public TextMeshPro uiInfo;              //TextMeshPro : 3d object, 

    void Awake()
    {
        TryGetComponent(out abilityControl);
        TryGetComponent(out rb);
        TryGetComponent(out animator);
        

        model = transform.Find("_MODEL_").transform;

        uiInfo = GetComponentInChildren<TextMeshPro>();
    }

    void Start() { }

    void Update()
    {
        isGrounded = CheckGrounded();
    }
    
    bool CheckGrounded(){
        var ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        return Physics.Raycast(ray, 0.3f);
    }

    public void Visible(bool b){
        model.gameObject.SetActive(b);
    }

    public void PlayeAnimation(int hash, float duration = 0f, int layer = 0)
    {
        animator?.CrossFadeInFixedTime(hash, duration, layer, 0f);
    }

    public void Display(string info){
        if(uiInfo == null) return;

        uiInfo.text = info;
    }
}