using UnityEngine;

public class ButtonKey : AnswerKey
{
    [SerializeField] ButtonKeyData buttonKeyData;
    private GameObject key;                  //key
    private FloorButton button;               //바닥 버튼

    void Start()
    {
        MakeKey();
        MakeButton();
    }

    public void MakeKey(){
        int rnd = Random.Range(0, buttonKeyData.key.Count-1);
        key = Instantiate(buttonKeyData.key[rnd], transform);
    }

    public void MakeButton(){
        button = Instantiate(buttonKeyData.button, transform).GetComponent<FloorButton>();        
        button.RegisterEvent(CheckValid);
    }

    void CheckValid(bool flag){
        if(flag){
            Debug.Log("Press Button, Open Door!!");
        }else{
            Debug.Log("null, Close Door....");
        }
    }
}