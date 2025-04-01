using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StageLogicManager : BehaviourSingleton<StageLogicManager>
{
    protected override bool IsDontDestroy() => false;
    
    public List<DoorLogic> logics = new ();
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

    public bool UseKey(Door door, AnswerKey answerKey){
        var targetKey = logics.Find(pair => pair.key == answerKey);
        if(targetKey == null) return false;

        return door == targetKey.door;
    }
}

[Serializable]
public class DoorLogic{
    public Door door;
    public AnswerKey key;
}