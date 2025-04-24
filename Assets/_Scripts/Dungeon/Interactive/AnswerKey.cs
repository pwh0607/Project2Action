using UnityEngine;

public enum KeyType{ NORMAL, BUTTON }
public abstract class AnswerKey : Item
{
    [SerializeField] KeyType key;
    public abstract KeyType Key{get;}
    public int index;
}