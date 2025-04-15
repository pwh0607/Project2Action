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
        data.target = obj as CharacterControl;
        if (data.target == null)
            return;

        owner.uiControl.Display(data.Flag.ToString());
        Debug.Log("Attack Activate");
    }

    public override void Deactivate()
    {
        Debug.Log("Attack Deactivate");
    }

    public override void Update()
    {
        if(isAttacking || data.target == null) return;

        CoolTimeAsync().Forget();                 //순서 의미는?

        owner.LookAtY(data.target.transform.position);
        owner.Stop();
        AnimationClip clip = owner.Profile.ATTACK.Random();
        owner.PlayeAnimation("ATTACK", owner.Profile.animatorOverride, clip, owner.Profile.attackInterval, 0.1f, 0);
        owner.AnimateMoveSpeed(0f, true);
    }

    // Ability는 Monobehavouir가 아니기 때문에 코루틴은 사용할 수 없다.
    // 이럴 때는 async - Await 방식 사용
    async UniTaskVoid CoolTimeAsync(){
        try{
            isAttacking = true;
            await UniTask.WaitForSeconds(1f);
            isAttacking = false;
        }catch(System.Exception e){
            Debug.LogException(e);
        }
    }
}