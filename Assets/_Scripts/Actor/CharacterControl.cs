using UnityEngine;
using CustomInspector;
using DG.Tweening;

// GAS (Game Ability System) : 언리얼.
// 32bit = 4byte ( int )
// 0000 .... 0000 0000
public class CharacterControl : MonoBehaviour, IActorControl
{
    [Header("Ability")]
    public Transform playerHand;
    public LockedGate detectedGate;
    
    // 원본 데이터
    [ReadOnly] public AbilityControl abilityControl;
    [ReadOnly] public UIControl uiControl;
    [ReadOnly] public FeedbackControl feedbackControl;
    
    [ReadOnly] public GameObject interActiveObject;

    [ReadOnly, SerializeField] private ActorProfile profile;
    public ActorProfile Profile { 
        get => profile;
        set => profile = value; 
    }

    [Header("flag")]   
    [ReadOnly] public bool isGrounded;
    [ReadOnly] public bool isDamageable = false;

    [ReadOnly] public bool isArrived = true;

    [Header("Physics")]   
    [ReadOnly] public Rigidbody rb;
    [ReadOnly] public Animator animator;

    [ReadOnly] public Transform eyePoint;
    [ReadOnly] public Transform handPoint;
    [ReadOnly] public Transform model;

    void Awake()
    {
        TryGetComponent(out abilityControl);
        TryGetComponent(out rb);
        TryGetComponent(out animator);
        
        eyePoint = transform.Find("_EYEPOINT_");
        model = transform.Find("_MODEL_");
        handPoint = transform.Find("_HANDPOINT_");

        uiControl = GetComponentInChildren<UIControl>();
    }

    void Start() { 
        interActiveObject = null;
    }

    void Update()
    {
        isGrounded = CheckGrounded();
    }

    bool CheckGrounded(){
        var ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        return Physics.Raycast(ray, 0.3f);
    }

    Tween tweenrot;
    public void LookAtY(Vector3 target){
        if(tweenrot != null || tweenrot.IsPlaying()) return;

        Vector3 direction = target - transform.position;
        direction.y = 0f;

        Vector3 euler = Quaternion.LookRotation(direction.normalized).eulerAngles;
        Quaternion rot = Quaternion.Euler(euler);
        
        transform.DORotateQuaternion(rot, 0.2f).SetEase(Ease.OutSine);
    }

    public void Stop(){
        isArrived = true;
        rb.linearVelocity = Vector3.zero;
    }

    public void Visible(bool b){
        model.gameObject.SetActive(b);
    }

    
    public void AnimateTrigger(string clipName, AnimatorOverrideController aoc, AnimationClip clip){
        if(animator == null) return;

        aoc[name] = clip;
        animator.runtimeAnimatorController = aoc;
        animator.SetTrigger(clipName);
    }

    public void PlayAnimation(string clipName, float duration = 0f, int layer = 0)
    {
        if(animator == null) return;
        
        animator?.CrossFadeInFixedTime(clipName, duration, layer, 0f);
    }

    public void PlayAnimation(string clipName, AnimatorOverrideController aoc, AnimationClip clip, float animationSpeed, float duration = 0f, int layer = 0)
    {
        if(animator == null) return;

        aoc[clipName] = clip;
        animator.runtimeAnimatorController = aoc;
        animator?.CrossFadeInFixedTime(clipName, duration, layer, 0f);
    }
    
    public void AnimateMoveSpeed(float speed, bool immediate = false){
        if(animator == null) return;

        float current = animator.GetFloat("MOVESPEED");
        float spd = Mathf.Lerp(current, speed, Time.deltaTime * 10f);
        animator.SetFloat("MOVESPEED", immediate ? speed : spd);
    }

    public void InterActItem(){
        
    }
}