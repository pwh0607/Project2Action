using System;
using System.Collections.Generic;
using UnityEngine;

public class StageLogicManager : BehaviourSingleton<StageLogicManager>
{
    [Serializable]
    public class GatePair{
        public InterActiveGate gate;
        public AnswerKey answerKey;

        public GatePair(InterActiveGate gate, AnswerKey answerKey){
            this.gate = gate;
            this.answerKey = answerKey;
        }
    }
    
    protected override bool IsDontDestroy() => true;

    public List<GatePair> pairs = new();

    public GameObject portalPrefab;

    private bool stageCompleted;

    public List<Room> rooms = new();
    public List<Link> links = new();

    void Start()
    {
        pairs.Clear();
        stageCompleted = false;
    }

    public void SetPair(LockedGate gate, AnswerKey answerKey){
        GatePair pair = new GatePair(gate, answerKey);
        pairs.Add(pair);

        if(answerKey is ButtonKey buttonKey)
            buttonKey.RegisterEvent(OnPressButton);
    }

    public bool OnUseKey(LockedGate gate, AnswerKey answerKey){
        var targetKey = pairs.Find(pair => pair.answerKey == answerKey);
        
        if(targetKey == null) {
            Debug.Log($"Logic Fail...");
            return false;
        }

        gate.OpenGate();
        return gate == targetKey.gate;
    }

    public void OnPressButton(AnswerKey answerKey, bool on){
        InterActiveGate gate = SearchGate(answerKey);

        if(gate == null) return;
        
        LockedGate lockedGate = gate as LockedGate;
        if(on) lockedGate.OpenGate();
        else lockedGate.CloseGate();

    }

    InterActiveGate SearchGate(AnswerKey answerKey){
        foreach(var p in pairs){
            if(p.answerKey == answerKey){
                return p.gate;
            }
        }
        return null;
    }

    void RemovePair(AnswerKey answerKey){
        GatePair removedPair = null;
        foreach(var p in pairs){
            if(p.answerKey == answerKey){
                removedPair = p;
                break;
            }
        }
        
        if(removedPair == null) return;

        pairs.Remove(removedPair);

        if(pairs.Count <= 0 && !stageCompleted){
            ShowPortal();
        }
    }

    public void SendDungeonData(List<Room> rooms){
        this.rooms = rooms;
    }

    void ShowPortal(){
        
    }
}