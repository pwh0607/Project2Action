using Cysharp.Threading.Tasks;
using UnityEngine;

public class AbilityDeath : Ability<AbilityDeathData>
{
    public AbilityDeath(AbilityDeathData data, CharacterControl owner) : base(data, owner) {
        if(owner.Profile == null) return;

        owner.isDeath = false;
    }

    public override void Activate(object obj)
    {
        data.eventAttackAfter.Register(OnEventDeath);
    }

    public override void Deactivate()
    {
        data.eventAttackAfter.Unregister(OnEventDeath);
    }

    void OnEventDeath(EventAttackAfter e){
        Debug.Log("공격 받음");
        owner.abilityControl.RemoveAll();
        owner.PlayAnimation("DEATH");
        owner.isDeath = true;

        UIShowTimeAsync(1.5f).Forget();
    }
    
    async UniTaskVoid UIShowTimeAsync(float time){
        try{
            await UniTask.WaitForSeconds(3f);
            GameManager.I.GameOver();
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