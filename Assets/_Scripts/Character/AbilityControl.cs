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

    public void Add(AbilityData d)
    {
        if (datas.Contains(d) == true || d == null)
            return;

        datas.Add(d);
        var ability = d.CreateAbility(GetComponent<CharacterControl>());

        flags.Add(d.Flag, null);
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

    public void Trigger(AbilityFlag flag)
    {
        foreach( var pair in actives){
            if(pair.Key.Has(flag)) pair.Value.Activate();
        }
    }
}