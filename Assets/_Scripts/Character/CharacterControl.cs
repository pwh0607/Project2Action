using UnityEngine;

using CustomInspector;
using Unity.Cinemachine;
using Project2Action;
using System.Collections;

// GAS (Game Ability System) : 언리얼.
// 32bit = 4byte ( int )
// 0000 .... 0000 0000
public class CharacterControl : MonoBehaviour
{
    [Header("Ability")]
    [HideInInspector] public AbilityControl ability;
    [ReadOnly] ActorProfile profile;
    
    
    [Header("Physics")]   
    [ReadOnly] public Rigidbody rb;
    [ReadOnly] public Animator animator;

#region Animator HashSet
    [HideInInspector] public int _MOVESPEED = Animator.StringToHash("MOVESPEED");
    [HideInInspector] public int _RUNTOSTOP = Animator.StringToHash("RUNTOSTOP");
    [HideInInspector] public int _JUMPUP = Animator.StringToHash("JUMPUP");
    [HideInInspector] public int _JUMPDOWN = Animator.StringToHash("JUMPDOWN");
    [HideInInspector] public int _SPAWN = Animator.StringToHash("STANDUP");
#endregion

    public Vector3 originalTargetPosition;
    public float fixedY = 0f;
    [ReadOnly] public Transform eyePoint;
    [ReadOnly] public Transform model;

    [Header("flag")]   
    [ReadOnly] public bool isGrounded;
    [ReadOnly] public bool isArrived = true;
    [ReadOnly] public bool isJumping = false;
    [HideInInspector] public ActionGameInput actionInput;
    
    void Awake()
    {
        TryGetComponent(out ability);
        TryGetComponent(out rb);
        TryGetComponent(out animator);
        
        model = GameObject.Find("_MODEL_").transform;
        
        actionInput = new ActionGameInput();
    }

    void Start()
    {
        Visible(false); 
    }

    void FixedUpdate()
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

    public void PlayeAnimation(int hash, float duration, int layer = 0)
    {
        animator?.CrossFadeInFixedTime(hash, duration, layer, 0f);
    }
}