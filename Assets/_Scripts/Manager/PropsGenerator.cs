using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DungeonArchitect;
using DungeonArchitect.Builders.GridFlow;
using CustomInspector;

public class PropsGenerator : MonoBehaviour
{
    [ReadOnly] public Dungeon dungeon;  
    
    [Space(20)]
    
    public int lockCount;
    
    public Dictionary<string, Room> roomDic = new();
    public List<Room> rooms = new();
    public List<Link> links = new();
    
    #region Mark    
    [Space(20)]
    public List<GameObject> markPrefab;
    #endregion

    public DoorData doorData;
    public Vector3 offset = new Vector3(5f,0,5f);

    void Start()
    {
        dungeon = FindAnyObjectByType<Dungeon>();

        InitGraphData();
        CreateGate();
        InitVisited();

        while(lockCount > 0){
            SearchGraph();
            lockCount--;
        }

        StageLogicManager.I.SendDungeonData(rooms);
    }

    #region InitDatas
    void InitGraphData(){

        if (dungeon == null)
            return;

        GridFlowDungeonModel gridFlowModel = dungeon.GetComponent<GridFlowDungeonModel>();
        if (gridFlowModel == null || gridFlowModel.LayoutGraph == null)
            return;

        var graphNodes = gridFlowModel.LayoutGraph.Nodes;
        var graphLinks = gridFlowModel.LayoutGraph.Links;
        
        foreach (var node in graphNodes)
        {
            if(node.active){
                Vector3 nodePosition = new Vector3(node.coord.x * 20, 0, node.coord.y * 20) + offset;
                Room room = new Room(node.nodeId.ToString(), nodePosition);
                roomDic[node.nodeId.ToString()] = room;
                rooms.Add(room);
            }
        }

        foreach(var link in graphLinks){
            Room startNode = roomDic[link.source.ToString()];
            Room endNode = roomDic[link.destination.ToString()];

            startNode.n_Node.Add(endNode);
            endNode.n_Node.Add(startNode);

            Vector3 startNodePosition = startNode.roomPosition;
            Vector3 endNodePosition = endNode.roomPosition;

            Vector3 vec = startNodePosition - endNodePosition;
            float y = Mathf.Atan2(vec.x, vec.y) * Mathf.Rad2Deg;

            Vector3 center = (startNodePosition + endNodePosition) / 2;
            Link newLink = new(startNode, endNode, center, Quaternion.Euler(new Vector3(0,y,0)));

            links.Add(newLink);
        }
    }    
    
    void CreateGate(){
        int totalLinks = links.Count;

        List<int> validIndexes = new List<int>();
        for (int i = 0; i < totalLinks; i++) {
            Link link = links[i];
            if (link.isNearWall){
                validIndexes.Add(i);   
            }
        }

        List<int> lockedIndexes = RandomGenerator.RandomIntGenerate(validIndexes.Count, lockCount)
                                                .Select(idx => validIndexes[idx])
                                                .ToList();

        for (int i = 0; i < validIndexes.Count; i++) {
            Link link = links[validIndexes[i]];

            GameObject gatePrefab = lockedIndexes.Contains(i) ? doorData.lockedGate : doorData.openedGate;

            InterActiveGate gate = Instantiate(gatePrefab, link.position, link.quaternion).GetComponent<InterActiveGate>();
            link.gate = gate;
        }
    }

    List<int> RandomIndex(int size){
        List<int> res = RandomGenerator.RandomIntGenerate(size, lockCount);
        return res;
    }
    #endregion

    #region 그래프 탐색
    [ReadOnly] public string startNodeId;

    public Dictionary<Room, bool> visited = new();
    public List<Room> visitedRoom = new();
    public List<Room> activeRooms = new();

    void InitVisited(){
        foreach(var room in roomDic){
            visited[room.Value] = false;
        }
    }

    #region TestCode
    [Button("SetMark"), HideField] public bool btn;
    public int searchCount = 0;
    #endregion

    public void SetMark(){
        SearchGraph();
        foreach(var room in activeRooms){
            Instantiate(markPrefab[searchCount], room.roomPosition, Quaternion.identity);
        }
        searchCount++;
    }

