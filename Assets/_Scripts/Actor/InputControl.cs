using Project2Action;
using UnityEngine;

public class InputControl : MonoBehaviour
{
    [HideInInspector] public ActionGameInput actionInput;

    void Awake()
    {
        actionInput = new ActionGameInput();
    }

    void OnEnable()
    {
        actionInput.Player.Enable();
    }

    void OnDisable()
    {
        actionInput.Player.Disable();
    }
}