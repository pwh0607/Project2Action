using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StageLogicManager : BehaviourSingleton<StageLogicManager>
{
    protected override bool IsDontDestroy() => false;
    
    public List<DoorLogic> logics = new ();
    public UnityAction OnOpenLogicCompleted;
    [SerializeField] List<InterActiveGate> gates = new();

    void Start()
    {
        gates.Clear();
        gates = new List<InterActiveGate>(FindObjectsByType<InterActiveGate>(FindObjectsSortMode.None));
    }

    //test
    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.Space)){
        //     OnOpenLogicCompleted?.Invoke();
        // }
    }

    public bool UseKey(InterActiveGate gate, AnswerKey answerKey){
        Debug.Log($"StageLogicManager : {gate} = {answerKey} CHECK!");
        var targetKey = logics.Find(pair => pair.key == answerKey);
        if(targetKey == null) return false;

        return gate == targetKey.gate;
    }


    #region Test-Code
    

    #endregion
}

[Serializable]
public class DoorLogic{
    public InterActiveGate gate;
    public AnswerKey key;
}