    [Space(20)]
    [ReadOnly] public InterActiveGate targetGate = null; 
    public List<InterActiveGate> gates = new();

    void SearchGraph(){
        SearchNode();

        Queue<Room> queue = new();
        Room startRoom = roomDic[startNodeId];
        activeRooms.Add(startRoom);
        queue.Enqueue(startRoom);
        
        visited[startRoom] = true;
        visitedRoom.Add(startRoom);

        while(queue.Count >= 1){
            Room r = queue.Dequeue();
            
            foreach(var node in r.n_Node){
                Link link = links.Find(v => (v.node.Item1.Equals(r) && v.node.Item2.Equals(node)) || (v.node.Item1.Equals(node) && v.node.Item2.Equals(r)));

                if(visited[node] || link == null) continue;

                if(link.gate != null){
                    if(link.gate.type == GateType.LOCK) {
                        if(!gates.Contains(link.gate)){                         
                            targetGate = link.gate;
                            gates.Add(link.gate);

                            AnswerKey key = MakeKey();
                            SendPairData(targetGate, key); 
                            SetKeyPosition(key);
                        }
                        continue;
                    }    
                }

                queue.Enqueue(node);
                activeRooms.Add(node);
                visited[node] = true;
                visitedRoom.Add(node);
            }
        }
    }

    public int spawnNodeNumber;             // PLAYER Spawn Node

    void SearchNode(){
        if(targetGate == null){
            var key = roomDic.Keys.ToList();
            startNodeId = roomDic[key[spawnNodeNumber]].roomId;     
        
            return;
        }
        
        Link link = links.Find(v => v.gate == targetGate);
        if(link == null) return;

        var room1 = link.node.Item1;
        var room2 = link.node.Item2;

        if(!visited[room1]){
            startNodeId = room1.roomId;
            return;
        }
        
        if(!visited[room2]){
            startNodeId = room2.roomId;
            return;
        }

        targetGate = null;
    }
  
    public AnswerKeyData answerKeyData;

    AnswerKey MakeKey(){
        int rnd = UnityEngine.Random.Range(0, answerKeyData.key.Count);
        
        GameObject keyPrefab = answerKeyData.key[rnd];
        AnswerKey key = Instantiate(keyPrefab).GetComponent<AnswerKey>();

        return key;
    }

    void SetKeyPosition(AnswerKey key){
        if(key is NormalKey normalKey){
            int rnd1 = UnityEngine.Random.Range(0, activeRooms.Count);

            Vector3 center = activeRooms[rnd1].roomPosition;
            normalKey.transform.position = RandomPositionCheck(center) + Vector3.up * 0.3f;
        }else if (key is ButtonKey buttonKey){
            List<int> randomIndex = RandomGenerator.RandomIntGenerate(activeRooms.Count, 2);

            Vector3 center1 = Vector3.zero; 
            Vector3 center2 = Vector3.zero; 
            
            if(activeRooms.Count <= 1){
                center1 = activeRooms[0].roomPosition;
                center2 = activeRooms[0].roomPosition;
            }else{
                center1 = activeRooms[randomIndex[0]].roomPosition;
                center2 = activeRooms[randomIndex[1]].roomPosition;
            }

            Vector3 pos1 = RandomPositionCheck(center1) + Vector3.up;
            Vector3 pos2 = RandomPositionCheck(center2) + Vector3.up * 0.1f;

            buttonKey.SetPosition(pos1, pos2);
        }
    }
#endregion

    Vector3 RandomPositionCheck(Vector3 center){
        GameObject tile = null;
        
        while(tile == null){
            Vector3 sphereRandom = UnityEngine.Random.insideUnitSphere * 8f;
            Vector3 pos = center + sphereRandom;

            if(Physics.Raycast(pos + Vector3.up * 20f, Vector3.down, out RaycastHit hit, Mathf.Infinity)){
                if(hit.collider.tag == "TILE"){
                    tile = hit.collider.gameObject;
                    break;
                }
            }
        }

        Vector3 res = (tile == null) ? Vector3.zero : tile.transform.position;
        return res;
    }

    public void SendPairData(InterActiveGate gate, AnswerKey key)
    {
        if(gate is LockedGate lockedGate){
            StageLogicManager.I.SetPair(lockedGate, key);
        }
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