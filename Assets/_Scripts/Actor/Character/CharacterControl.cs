using UnityEngine;
using CustomInspector;
using Project2Action;

// GAS (Game Ability System) : 언리얼.
// 32bit = 4byte ( int )
// 0000 .... 0000 0000
public class CharacterControl : MonoBehaviour, IActorControl
{
    [Header("Ability")]
    [HideInInspector] public AbilityControl ability;
    
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

    void Awake()
    {
        TryGetComponent(out ability);
        TryGetComponent(out rb);
        TryGetComponent(out animator);

        model = transform.Find("_MODEL_").transform;
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
        Debug.Log($"anim : {hash.GetType()}");
        animator?.CrossFadeInFixedTime(hash, duration, layer, 0f);
    }

    public void PlayeAnimation(string state, float duration = 0f, int layer = 0)
    {
        Debug.Log($"anim : {state}");
        animator?.CrossFadeInFixedTime(state, duration, layer, 0f);
    }
}