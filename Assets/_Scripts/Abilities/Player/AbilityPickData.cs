using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Pick")]
public class AbilityPickData : AbilityData
{
    public override AbilityFlag Flag => AbilityFlag.Pick;
    
    [SerializeField] public EventSensorItemEnter eventSensorItemEnter;
    [SerializeField] public EventSensorItemExit eventSensorItemExit;
    public LayerMask layerMask;
    
    public override Ability CreateAbility(CharacterControl owner) => new AbilityPick(this, owner);

    public float pickRange;
    public Vector3 pickOffset;
}