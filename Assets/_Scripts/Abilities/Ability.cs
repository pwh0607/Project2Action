using System;
using UnityEngine;

[Flags]
public enum AbilityFlag
{
    None = 0,
    // Player
    MoveKeyboard = 1 << 0,
    MoveMouse = 1 << 1,     
    Jump = 1 << 2,          
    Pick = 1 << 3,

    // Enemy
    Wandor = 1 << 11,     
    Trace = 1 << 12,

    // Attack
    Attack = 1 << 21,
    
    // Damaged
    Damage = 1 << 30,
    Death = 1 << 35,
}

public abstract class AbilityData : ScriptableObject
{
    public abstract AbilityFlag Flag { get; }
    public abstract Ability CreateAbility( CharacterControl owner );
}

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

    public Ability(D data, CharacterControl owner){   
        this.data = data;
        this.owner = owner;
    }
}