using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Events;

public class StageLogicManager : BehaviourSingleton<StageLogicManager>
{
    protected override bool IsDontDestroy() => false;
    
    public List<GatePair> pairs = new ();
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

    public void SetPair(InterActiveGate gate, AnswerKey answerKey){
        GatePair pair = new(gate, answerKey);
        pairs.Add(pair);
    }

    public bool UseKey(InterActiveGate gate, AnswerKey answerKey){
        Debug.Log($"StageLogicManager : {gate} = {answerKey} CHECK!");
        var targetKey = pairs.Find(pair => pair.key == answerKey);
        if(targetKey == null) {
            Debug.Log($"Logic Fail...");
            return false;
        }
        Debug.Log($"Logic Complete!");
        return gate == targetKey.gate;
    }


    #region Test-Code
    

    #endregion
}

[Serializable]
public class GatePair{
    public InterActiveGate gate;
    public AnswerKey key;
    public GatePair(InterActiveGate gate, AnswerKey key){
        this.gate = gate;
        this.key = key;
    }
}