using System.Collections;
using System.Linq;
using UnityEngine;

public class CharacterEventControl : MonoBehaviour
{
#region Events
    [SerializeField] EventCameraSwitch eventCameraSwitch;
    [SerializeField] EventPlayerSpawnAfter eventPlayerSpawnAfter;
#endregion
    
    private CharacterControl controller;
    void Start()
    {
        if(!TryGetComponent(out controller)) Debug.LogWarning("GameEventControl - controllerControl 없음...");
        controller.Visible(false);
    }

    void OnEnable()
    {
        eventPlayerSpawnAfter.Register(OnEventPlayerSpawnAfter);
        eventCameraSwitch.Register(OnEventCameraSwitch);
    }

    void OnDisable()
    {
        eventPlayerSpawnAfter.UnRegister(OnEventPlayerSpawnAfter);
        eventCameraSwitch.UnRegister(OnEventCameraSwitch);
    }


    void OnEventCameraSwitch(EventCameraSwitch e){
        if(e.inout)
            controller.ability.Deactivate(AbilityFlag.MoveKeyboard);
        else
            controller.ability.Activate(AbilityFlag.MoveKeyboard);
    }

    void OnEventPlayerSpawnAfter(EventPlayerSpawnAfter e){
        StartCoroutine(SpawnSequence(e));
    }
    
    IEnumerator SpawnSequence(EventPlayerSpawnAfter e){
        yield return new WaitUntil(() => e.actorProfile.avatar != null && e.actorProfile.model != null);

        controller.Profile = e.actorProfile;

        // 플레이어 모델 생성후 하위 항목인 Model에 설정
        if(e.actorProfile.model == null)
            Debug.LogError("CharacterEventControl ] model 없음.");

        var clone = Instantiate(e.actorProfile.model, controller.model);

        clone.GetComponentsInChildren<SkinnedMeshRenderer>().ToList().ForEach( m =>{
            m.gameObject.layer = LayerMask.NameToLayer("Silhouette");
        });

        if(e.actorProfile.avatar == null)
            Debug.LogError("CharacterEventControl ] avatar 없음.");
        controller.animator.avatar = e.actorProfile.avatar;

        yield return new WaitForSeconds(1f);

        controller.Visible(true);
        controller.PlayeAnimation(AnimationClipHashSet._SPAWN, 0.05f);

        PoolManager.I.Spawn(e.spawnParticle, transform.position, Quaternion.identity, null);
    
        yield return new WaitForSeconds(3f);

        foreach( var dat in e.actorProfile.abilities )
            controller.ability.Add(dat, true);
    }
}

// 비동기(async)
/*
    1. 코루틴   
    2. Invoke
    3. async / await
    4. Awaitable
    5. CySharp - Unitask
    6. DoTween - DoVirtual.Delay(3f, () => {...})
*/
// 유니티는 단일 스레드 엔진이다.
// => 비동기라고 표현하지만 사실방 코어가 빠르게 번갈아가며 처리하는 것! [병렬 수행하는 것처럼 보이게 하는 것.]