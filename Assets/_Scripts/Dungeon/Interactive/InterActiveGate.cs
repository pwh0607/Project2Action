using System;
using UnityEngine;

[Serializable]
public abstract class InterActiveGate : MonoBehaviour
{
    public GateType type;
    public Vector3 doorPos {get; private set;}
}