using UnityEngine;

public enum KeyType{ NORMAL, BUTTON }
public abstract class AnswerKey : MonoBehaviour
{
    [SerializeField] KeyType key;
    public abstract KeyType Key{get;}
}
