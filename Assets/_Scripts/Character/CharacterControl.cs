using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using CustomInspector;
using Unity.Cinemachine;
using Project2Action;

// GAS (Game Ability System) : 언리얼.
// 32bit = 4byte ( int )
// 0000 .... 0000 0000
public class CharacterControl : MonoBehaviour
{
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

    //임시
    public CinemachineVirtualCameraBase mainCamera;

    public Transform cameraTarget;
    public Vector3 originalTargetPosition;
    public float fixedY = 0f;

    [Header("flag")]   
    [ReadOnly] public bool isGrounded;
    [ReadOnly] public bool isArrived = true;
    [ReadOnly] public bool isJumping = false;
    [HideInInspector] public ActionGameInput actionInput;

    // void OnDestroy()
    // {
    //     actionInput.Dispose();                              // Destroy asset object.
    // }

    // void OnEnable()
    // {
    //     actionInput.Enable();                                // Enable all actions within map.
    // }

    // void OnDisable()
    // {
    //     actionInput.Disable();                               // Disable all actions within map.
    // }

    // public void OnMove(InputAction.CallbackContext context){

    // }

    void Awake()
    {
        TryGetComponent(out ability);
        TryGetComponent(out rb);
        TryGetComponent(out animator);

        originalTargetPosition = cameraTarget.transform.localPosition;

        actionInput = new ActionGameInput();
        actionInput.Player.Move.performed += Context => Debug.Log($"인풋{Context.ReadValue<Vector2>()} , 아웃 풋");
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
    
    // public void FixPosition(){
    //     fixedY = cameraTarget.transform.position.y;
    // }
    
    bool CheckGrounded(){
        var ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        return Physics.Raycast(ray, 0.3f);
    }

    #region Input System
    public void OnMoveKeyboard(InputAction.CallbackContext context){
        if(context.performed){
            ability.Activate(AbilityFlag.Move);
        }
    }
    public void OnMoveMouse(InputAction.CallbackContext context){
        if(context.performed){
            ability.Activate(AbilityFlag.Move);
        }
    }

    public void OnJumpKeyboard(InputAction.CallbackContext context){
        if(context.performed){
            ability.Activate(AbilityFlag.Jump);
        }
    }
    #endregion

    #region LogicSystem
    
    #endregion
}