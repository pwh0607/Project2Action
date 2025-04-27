using System.Collections;
using System.Linq;
using UnityEngine;

public class CharacterEventControl : MonoBehaviour
{
#region Events
    [SerializeField] EventCameraSwitch eventCameraSwitch;
    [SerializeField] EventPlayerSpawnAfter eventPlayerSpawnAfter;
    [SerializeField] EventDeath eventDeath;
    [SerializeField] EventAttackAfter eventAttackAfter;

    [SerializeField] EventSensorSightEnter eventSensorSightEnter;
    [SerializeField] EventSensorSightExit eventSensorSightExit;

    [SerializeField] EventCursorHover eventCursorHover;
#endregion
    
    private CharacterControl owner;
    void Start()
    {
        if(!TryGetComponent(out owner)) Debug.LogWarning("GameEventControl - controllerControl 없음...");
        owner.Visible(false);
    }

    void OnEnable()
    {
        eventPlayerSpawnAfter.Register(OnEventPlayerSpawnAfter);
        eventCameraSwitch.Register(OnEventCameraSwitch);
        eventAttackAfter.Register(OnEventAttackAfter);
        eventDeath.Register(OnEventDeath);

        eventSensorSightEnter.Register(OnEventSensorSightEnter);
        eventSensorSightExit.Register(OnEventSensorSightExit);
    }

    void OnDisable()
    {
        eventPlayerSpawnAfter.Unregister(OnEventPlayerSpawnAfter);
        eventCameraSwitch.Unregister(OnEventCameraSwitch);
        eventAttackAfter.Unregister(OnEventAttackAfter);
        eventDeath.Unregister(OnEventDeath);
        
        eventSensorSightEnter.Unregister(OnEventSensorSightEnter);
        eventSensorSightExit.Unregister(OnEventSensorSightExit);
    }

    void OnEventCameraSwitch(EventCameraSwitch e){
        if(e.inout)
            owner.abilityControl.Deactivate(AbilityFlag.MoveKeyboard);
        else
            owner.abilityControl.Activate(AbilityFlag.MoveKeyboard, false, null);
    }

    void OnEventPlayerSpawnAfter(EventPlayerSpawnAfter e){
        StartCoroutine(SpawnSequence(e));
    }
    
    IEnumerator SpawnSequence(EventPlayerSpawnAfter e){
        Debug.Log("Spawn Player start");
        yield return new WaitUntil(() => owner.Profile.avatar != null && owner.Profile.models != null);

        Debug.Log("Spawn Player start");
        owner.Profile = owner.Profile;

        if(owner.Profile.models == null)
            Debug.LogError("CharacterEventControl ] model 없음.");

        var model = owner.Profile.models.Random();

        var clone = Instantiate(model, owner.model);

        clone.GetComponentsInChildren<SkinnedMeshRenderer>().ToList().ForEach( m =>{
            m.gameObject.layer = LayerMask.NameToLayer("Silhouette");
        });

        if(owner.Profile.avatar == null)
            Debug.LogError("CharacterEventControl ] avatar 없음.");
            
        owner.animator.avatar = owner.Profile.avatar;

        yield return new WaitForSeconds(1f);

        owner.Visible(true);
        owner.PlayAnimation("SPAWN", 0f);

        PoolManager.I.Spawn(e.spawnParticle, transform.position, Quaternion.identity, null);

        owner.uiControl.Show(true);
        
        yield return new WaitForSeconds(1f);
    
        foreach(var dat in owner.Profile.abilities)
            owner.abilityControl.Add(dat, true);
    }

    #region DAMAGE
    void OnEventAttackAfter(EventAttackAfter e){
        if(owner != e.to) return;
        owner.abilityControl.Activate(AbilityFlag.Damage, false, e);             
    }
    #endregion

    #region Death
    void OnEventDeath(EventDeath e){
        if(owner != e.target) return;
        
        owner.PlayAnimation("DEATH", 0.2f);
        owner.abilityControl.RemoveAll();
    }
    #endregion


    #region Sight
    void OnEventSensorSightEnter(EventSensorSightEnter e){
        if(owner != e.from) return;
    }

    void OnEventSensorSightExit(EventSensorSightExit e){
        if(owner != e.from) return;
    }
    #endregion
}