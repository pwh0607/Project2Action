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
        

        PoolManager.I.Spawn(e.feedbackFloatingText, owner.eyePoint.position, Quaternion.identity, null);
        e.feedbackFloatingText.SetText(e.damage.ToString());

        owner.feedbackControl?.PlayImpact();

        Vector3 rndsphere = Random.insideUnitSphere;
        rndsphere.y = 0f;

        Vector3 rndpos = rndsphere * 0.5f + owner.eyePoint.position;

        var floating = PoolManager.I.Spawn(e.particleHit, rndpos, Quaternion.identity, null) as PoolableFeedback;
        if(floating != null)
            floating.SetText($"{e.damage}");

        owner.state.health -= e.damage;
        owner.uiControl.SetHealth(owner.state.health, owner.Profile.health);
        
        if(owner.state.health <= 0){
            
            data.eventDeath.target = owner;
            data.eventDeath.Raise();
        }
    }


    public override void Deactivate()
    {        
        // owner.isDamageable = false;
        owner.uiControl?.Show(false);
        owner.abilityControl.RemoveAll();
    }

    public override void Update()
    {

    }
}