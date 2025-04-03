using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Datas/ActorProfile")]
public class ActorProfile : ScriptableObject
{
    [HorizontalLine("Properties", color:FixedColor.Cyan), HideField] public bool _h_s0;
    public string alias;
    [Preview(Size.medium)] public Sprite portrait;
    [Preview(Size.medium)] public GameObject model;
    [Preview(Size.medium)] public Avatar avatar;

    [Space(10), HorizontalLine("Abilities"), HideField] public bool _h_e0;
    public List<AbilityData> abilities;
}