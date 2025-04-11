using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class AbilityAttack : Ability<AbilityAttackData>
{
    bool isAttacking = false;
    CancellationTokenSource cts;
    public AbilityAttack(AbilityAttackData data, CharacterControl owner) : base(data, owner) {
        if(owner.Profile == null) return;

        cts = new();
    }

    public override void Activate(object obj)
    {        
        if(obj is CharacterControl control)
            data.target = control;

        owner.Display(data.Flag.ToString());
    }

    public override void Deactivate()
    {

    }

    public override void Update()
    {
        if(isAttacking || data.target == null) return;

        CoolTimeAsync().Forget();                 //순서 의미는?

        owner.LookAtY(data.target.transform.position);
        AnimationClip clip = owner.Profile.ATTACK.Random();
        float anispd = owner.Profile.attackInterval;
        owner.PlayeAnimation("ATTACK", owner.Profile.animatorOverride, clip, anispd, 0.1f, 0);
        owner.AnimateMoveSpeed(0f, true);
    }


    // Ability는 Monobehavouir가 아니기 때문에 코루틴은 사용할 수 없다.
    // 이런땐 async - Await 방식 사용
    async UniTaskVoid CoolTimeAsync(){
        try{
            isAttacking = true;
            await UniTask.WaitForSeconds(owner.Profile.attackInterval);
            isAttacking = false;
        }catch(System.Exception e){
            Debug.LogException(e);
        }
    }
}