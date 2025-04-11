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
        var ability = d.CreateAbility(GetComponent<CharacterControl>());
        
        if(immediate){
            actives[d.Flag] = ability;      
            actives[d.Flag].Activate(null);
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

    // bool => 기존 ability를 비활성화 하고 새로운 ability를 활성화 시킨다.
    public void Activate(AbilityFlag flag, bool forceDeactivate, object obj)                //Object object.
    {
        if(forceDeactivate){
            DeactivateAll();
        }

        foreach(var d in datas){
            if((d.Flag & flag) == flag) {
                if(!actives.ContainsKey(flag))
                    actives[flag] = d.CreateAbility(GetComponent<CharacterControl>());
                
                actives[flag].Activate(obj);
            }
        }
        
    }

    public void Deactivate(AbilityFlag flag){
        foreach(var d in datas){
            if((d.Flag & flag) == flag)
            {
                if(actives.ContainsKey(flag)){
                    flags.Remove(flag);
                    actives[flag].Deactivate();
                    actives[flag] = null;
                    actives.Remove(flag);
                }   
            }
        }
    }

    // 모든 어빌리티 비활성화.
    public void DeactivateAll(){
        foreach(var active in actives.Values){
            active.Deactivate();
        }
        actives.Clear();
    }
}