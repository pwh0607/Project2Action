using UnityEngine;
using CustomInspector;
using DG.Tweening;

//TempCode
public struct CharacterState
{
    public int health;
    public int damage;

    public void Set(ActorProfile profile){
        health = profile.health;
        damage = profile.attackDamage;
    }
}

// GAS (Game Ability System) : 언리얼.
// 32bit = 4byte ( int )
// 0000 .... 0000 0000
public class CharacterControl : MonoBehaviour, IActorControl
{
    [Header("Ability")]
    [ReadOnly] public UIControl uiControl;

    // 원본 데이터
    [ReadOnly] public AbilityControl abilityControl;
    // 인스턴스화 한 데이터
    public CharacterState state;
    
    [ReadOnly] public FeedbackControl feedbackControl;

    [ReadOnly, SerializeField] private ActorProfile profile;
    public ActorProfile Profile { 
        get => profile;
        set => profile = value; 
    }

    
    [Header("Physics")]   
    [ReadOnly] public Rigidbody rb;
    [ReadOnly] public Animator animator;
    [ReadOnly] public AnimationIKControl ik;

    [ReadOnly] public Transform eyePoint;
    [ReadOnly] public Transform model;

    [Header("flag")]   
    // 땅에 붙어 있는가?
    [ReadOnly] public bool isGrounded;

    //데미지를 받을 수 있는 상태
    [ReadOnly] public bool isDamageable = true;
    // 목적지 도착 상태.
    [ReadOnly] public bool isArrived = true;

    void Awake()
    {
        TryGetComponent(out abilityControl);
        TryGetComponent(out rb);
        TryGetComponent(out animator);
        TryGetComponent(out uiControl);
        
        eyePoint = transform.Find("_EYEPOINT_");
        model = transform.Find("_MODEL_");

        // Option : 있으면 쓰고, 없으면 무시.
        ik = GetComponent<AnimationIKControl>();
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

    Tween tweenrot;

    // 타겟을 바라본다 (Y축만 회전)
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

    public void AnimateTrigger(int hash, AnimatorOverrideController aoc, AnimationClip clip){
        if(animator == null) return;

        aoc[name] = clip;
        animator.runtimeAnimatorController = aoc;
        animator.SetTrigger(hash);
    }
    
    public void AnimateTrigger(string clipName, AnimatorOverrideController aoc, AnimationClip clip){
        if(animator == null) return;

        aoc[name] = clip;
        animator.runtimeAnimatorController = aoc;
        animator.SetTrigger(clipName);
    }

    public void PlayAnimation(int hash, float duration = 0f, int layer = 0)
    {
        if(animator == null) return;
        
        animator?.CrossFadeInFixedTime(hash, duration, layer, 0f);
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

    public void PlayAnimation(int hash, AnimatorOverrideController aoc, float animationSpeed, float duration = 0f, int layer = 0)
    {
        if(animator == null) return;
        
        animator?.CrossFadeInFixedTime(hash, duration, layer, 0f);
    }


    //immediate = true => 보간처리 없이 바로 애니메이션 수행.
    public void AnimateMoveSpeed(float speed, bool immediate = false){
        if(animator == null) return;

        float current = animator.GetFloat("MOVESPEED");
        float spd = Mathf.Lerp(current, speed, Time.deltaTime * 10f);
        animator.SetFloat("MOVESPEED", immediate ? speed : spd);
    }
}