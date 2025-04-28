using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CustomInspector;

public class AbilityControl : MonoBehaviour
{
    [HorizontalLine("Current-Abilities"), HideField] public bool h_s_02;
    [Space(20), ReadOnly] public AbilityFlag flags = AbilityFlag.None;
    
    [Space(10), SerializeField] List<AbilityData> datas = new List<AbilityData>();

    private readonly Dictionary<AbilityFlag, Ability> actives = new Dictionary<AbilityFlag, Ability>();

    private void Update()
    {
        foreach( var a in actives.ToList())
            a.Value?.Update();
    }

    private void FixedUpdate()
    {
        foreach( var a in actives.ToList())
            a.Value?.FixedUpdate();
    }

    public void Add(AbilityData d, bool immediate = false)
    {
        if (datas.Contains(d) == true || d == null)
            return;
        flags.Add(d.Flag, null);

        datas.Add(d);
        var ability = d.CreateAbility(GetComponent<CharacterControl>());
        
        if(immediate){
            actives[d.Flag] = ability;      
            ability.Activate(null);
        }
    }

    public void Remove(AbilityData d)
    {
        if (datas.Contains(d) == false || d == null)
            return;

        Deactivate(d.Flag);
        datas.Remove(d);
        flags.Remove(d.Flag, null);
        actives.Remove(d.Flag);
    }
    
    public void RemoveAll(){
        DeactivateAll();

        flags = AbilityFlag.None;
        actives.Clear();
        datas.Clear();
    }

    public void Activate(AbilityFlag flag, bool forceDeactivate, object obj)                
    {
        if(forceDeactivate) DeactivateAll();

        List<AbilityData> temp = new();
        temp.AddRange(datas.GetRange(0, datas.Count));
    
        foreach( var d in datas )
        {
            if ((d.Flag & flag) == flag)
            {
                if (actives.ContainsKey(flag) == false)
                    actives[flag] = d.CreateAbility(GetComponent<CharacterControl>());

                actives[flag].Activate(obj);
            }
        }        
    }

    public void Deactivate(AbilityFlag flag)
    {        
        foreach( var d in datas )
        {
            if ((d.Flag & flag) == flag)               
            {
                if (actives.ContainsKey(flag) == true)
                {
                    flags.Remove(flag, null);
                    actives[flag].Deactivate();
                    actives[flag] = null;
                    actives.Remove(flag);
                }
            }
        }
    }

    public void DeactivateAll()
    {
        foreach( var a in actives )
            a.Value.Deactivate();
        actives.Clear();
    }
}