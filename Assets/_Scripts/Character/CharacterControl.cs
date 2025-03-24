using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

// GAS (Game Ability System) : 언리얼.
// 32bit = 4byte ( int )
// 0000 .... 0000 0000
public class CharacterControl : MonoBehaviour{
    [HideInInspector] public AbilityControl ability;
    public List<AbilityData> initialAbilities;
    [ReadOnly] public CharacterController characterController;
    [ReadOnly] public bool isGrounded;
    void Awake()
    {
        TryGetComponent(out ability);
        TryGetComponent(out characterController);
    }

    void Start()
    {
        foreach( var dat in initialAbilities){
            ability.Add(dat, true);
        }
    }

    void Update()
    {
        isGrounded = characterController.isGrounded;
        InputKeyboard();
    }

    void InputKeyboard(){
        if(Input.GetButtonDown("Jump")) {
            ability.Activate(AbilityFlag.Jump);
        }
    }
}