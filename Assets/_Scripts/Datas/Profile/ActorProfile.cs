using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

[CreateAssetMenu(menuName = "Datas/ActorProfile")]
public class ActorProfile : ScriptableObject
{
    [HorizontalLine("Prefabs", color:FixedColor.Cyan), HideField] public bool _h_s0;
    
    public string alias;
    [Preview(Size.medium)] public Sprite portrait;
    [Preview(Size.medium)] public List<GameObject> models;
    [Preview(Size.medium)] public Avatar avatar;

    [Space(20)]
    [HorizontalLine("Attribute", color:FixedColor.Cyan), HideField] public bool _h_s1;
    [Tooltip("체력")] public int health;              
    [Tooltip("초당 이동 속도")] public float moveSpeed;
    [Tooltip("초당 회전 속도")] public float rotateSpeed;
    [Tooltip("점프 파워")] public float jumpForce; 
    [Tooltip("점프 체공 시간")] public float jumpDuration;

    [Space(20)]
    [HorizontalLine("Abilities"), HideField] public bool _h_s2;
    public List<AbilityData> abilities;
}