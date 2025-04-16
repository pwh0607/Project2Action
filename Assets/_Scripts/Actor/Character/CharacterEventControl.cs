using System.Collections;
using System.Linq;
using UnityEngine;

public class CharacterEventControl : MonoBehaviour
{
#region Events
    [SerializeField] EventCameraSwitch eventCameraSwitch;
    [SerializeField] EventPlayerSpawnAfter eventPlayerSpawnAfter;
    [SerializeField] EventAttackAfter eventAttackAfter;
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
    }

    void OnDisable()
    {
        eventPlayerSpawnAfter.Unregister(OnEventPlayerSpawnAfter);
        eventCameraSwitch.Unregister(OnEventCameraSwitch);
        eventAttackAfter.Unregister(OnEventAttackAfter);
    }

    //Temp
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Backspace))
            owner.state.health = 0;
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
        yield return new WaitUntil(() => owner.Profile.avatar != null && owner.Profile.models != null);

        owner.Profile = owner.Profile;

        // 플레이어 모델 생성후 하위 항목인 Model에 설정
        if(owner.Profile.models == null)
            Debug.LogError("CharacterEventControl ] model 없음.");

        var model = owner.Profile.models.Random();

        var clone = Instantiate(model, owner.model);

        var feedback = clone.GetComponentInChildren<FeedbackControl>();
        if(feedback != null){
            owner.feedbackControl = feedback;
        }

        clone.GetComponentsInChildren<SkinnedMeshRenderer>().ToList().ForEach( m =>{
            m.gameObject.layer = LayerMask.NameToLayer("Silhouette");
        });

        if(owner.Profile.avatar == null)
            Debug.LogError("CharacterEventControl ] avatar 없음.");
        owner.animator.avatar = owner.Profile.avatar;

        yield return new WaitForSeconds(1f);

        owner.Visible(true);
        owner.PlayeAnimation(AnimationClipHashSet._SPAWN, 0f);

        PoolManager.I.Spawn(e.spawnParticle, transform.position, Quaternion.identity, null);
    
        yield return new WaitForSeconds(1f);

        foreach( var dat in owner.Profile.abilities)
            owner.abilityControl.Add(dat, true);
    }

    #region DAMAGE
    void OnEventAttackAfter(EventAttackAfter e){
        if(owner != e.to) return;              // e.to는 Player .. 피격자[공격을 받는 사람은]

        // 플레이어 damage를 입다.
        // object : damage값.
        owner.abilityControl.Activate(AbilityFlag.Damage, false, e);             //일반 공격의 경우에는 맞으면서 움직일 수 있다.

        PoolManager.I.Spawn(e.feedbackFloatingText, owner.eyePoint.position, Quaternion.identity, null);
        e.feedbackFloatingText.SetText(e.damage.ToString());

        Vector3 rndsphere = Random.insideUnitSphere;
        rndsphere.y = 0f;

        Vector3 rndpos = rndsphere * 0.5f + owner.eyePoint.position;

        var floating = PoolManager.I.Spawn(e.particleHit, rndpos, Quaternion.identity, null) as PoolableFeedback;
        if(floating != null)
            floating.SetText($"{e.damage}");

        // 데미지 ui 갱신
        owner.state.health -= e.damage;
        owner.uiControl.SetHealth(owner.state.health, owner.Profile.health);
    }
    #endregion

}

// 비동기(async)
/*
    1. 코루틴   
    2. Invoke
    3. async / await [UniTask]
    4. Awaitable
    5. CySharp - Unitask
    6. DoTween - DoVirtual.Delay(3f, () => {...})
*/
// 유니티는 단일 스레드 엔진이다.
// => 비동기라고 표현하지만 사실방 코어가 빠르게 번갈아가며 처리하는 것! [병렬 수행하는 것처럼 보이게 하는 것.]