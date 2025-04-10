using UnityEngine;

public class AbilityAttack : Ability<AbilityAttackData>
{
    private float attackSpeed;
    float elapsed = 0f;

    public AbilityAttack(AbilityAttackData data, CharacterControl owner) : base(data, owner) {
        if(owner.Profile == null) return;
        // 타겟 설정.
        
        attackSpeed = owner.Profile.interval;
    }

    public override void Activate(object obj)
    {        
        if(obj is CharacterControl control)
            data.target = control;

        owner.Display(data.Flag.ToString());
        owner.PerformAttackOnce(data.target.transform);
        elapsed = 0f;                                   // Activate되고나서 
    }

    public override void Deactivate()
    {

    }

    public override void Update()
    {
        if(data.target == null) return;

        elapsed -= Time.deltaTime;

        if(elapsed <= owner.Profile.interval){                            //1초에 한번씩 때린다.   
            owner.PerformAttackOnce(data.target.transform);
            elapsed = owner.Profile.interval;
        }

        owner.AnimateMoveSpeed(0f);
    }
}