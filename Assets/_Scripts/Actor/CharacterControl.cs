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
        
        eyePoint = transform.Find("_EYEPOINT_");
        model = transform.Find("_MODEL_");
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

    // 타겟을 바라본다 (Y축만 회전)
    public void LookAtY(Vector3 target){
        Vector3 direction = target - transform.position;
        direction.y = 0f;

        Vector3 euler = Quaternion.LookRotation(direction.normalized).eulerAngles;
        transform.rotation = Quaternion.Euler(euler);
    }

    public void Stop(){
        isArrived = true;
        rb.linearVelocity = Vector3.zero;
    }

    public void Visible(bool b){
        model.gameObject.SetActive(b);
    }

    public void PlayeAnimation(int hash, float duration = 0f, int layer = 0)
    {
        if(animator == null) return;
        
        animator?.CrossFadeInFixedTime(hash, duration, layer, 0f);
    }


    public void PlayeAnimation(string clipName, AnimatorOverrideController aoc, AnimationClip clip, float animationSpeed, float duration = 0f, int layer = 0)
    {
        if(animator == null) return;

        aoc[clipName] = clip;
        animator.runtimeAnimatorController = aoc;
        animator?.CrossFadeInFixedTime(clipName, duration, layer, 0f);
    }

    public void Display(string info){
        if(uiInfo == null) return;

        uiInfo.text = info;
    }

    //immediate = true => 보간처리 없이 바로 애니메이션 수행.
    public void AnimateMoveSpeed(float speed, bool immediate = false){
        if(animator == null) return;

        float current = animator.GetFloat(AnimationClipHashSet._MOVESPEED);
        float spd = Mathf.Lerp(current, speed, Time.deltaTime * 10f);
        animator.SetFloat(AnimationClipHashSet._MOVESPEED, immediate ? speed : spd);
    }

    public void PerformAttackOnce(Transform target){ 
        // LookAtY(target.transform.position);
        PlayeAnimation(AnimationClipHashSet._ATTACK, 0.2f);
    }

}