using System;
using UnityEngine;

[Flags]
public enum AbilityFlag
{
    None = 0,

    // Player
    MoveKeyboard = 1 << 0,  // 0001
    MoveMouse = 1 << 1,     // 0010
    Jump = 1 << 2,          // 0100
    Pick = 1 << 3,

    // Enemy
    Wandor = 1 << 11,        // 1000
    Trace = 1 << 12,

    // Attack
    Attack = 1 << 21,
    
    // Damaged
    Damage = 1 << 30,
    Death = 1 << 35,
}

// 데이터 담당 : 역할
// 1. Ability 의 타입을 정한다.
// 2. Ability 타입에 맞게 생성한다.
public abstract class AbilityData : ScriptableObject
{
    public abstract AbilityFlag Flag { get; }
    public abstract Ability CreateAbility( CharacterControl owner );
}

// 행동 담당
// abstract 와 virtual 차이 ? 
// abstract (정의 - 필수) 는 자식에서 무조건 정의 해야한다.
// virtual (정의 - 선택) 은 옵션
public abstract class Ability{
    public virtual void Activate(object obj = null) { }
    public virtual void Deactivate() { }
    public virtual void Update(){ }
    public virtual void FixedUpdate() { }
}

public abstract class Ability<D> : Ability where D : AbilityData
{    
    public D data;

    protected CharacterControl owner;

    public Ability(D data, CharacterControl owner){             // data와 data가 적용된 controller
        this.data = data;
        this.owner = owner;
    }
}