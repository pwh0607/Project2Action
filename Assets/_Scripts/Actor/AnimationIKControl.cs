using UnityEngine;
using CustomInspector;

public class AnimationIKControl : MonoBehaviour
{
    [HorizontalLine("TARGET"), HideField] public bool _h0;
    public bool isTarget = false;
    [Tooltip("바라볼 타겟(Transform)")]
    public Transform target;


    private Animator animator;
    private float currentWeight;
    
    [Tooltip("전체 IK 수치")]
    [Range(0f,1f), SerializeField] float overallWeight; 

    [Tooltip("몸통(Spine ... Neck) IK 수치")]
    [Range(0f,1f), SerializeField] float bodyWeight;

    
    [Tooltip("머리 IK 수치")]
    [Range(0f,1f), SerializeField] float headWeight;
    
    void Start()
    {
        TryGetComponent(out animator);
    }

    void Update()
    {
        if(animator == null || target == null) return;
        
        // 타겟이 있으면, overall 값, 없으면 0
        float targetWeight = isTarget && target != null ? overallWeight : 0f;
        
        currentWeight = Mathf.MoveTowards(currentWeight, targetWeight, Time.deltaTime * 10f);
    }

    // Unity 예약함수 - IK 연산처리.
    void OnAnimatorIK(int layerIndex)
    {
        if(target == null || currentWeight <= 0.01f){           //노이즈 방지.
            animator.SetLookAtWeight(0);
            return;
        }

        animator.SetLookAtWeight(overallWeight, bodyWeight, headWeight);
        animator.SetLookAtPosition(target.position);
    }

    void OnAnimatorMove()
    {
        
    }
}
