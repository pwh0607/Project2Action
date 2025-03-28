using System.Collections.Generic;
using CustomInspector;
using UnityEngine;
using UnityEngine.InputSystem;

// GAS (Game Ability System) : 언리얼.
// 32bit = 4byte ( int )
// 0000 .... 0000 0000
public class CharacterControl : MonoBehaviour{
    [Header("Ability")]
    [HideInInspector] public AbilityControl ability;
    public List<AbilityData> initialAbilities;

    [Header("애니메이션 컨트롤")]
    public int _MOVESPEED = Animator.StringToHash("MOVESPEED");
    public int _RUNTOSTOP = Animator.StringToHash("RUNTOSTOP");
    public int _JUMPUP = Animator.StringToHash("JUMPUP");
    public int _JUMPDOWN = Animator.StringToHash("JUMPDOWN");
    
    
    [Header("Physics")]   
    [ReadOnly] public Rigidbody rb;
    [ReadOnly] public Animator animator;
    public Transform cameraTarget;
    public Vector3 originalTargetPosition;
    public float fixedY = 0f;

    [Header("flag")]   
    [ReadOnly] public bool isGrounded;
    [ReadOnly] public bool isArrived = true;
    [ReadOnly] public bool isJumping = false;

    void Awake()
    {
        TryGetComponent(out ability);
        TryGetComponent(out rb);
        TryGetComponent(out animator);

        originalTargetPosition = cameraTarget.transform.localPosition;
    }

    void Start()
    {
        foreach( var data in initialAbilities){
            ability.Add(data, true);
        }
    }

    void FixedUpdate()
    {
        isGrounded = CheckGrounded();

        if(isJumping){
            cameraTarget.transform.position = new Vector3(transform.position.x, fixedY, transform.position.z);
        }else{
            cameraTarget.transform.localPosition = originalTargetPosition;
        }
    }
    
    public void FixPosition(){
        fixedY = cameraTarget.transform.position.y;
    }
    
    bool CheckGrounded(){
        var ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        return Physics.Raycast(ray, 0.3f);
    }

    #region Input System
    public void OnMoveKeyboard(InputAction.CallbackContext context){
        Debug.Log($"Move keyboard ...{context.phase}");
        if(context.performed){
            ability.Activate(AbilityFlag.Move, context);
        }
    }
    public void OnMoveMouse(InputAction.CallbackContext context){
        Debug.Log($"마우스 움직임...{context.phase}");

        if(context.performed){
            ability.Activate(AbilityFlag.Move, context);
        }
    }

    public void OnJumpKeyboard(InputAction.CallbackContext context){
        if(context.performed){
            ability.Activate(AbilityFlag.Jump, context);
        }
    }
    #endregion
}