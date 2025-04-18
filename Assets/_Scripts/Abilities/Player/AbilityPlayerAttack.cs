using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

public class AbilityPlayerAttack : Ability<AbilityPlayerAttackData>
{
    
    bool isAttacking = false;
    private CancellationTokenSource cts;
    
    public AbilityPlayerAttack(AbilityPlayerAttackData data, CharacterControl owner) : base(data, owner) {
        if(owner.Profile == null) return;       
    }

    public override void Activate(object obj)
    {        
        cts?.Dispose();
        cts = new();

        Debug.Log("Attack Activate");
        
        if(owner.TryGetComponent<InputControl>(out var input)){
            input.actionInput.Player.Attack.performed += InputAttack;
        }
    }

    public override void Deactivate()
    {
        if(owner.TryGetComponent<InputControl>(out var input)){
            input.actionInput.Player.Attack.performed -= InputAttack;        
        }
                
        cts?.Cancel();
        cts?.Dispose();
    }

    void InputAttack(InputAction.CallbackContext context){
        if(!context.performed || isAttacking) return;
        
        PerformAttack();
    }

    void PerformAttack(){
        CoolTimeAsync().Forget();
        AnimationClip clip = owner.Profile.ATTACK.Random();
        owner.AnimateTrigger("ATTACK", owner.Profile.animatorOverride, clip);
    }


    async UniTaskVoid CoolTimeAsync(){
        try{
            isAttacking = true;
            await UniTask.WaitForSeconds(owner.Profile.attackInterval, cancellationToken: cts.Token);
            isAttacking = false;
        }
        catch ( System.OperationCanceledException)      
        {
            //Debug.Log("쿨타임 취소");
        }
        catch ( System.Exception e )
        {
            Debug.LogException(e);
        }
    }
}