using System.Collections;
using CustomInspector;
using UnityEngine;

public class ButtonKey : AnswerKey
{
    // 설치 가능한 방을 propsGenerator에게 받는다.
    public override KeyType Key => KeyType.BUTTON;
    [SerializeField] ButtonKeyData buttonKeyData;
    [SerializeField, ReadOnly] private GameObject heavyObject;                     //key
    [SerializeField, ReadOnly] private FloorButton button;                 //바닥 버튼

    void Start()
    {
        MakeKey();
        MakeButton();
    }

    public void MakeKey(){
        int rnd = Random.Range(0, buttonKeyData.key.Count);
        heavyObject = Instantiate(buttonKeyData.key[rnd], transform);
    }

    public void MakeButton(){
        button = Instantiate(buttonKeyData.button, transform).GetComponent<FloorButton>();
        button.RegisterEvent(CheckValid);
    }

    public void SetPosition(Vector3 pos1, Vector3 pos2){
        StartCoroutine(PositionSetter(pos1, pos2));
    }

    IEnumerator PositionSetter(Vector3 pos1, Vector3 pos2){
        yield return new WaitUntil(() => heavyObject != null && button != null);

        heavyObject.transform.position = pos1;
        button.transform.position = pos2;
    }

    void CheckValid(bool flag){
        if(flag){
            Debug.Log("Press Button, Open Door!!");
        }else{
            Debug.Log("null, Close Door....");
        }
    }
}