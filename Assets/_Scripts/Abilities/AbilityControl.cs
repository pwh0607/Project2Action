using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CustomInspector;

// abilityDatas : 외부에서 능력 부여/회수 인터페이스
// abilities : abilityDatas 갱신해서 행동
public class AbilityControl : MonoBehaviour
{
    [HorizontalLine("Current-Abilities"), HideField] public bool h_s_02;
    [Space(20), ReadOnly] public AbilityFlag flags = AbilityFlag.None;
    
    [Space(10), SerializeField] List<AbilityData> datas = new List<AbilityData>();

    private readonly Dictionary<AbilityFlag, Ability> actives = new Dictionary<AbilityFlag, Ability>();

    private void FixedUpdate()
    {
        foreach( var a in actives.ToList())
            a.Value.FixedUpdate();
    }

    private void Update()
    {
        foreach( var a in actives.ToList())
            a.Value.Update();
    }

    // 잠재능력을 추가
    public void Add(AbilityData d, bool immediate = false)
    {
        if (datas.Contains(d) == true || d == null)
            return;
        flags.Add(d.Flag, null);

        datas.Add(d);
        var ability = d.CreateAbility(GetComponent<IActorControl>());
        
        if(immediate){
            actives[d.Flag] = ability;      
            actives[d.Flag].Activate();
        }
    }

    // 잠재능력 제거 => 절대 할 수 없는 행동.
    public void Remove(AbilityData d)
    {
        if (datas.Contains(d) == false || d == null)
            return;

        datas.Remove(d);
        flags.Remove(d.Flag, null);
        actives.Remove(d.Flag);
    }

    public void Activate(AbilityFlag flag, bool immediate = false)
    {
        foreach(var d in datas){
            if((d.Flag & flag) == flag) {
                if(!actives.ContainsKey(flag))
                    actives[flag] = d.CreateAbility(GetComponent<CharacterControl>());
                actives[flag].Activate();
            }
        }
    }

    public void Deactivate(AbilityFlag flag){
        foreach(var d in datas){
            if((d.Flag & flag) == flag)
            {
                if(actives.ContainsKey(flag)){
                    flags.Remove(flag, null);
                    actives[flag].Deactivate();
                    actives[flag] = null;
                    actives.Remove(flag);
                }
            }
        }
    }
}