using UnityEngine;

public class AbilityDeath : Ability<AbilityDeathData>
{
    public AbilityDeath(AbilityDeathData data, CharacterControl owner) : base(data, owner) {
        if(owner.Profile == null) return;

        owner.isDeath = false;
    }

    public override void Activate(object obj)
    {
        data.eventDeath.Register(OnEventDeath);
    }


    public override void Deactivate()
    {
        data.eventDeath.Unregister(OnEventDeath);
    }

    void OnEventDeath(EventDeath e){
        owner.abilityControl.RemoveAll();
        owner.PlayAnimation("Death");
    }
}