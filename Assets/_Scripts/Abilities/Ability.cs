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
    Move = 1 << 0,      // 0001
    Jump = 1 << 1,      // 0010
    Dodge = 1 << 2,     // 0100
    Attack = 1 << 3,    // 1000
}

//지속성 여부.
public enum AbilityEffect
{
    INSTANCE,
    DURATION,
    INFINITE,
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
    // 어빌리티 활성
    public virtual void Activate() { }
    // 어빌리티 비활성a
    public virtual void Deactivate() { }
    // 어빌리티 계속 Update
    public virtual void Update() { }

    // 어빌리티를 더 빠르게 Update 수행 (주로 물리 연산에 이용)
    public virtual void FixedUpdate(){}
}

public abstract class Ability<D> : Ability where D : AbilityData
{    
    public D data;

    protected CharacterControl owner;

    public Ability(D data, CharacterControl owner){
        this.data = data;
        this.owner = owner;
    }
}