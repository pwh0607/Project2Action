using System.Collections.Generic;
using UnityEngine;

// GAS (Game Ability System) : 언리얼.
// 32bit = 4byte ( int )
// 0000 .... 0000 0000
public class CharacterControl : MonoBehaviour{
    [HideInInspector] public AbilityControl ability;
    public List<AbilityData> initialAbilities;
    public bool isGrounded;
    public Rigidbody rb;

    void Awake()
    {
        if(TryGetComponent(out ability) == false){
            Debug.LogWarning("CharacterControl : AblityControl 없음");
        }
        if(TryGetComponent(out rb) == false){
            Debug.LogWarning("rigidbody 없음");
        }
    }

    void Start()
    {
        foreach( var dat in initialAbilities){
            ability.Add(dat);
        }
    }

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position + Vector3.up, -transform.up, 1f);
        InputKeyboard();
    }

    void InputKeyboard(){
        if(Input.GetButtonDown("Jump")) ability.Trigger(AbilityFlag.Jump);
    }
}