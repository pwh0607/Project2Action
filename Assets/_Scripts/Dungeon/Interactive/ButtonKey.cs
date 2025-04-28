using System.Collections;
using CustomInspector;
using UnityEngine;
using UnityEngine.Events;

public class ButtonKey : AnswerKey
{
    public override KeyType Key => KeyType.BUTTON;
    [SerializeField] ButtonKeyData buttonKeyData;
    [SerializeField, ReadOnly] private GameObject heavyObject;                     
    [SerializeField, ReadOnly] private GameObject button;
    private bool isPressed = false;
 
    private AudioSource auidoSource;
 
 
    void Start()
    {
        isPressed = false;
        TryGetComponent(out auidoSource);

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
        StartCoroutine(SetPosition_Co(pos1, pos2));
    }

    IEnumerator SetPosition_Co(Vector3 pos1, Vector3 pos2){
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
            OnPressedButton?.Invoke(this, true);
            auidoSource.Play();
        }       
    }

    void OnCollisionExit(Collision collision)
    {
        if(!isPressed) return;
        if(collision.gameObject.tag == "HEAVYOBJECT"){
            OnPressedButton?.Invoke(this, false);
        }          
    }
    #endregion
}