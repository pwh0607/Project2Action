using System;
using UnityEngine;

[Serializable]
public abstract class InterActiveGate : InteractiveObject
{
    public GateType type;
    public Vector3 doorPos {get; private set;}

}