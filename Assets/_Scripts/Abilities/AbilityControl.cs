using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

// abilityDatas : 외부에서 능력 부여/회수 인터페이스
// abilities : abilityDatas 갱신해서 행동
public class AbilityControl : MonoBehaviour
{
    [Space(20), Title("ABILITY SYSTEM", underlined:true, fontSize = 15, alignment = TextAlignment.Center), HideField] public bool _t0;

    [Space(20), ReadOnly] public AbilityFlag flags = AbilityFlag.None;
    
    // 보유[잠재된 능력] 능력들 -> 아직 사용할 수 없는 능력들 보유
    [Space(10), SerializeField] List<AbilityData> datas = new List<AbilityData>();
    
    // <Key , Value>
    // 사용할 수 있는 능력
    private readonly Dictionary<AbilityFlag, Ability> actives = new Dictionary<AbilityFlag, Ability>();

    private void Update()
    {
        foreach( var a in actives)
            a.Value.Update();    
    }

    private void FixedUpdate()
    {
        foreach( var a in actives)
            a.Value.FixedUpdate();
    }

    public void Add(AbilityData d, bool immediate = false)
    {
        if (datas.Contains(d) == true || d == null)
            return;
        flags.Add(d.Flag, null);

        datas.Add(d);
        var ability = d.CreateAbility(GetComponent<CharacterControl>());
        
        if(immediate)
            actives[d.Flag] = ability;      
    }

    public void Remove(AbilityData d)
    {
        if (datas.Contains(d) == false || d == null)
            return;

        datas.Remove(d);
        flags.Remove(d.Flag, null);
        actives.Remove(d.Flag);
    }

    // 능력 활성화 및 업데이트 활성화
    public void Activate(AbilityFlag flag)
    {
        foreach(var pair in actives){
            if(pair.Key.Has(flag)) {
                pair.Value.Activate();
                if(!actives.ContainsKey(flag)){
                    actives[flag] = pair.Value;
                }
            }
        }
        // HashSet<Key> : 리스트와 같지만, 중복(x), 자동 정렬(O)
    }

    // 능력 비활성화 및 업데이트 제거
    public void DeActivate(AbilityFlag flag){
        foreach(var pair in actives){
            if(pair.Key.Has(flag))
            {
                actives.Remove(flag);
                pair.Value.Deactivate();
            }
        }
    }
}