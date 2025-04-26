using System.Collections;
using CustomInspector;
using UnityEngine;
using UnityEngine.Events;

public class ButtonKey : AnswerKey
{
    // 설치 가능한 방을 propsGenerator에게 받는다.
    public override KeyType Key => KeyType.BUTTON;
    [SerializeField] ButtonKeyData buttonKeyData;
    [SerializeField, ReadOnly] private GameObject heavyObject;                     //key 쌍
    [SerializeField, ReadOnly] private GameObject button;                 //바닥 버튼
    private bool isPressed = false;
    void Start()
    {
        isPressed = false;

        MakeKey();
        MakeButton();
    }

    public void MakeKey(){
        int rnd = Random.Range(0, buttonKeyData.key.Count);
        heavyObject = Instantiate(buttonKeyData.key[rnd]);
        heavyObject.SetActive(false);
    }

    public void MakeButton(){
        button = Instantiate(buttonKeyData.button, transform);
    }

    public void SetPosition(Vector3 pos1, Vector3 pos2){
        StartCoroutine(PositionSet(pos1, pos2));
    }

    IEnumerator PositionSet(Vector3 pos1, Vector3 pos2){
        yield return new WaitUntil(() => heavyObject != null && button != null);

        heavyObject.transform.position = pos1;
        transform.position = pos2;
        heavyObject.SetActive(true);
    }

    #region Event
    private UnityAction<AnswerKey, bool> OnPressedButton;
    
    public void RegisterEvent(UnityAction<AnswerKey, bool> action){
        OnPressedButton += action;
    }

    public void UnRegisterEvent(UnityAction<AnswerKey, bool> action){
        OnPressedButton -= action;
    }
    #endregion

    #region Collider
    void OnCollisionEnter(Collision collision)
    {
        if(isPressed) return;
        if(collision.gameObject.tag == "HEAVYOBJECT"){
            Debug.Log("버튼 푸쉬!");
            OnPressedButton?.Invoke(this, true);
        }       
    }

    void OnCollisionExit(Collision collision)
    {
        if(!isPressed) return;
        if(collision.gameObject.tag == "HEAVYOBJECT"){
            Debug.Log("버튼 풀...!");
            OnPressedButton?.Invoke(this, false);
        }          
    }
    #endregion
}