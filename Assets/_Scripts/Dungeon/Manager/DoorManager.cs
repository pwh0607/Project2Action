using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorManager : BehaviourSingleton<DoorManager>
{
    protected override bool IsDontDestroy() => false;
    
    public UnityAction OnOpenLogicCompleted;
    [SerializeField] List<Door> doors = new();

    void Start()
    {
        doors.Clear();
        doors = new List<Door>(FindObjectsByType<Door>(FindObjectsSortMode.None));
    }

    //test
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            OnOpenLogicCompleted?.Invoke();
        }
    }
}