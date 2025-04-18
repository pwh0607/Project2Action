using UnityEngine;
using UnityEngine.Events;

public class FloorButton : MonoBehaviour
{
    private UnityAction<bool> OnPressedButton;
    
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "HEAVYOBJECT" || collision.gameObject.tag == "Player"){
            OnPressedButton(true);
        }       
    }

    void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "HEAVYOBJECT" || collision.gameObject.tag == "Player"){
            OnPressedButton(false);
        }          
    }

    public void RegisterEvent(UnityAction<bool> action){
        OnPressedButton += action;
    }

    public void UnRegisterEvent(UnityAction<bool> action){
        OnPressedButton -= action;
    }
}