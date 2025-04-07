using CustomInspector;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [Header("Ability")]
    [HideInInspector] public AbilityControl ability;
    [ReadOnly] public ActorProfile profile;
    
    [Header("Physics")]   
    [ReadOnly] public Rigidbody rb;
    [ReadOnly] public Animator animator;

    [ReadOnly] public Transform eyePoint;
    [ReadOnly] public Transform model;

    [Header("flag")]
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
