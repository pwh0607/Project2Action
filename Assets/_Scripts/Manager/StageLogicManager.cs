using System;
using System.Collections.Generic;
using UnityEngine;

public class StageLogicManager : BehaviourSingleton<StageLogicManager>
{
    protected override bool IsDontDestroy() => true;
    int keyNumber;
    public List<GatePair> pairs = new();

    void Start()
    {
        pairs.Clear();
        keyNumber = 1;
    }

    public void SetPair(LockedGate gate, AnswerKey answerKey){
        Debug.Log($"Stage Logic Manager : {gate}, {answerKey}");
        GatePair pair = new GatePair(gate, answerKey);
        pairs.Add(pair);

        if(answerKey is ButtonKey buttonKey){
            buttonKey.RegisterEvent(OnPressButton);
        }
        
        answerKey.index = keyNumber;
        gate.index = keyNumber;

        keyNumber++;
    }

    //문에 아이템을 사용했을 때 이벤트 처리.
    
    // Normal Key를 문 앞에서 사용했을 때.
    public bool OnUseKey(LockedGate gate, AnswerKey answerKey){
        Debug.Log($"Stage Logic Manager : {gate} = {answerKey} CHECK!");
        var targetKey = pairs.Find(pair => pair.answerKey == answerKey);
        
        if(targetKey == null) {
            Debug.Log($"Logic Fail...");
            return false;
        }
        Debug.Log($"Logic Complete!");
        return gate == targetKey.gate;
    }

    // Button Key를 사용했을 때.
    public void OnPressButton(AnswerKey answerKey, bool on){
        InterActiveGate gate = SearchGate(answerKey);

        if(gate == null) return;

        LockedGate lockedGate = gate as LockedGate;
        if(on){
            Debug.Log("문 열기.");
            lockedGate.OpenGate();
        }else{
            Debug.Log("문 닫기.");
            lockedGate.CloseGate();
        }
    }

    InterActiveGate SearchGate(AnswerKey answerKey){
        foreach(var p in pairs){
            if(p.answerKey == answerKey){
                Debug.Log("키와 맞는 쌍을 찾았다.");
                return p.gate;
            }
        }
        return null;
    } 
}

[Serializable]
public class GatePair{
    public InterActiveGate gate;
    public AnswerKey answerKey;

    public GatePair(InterActiveGate gate, AnswerKey answerKey){
        this.gate = gate;
        this.answerKey = answerKey;
    }
}