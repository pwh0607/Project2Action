using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityPlayerAttack : Ability<AbilityPlayerAttackData>
{
    public AbilityPlayerAttack(AbilityPlayerAttackData data, CharacterControl owner) : base(data, owner) {
        if(owner.Profile == null) return;       
    }

    public override void Activate(object obj)
    {        
        if(owner.TryGetComponent<InputControl>(out var input)){
            input.actionInput.Player.Attack.performed += InputAttack;
        }
    }

    public override void Deactivate()
    {
        if(owner.TryGetComponent<InputControl>(out var input)){
            input.actionInput.Player.Attack.performed -= InputAttack;        
        }
    }

    void InputAttack(InputAction.CallbackContext context){
        if(context.performed){
            PerformAttack();
        }
    }

    void PerformAttack(){
        Debug.Log("공격!!");
        owner.PlayeAnimation(AnimationClipHashSet._ATTACK, owner.Profile.animatorOverride, 0.1f, 0);
        owner.AnimateMoveSpeed(0f, true);
    }
}