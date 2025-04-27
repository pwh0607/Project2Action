using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class AbilityAttack : Ability<AbilityAttackData>
{
    bool isAttacking = false;
    private CancellationTokenSource cts;
    public AbilityAttack(AbilityAttackData data, CharacterControl owner) : base(data, owner) {
        if(owner.Profile == null) return;

        cts = new CancellationTokenSource(); 
    }

    public override void Activate(object obj)
    {   
        cts?.Dispose();
        cts = new();

        data.target = obj as CharacterControl;
        if (data.target == null)
            return;

        owner.uiControl.Display(data.Flag.ToString());
        data.eventAttackBefore.Register(OnEventAttackBefore);
    }

    public override void Deactivate()
    {
        data.eventAttackBefore.Unregister(OnEventAttackBefore);
        
        cts?.Cancel();
        cts?.Dispose();
    }

    
    private void OnEventAttackBefore(EventAttackBefore e){
        if(owner != e.from) return;

        Debug.Log("OnEventAttackBefore : 공격 시작!");
        data.eventAttackAfter.from = owner;
        data.eventAttackAfter.to = data.target;
        data.eventAttackAfter.Raise();
    }

    public override void Update()
    {
        if(isAttacking || data.target == null) return;

        CoolTimeAsync().Forget();
       
        AnimationClip clip = owner.Profile.ATTACK.Random();

        owner.AnimateTrigger("ATTACK", owner.Profile.animatorOverride, clip);
        owner.AnimateMoveSpeed(0f, true);
        data.eventAttackAfter.Raise();
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