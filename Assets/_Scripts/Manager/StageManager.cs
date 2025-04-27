using System;
using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using DungeonArchitect;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : BehaviourSingleton<StageManager>
{
    protected override bool IsDontDestroy() => true;

    [ReadOnly] public Dungeon dungeon;

    public DungeonContext context = new();
    private DungeonBuilder builder;
    private GateGenerator gateGenerator;
    private KeyGenerator keyGenerator;


    private bool stageCompleted;
   
    public int lockCount;
    public int gateCount;
    public int spawnNodeNumber;


    public DoorData doorData;
    public AnswerKeyData answerKeyData;
    public Vector3 offset = new Vector3(5f, 0, 5f);

    public GameObject portalPrefab;
    private Portal stagePortal;

    void Start()
    {
        InitializeStage();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"새 씬 로드됨: {scene.name}");
        InitializeStage();
    }
    
    void InitializeStage(){
        SceneManager.sceneLoaded += OnSceneLoaded;
        gateCount = lockCount;
        dungeon = FindAnyObjectByType<Dungeon>();

        context = new();
        context.spawnNodeNumber = spawnNodeNumber;
        
        stageCompleted = false;

        builder = new DungeonBuilder(dungeon, context, offset);

        gateGenerator = new GateGenerator(context, doorData, offset, lockCount);

        keyGenerator = new KeyGenerator(context, answerKeyData, spawnNodeNumber);
        keyGenerator.Register(SetPair);
        
        StartCoroutine(InitializeStage_Co());
    }

    void Update()
    {
        CheckComplete();
    }

    IEnumerator InitializeStage_Co(){
        builder.InitDungeonData();
        builder.InitVisited();

        MakePortal();
        yield return new WaitForEndOfFrame();
        
        gateGenerator.Initialize();
        
        yield return new WaitForEndOfFrame();
        
        keyGenerator.Initialize();
    }

    void MakePortal(){
        int num = UnityEngine.Random.Range(0, context.rooms.Count);
        GameObject portal = Instantiate(portalPrefab, context.rooms[num].roomPosition, Quaternion.identity);
        
        portal.SetActive(false);
    }

    public void SetPair(LockedGate gate, AnswerKey answerKey){
        GatePair pair = new GatePair(gate, answerKey);
        context.pairs.Add(pair);

        if(answerKey is ButtonKey buttonKey)
            buttonKey.RegisterEvent(OnPressButton);
    }

    public bool OnUseKey(LockedGate gate, AnswerKey answerKey){
        var targetKey = context.pairs.Find(pair => pair.answerKey == answerKey);
        
        if(targetKey == null) {
            Debug.Log($"Logic Fail...");
            return false;
        }

        if(gate == targetKey.gate){
            gate.OpenGate();
            gateCount--;
            return true;
        }

        return false;
    }

    public void OnPressButton(AnswerKey answerKey, bool on){
        InterActiveGate gate = SearchGate(answerKey);

        if(gate == null) return;
        
        LockedGate lockedGate = gate as LockedGate;
        if(on) {
            lockedGate.OpenGate();
            gateCount--;
        }else {
            lockedGate.CloseGate();
            gateCount++;
        }
    }

    InterActiveGate SearchGate(AnswerKey answerKey){
        foreach(var p in context.pairs){
            if(p.answerKey == answerKey){
                return p.gate;
            }
        }
        return null;
    }

    void CheckComplete(){
        if(gateCount <= 0 && !stageCompleted){
            ShowPortal();
        }
    }

    void ShowPortal(){
        stageCompleted = true;
        stagePortal.gameObject.SetActive(true);  
    }
}

[Serializable]
public class DungeonContext{
    public int spawnNodeNumber;

    public Dictionary<string, Room> roomDic = new();
    public List<Room> rooms = new();
    public List<Link> links = new();
    
    public List<InterActiveGate> gates = new();

    public List<Room> activeRooms = new();
        
    public Dictionary<Room, bool> visited = new();
    public List<GatePair> pairs = new();
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


[Serializable]              
public class Room{
    public string roomId;
    public Vector3 roomPosition;
    public List<Room> n_Node;
    public Room(string id, Vector3 position){
        this.roomId = id;
        this.roomPosition = position;
        n_Node = new();
    }
}

public enum GateType{ NONE = 0, LOCK, OPEN }

[Serializable]
public class Link{
    public (Room, Room) node;
    public Vector3 position;
    public Quaternion quaternion;

    public InterActiveGate gate;
    public bool isNearWall;

    public Link(Room startNodeId, Room endNodeId, Vector3 position, Quaternion quaternion){
        this.node = (startNodeId,endNodeId);
        
        this.position = position;
        this.quaternion = quaternion;

        isNearWall = CheckWall(Physics.OverlapSphere(this.position, 5f));
    }

    private bool CheckWall(Collider[] colliders){
        int count = 0;
        foreach(var col in colliders){
            if(col.tag == "WALL") count++;
        }
        return count > 1;
    }
}