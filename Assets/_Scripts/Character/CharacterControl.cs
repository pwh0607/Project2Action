using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public List<AbilityData> initialAbilities;

    [Header("애니메이션 컨트롤")]
    public int _MOVESPEED = Animator.StringToHash("MOVESPEED");
    public int _RUNTOSTOP = Animator.StringToHash("RUNTOSTOP");
    public int _JUMPUP = Animator.StringToHash("JUMPUP");
    public int _JUMPDOWN = Animator.StringToHash("JUMPDOWN");
    public int _SPAWN = Animator.StringToHash("Spawn");
        
    [Header("Physics")]   
    [ReadOnly] public Rigidbody rb;
    [ReadOnly] public Animator animator;

    //임시
    public CinemachineVirtualCameraBase mainCamera;

    public Transform cameraTarget;
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
        originalTargetPosition = cameraTarget.transform.localPosition;
        
        actionInput = new ActionGameInput();
    }

    void Start()
    {
        // Visible(false);      //test

        #region TMPCode
        foreach( var data in initialAbilities){
            ability.Add(data, true);
        }
        #endregion
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

    public void Visible(bool b){
        model.gameObject.SetActive(b);
    }

    public void PlayeAnimation(int hash, float duration, int layer = 0)
    {
        animator?.CrossFadeInFixedTime(hash, duration, layer, 0f);
    }

    IEnumerator SpawnSequence(EventPlayerSpawnAfter e){
        yield return new WaitForSeconds(1f);
        PoolManager.I.Spawn(e.spawnParticle, transform.position, Quaternion.identity, transform);
        yield return new WaitForSeconds(0.3f);
        Visible(true);
        PlayeAnimation(_SPAWN, 0f);
    }
}