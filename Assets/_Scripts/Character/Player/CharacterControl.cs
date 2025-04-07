using UnityEngine;
using CustomInspector;
using Project2Action;

// GAS (Game Ability System) : 언리얼.
// 32bit = 4byte ( int )
// 0000 .... 0000 0000
public class CharacterControl : MonoBehaviour
{
    [Header("Ability")]
    [HideInInspector] public AbilityControl ability;
    [ReadOnly] public ActorProfile profile;
    
    [Header("Physics")]   
    [ReadOnly] public Rigidbody rb;
    [ReadOnly] public Animator animator;


#region Animator HashSet
    [ReadOnly] public int _MOVESPEED = Animator.StringToHash("MOVESPEED");
    [ReadOnly] public int _RUNTOSTOP = Animator.StringToHash("RUNTOSTOP");
    [ReadOnly] public int _JUMPUP = Animator.StringToHash("JUMPUP");
    [ReadOnly] public int _JUMPDOWN = Animator.StringToHash("JUMPDOWN");
    [ReadOnly] public int _SPAWN = Animator.StringToHash("SPAWN");
#endregion

    [ReadOnly] public Transform eyePoint;
    [ReadOnly] public Transform model;

    [Header("flag")]   
    [ReadOnly] public bool isGrounded;
    [ReadOnly] public bool isArrived = true;
    [HideInInspector] public ActionGameInput actionInput;
    
    void Awake()
    {
        actionInput = new ActionGameInput();
        TryGetComponent(out ability);
        TryGetComponent(out rb);
        TryGetComponent(out animator);

        model = transform.Find("_MODEL_").transform;
    }

    void OnDestroy()
    {
        actionInput.Dispose();
    }
    
    void OnEnable()
    {
        actionInput.Enable();
    }
    
    void OnDisable()
    {
        actionInput.Disable();
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