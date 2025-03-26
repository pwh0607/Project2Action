using System.Collections.Generic;
using CustomInspector;
using UnityEngine;
using UnityEngine.AI;

// GAS (Game Ability System) : 언리얼.
// 32bit = 4byte ( int )
// 0000 .... 0000 0000
public class CharacterControl : MonoBehaviour{
    [HideInInspector] public AbilityControl ability;
    public List<AbilityData> initialAbilities;
    [ReadOnly] public Rigidbody rb;
    [ReadOnly] public Animator animator;
    [ReadOnly] public bool isGrounded;
    [ReadOnly] public bool isLanding;
    void Awake()
    {
        TryGetComponent(out ability);
        TryGetComponent(out rb);
        TryGetComponent(out animator);
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
        isLanding = CheckLanding();
        InputKeyboard();
    }

    void InputKeyboard(){
        if(Input.GetButtonDown("Jump")) {
            ability.Activate(AbilityFlag.Jump);
        }
    }

    bool CheckGrounded(){
        var ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        return Physics.Raycast(ray, 0.3f);
    }

    bool CheckLanding(){
        var ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        return Physics.Raycast(ray, 0.6f);
    }
}