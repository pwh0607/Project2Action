using System;
using UnityEngine;

// MVC : Model(데이터) , View(UI) , Control(행동)

// abstract     : 부모,자식 - 변수(O)
// interface    : 친척      - 변수(X)

// GAS ( Game Ability System ) : 언리얼
// 32bit = 4byte ( int )
// 0000 0000 0000 0000  ... 0000 0000 0000 0101
[Flags]
public enum AbilityFlag
{
    None = 0,

    // Player
    MoveKeyboard = 1 << 0,  // 0001
    MoveMouse = 1 << 1,     // 0010
    Jump = 1 << 2,          // 0100
    
    // Enemy
    Wandor = 1 << 3,        // 1000
}

// 데이터 담당 : 역할
// 1. Ability 의 타입을 정한다.
// 2. Ability 타입에 맞게 생성한다.
public abstract class AbilityData : ScriptableObject
{
    public abstract AbilityFlag Flag { get; }
    public abstract Ability CreateAbility( IActorControl owner );
}

// 행동 담당
// abstract 와 virtual 차이 ? 
// abstract (정의 - 필수) 는 자식에서 무조건 정의 해야한다.
// virtual (정의 - 선택) 은 옵션
public abstract class Ability{
    public virtual void Activate() { }
    public virtual void Deactivate() { }
    public virtual void Update(){ }
    public virtual void FixedUpdate() { }
}

public abstract class Ability<D> : Ability where D : AbilityData
{    
    public D data;

    protected IActorControl owner;

    public Ability(D data, IActorControl owner){             // data와 data가 적용된 controller
        this.data = data;
        this.owner = owner;
    }
}