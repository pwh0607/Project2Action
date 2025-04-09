using UnityEngine;

public enum KeyType{ NORMAL, BUTTON }
public abstract class AnswerKey : MonoBehaviour
{
    [SerializeField] KeyType key;
    public abstract KeyType Key{get;}

    [SerializeField] AnswerKeyData answerKeyData;
    //모델을 생성하고, 그 모델이 무엇인지에 대해 판단하여 위치를 설정한다.

    void Start()
    {
        
    }
}
