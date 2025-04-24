using UnityEngine;

public class AbilityDamage : Ability<AbilityDamageData>
{
    public AbilityDamage(AbilityDamageData data, CharacterControl owner) : base(data, owner) {
        if(owner.Profile == null) return;

        owner.isDamageable = true;
        owner.uiControl?.Show(true);
        owner.uiControl.SetHealth(owner.Profile.health, owner.Profile.health);
    }

    public override void Activate(object obj)
    {     
        var e = obj as EventAttackAfter;
        if(e == null){
            Debug.LogWarning("AbilityDamage ] EventAttackAfter 형변환 실패...");
            return;
        } 
    }


    public override void Deactivate()
    {
        owner.uiControl?.Show(false);
        owner.abilityControl.RemoveAll();
    }

    public override void Update()
    {

    }
}