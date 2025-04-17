using UnityEngine;

public class ButtonKey : AnswerKey
{
    // 설치 가능한 방을 propsGenerator에게 받는다.
    public override KeyType Key => KeyType.BUTTON;
    [SerializeField] ButtonKeyData buttonKeyData;
    private GameObject heavyObject;                     //key
    private FloorButton button;                 //바닥 버튼

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
        // button.RegisterEvent(CheckValid);
    }

    public void SetPosition(Vector3 pos1, Vector2 pos2){
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