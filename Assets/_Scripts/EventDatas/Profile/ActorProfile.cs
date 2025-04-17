using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

public enum ActorType {NONE, PLAYER, ENEMY, NPC}

[CreateAssetMenu(menuName = "Datas/ActorProfile")]
public class ActorProfile : ScriptableObject
{
    [HorizontalLine("Prefabs", color:FixedColor.Cyan), HideField] public bool _h_s0;
    
    public ActorType actorType = ActorType.NONE;

    public string alias;
    [Preview(Size.medium)] public Sprite portrait;
    [Preview(Size.medium)] public List<GameObject> models;
    [Preview(Size.medium)] public Avatar avatar;
    
    [HorizontalLine("Animations", color:FixedColor.Cyan), HideField] public bool _h_s1;
    public AnimatorOverrideController animatorOverride;
    [Preview(Size.medium)] public List<AnimationClip> ATTACK;
    [HorizontalLine("Animations", color:FixedColor.Cyan), HideField] public bool _h_e1;
    [Space(20)]
    [HorizontalLine("Attribute", color:FixedColor.Cyan), HideField] public bool _h_s2;
    [Tooltip("체력")] public int health;              
    [Tooltip("초당 이동 속도")] public float moveSpeed;
    [Tooltip("초당 회전 속도")] public float rotateSpeed;
    [Tooltip("점프 파워")] public float jumpForce; 
    [Tooltip("점프 체공 시간")] public float jumpDuration;
    [Tooltip("공격 빈도")] public float attackInterval;    
    [Tooltip("공격력")] public int attackDamage;         
    
    [Space(20)]
    [HorizontalLine("Abilities"), HideField] public bool _h_s3;
    public List<AbilityData> abilities;
}