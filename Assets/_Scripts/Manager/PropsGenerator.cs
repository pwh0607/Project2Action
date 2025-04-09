using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DungeonArchitect;
using DungeonArchitect.Builders.GridFlow;
using CustomInspector;
using UnityEngine.InputSystem;

public class PropsGenerator : MonoBehaviour
{
    [HorizontalLine("DungeonProps"), HideInInspector] public bool h_s1;
    private int searchCount = 0;
    public int lockCount;
    [HorizontalLine(color:FixedColor.Cyan), HideInInspector] public bool h_e1;

    [Space(20)]
    [HorizontalLine("DungeonData"), HideInInspector] public bool h_s3;
    public Dungeon dungeon;
    public Dictionary<string, Room> roomDic = new();
    public List<Room> rooms = new();
    public List<Link> links = new();
    [HorizontalLine(color:FixedColor.Cyan), HideInInspector] public bool h_e3;
    
    [Space(20)]
    public List<GameObject> tmpPrefab;

    //Test
    public DoorData doorData;
    public Vector3 offset = new Vector3(5f,0,5f);
    void Start()
    {
        InitGraphData();
        InitGate();
        InitVisited();

        while(lockCount > 0){
            SearchGraph();
            lockCount--;
        }
    }

    #region InitDatas
    void InitGraphData(){
        Debug.Log("InitGraph...");
        if (dungeon == null)
        {
            Debug.LogError("Dungeon 객체가 설정되지 않았습니다!");
            return;
        }

        GridFlowDungeonModel gridFlowModel = dungeon.GetComponent<GridFlowDungeonModel>();
        if (gridFlowModel == null || gridFlowModel.LayoutGraph == null)
        {
            Debug.LogError("GridFlow Dungeon Model을 찾을 수 없습니다!");
            return;
        }

        var graphNodes = gridFlowModel.LayoutGraph.Nodes;
        var graphLinks = gridFlowModel.LayoutGraph.Links;
        
        foreach (var node in graphNodes)
        {
            if(node.active){
                // room 위치 측정하기.
                Vector3 nodePosition = new Vector3(node.coord.x * 20, 0, node.coord.y * 20) + offset;
                Room room = new Room(node.nodeId.ToString(), nodePosition);
                roomDic[node.nodeId.ToString()] = room;
                rooms.Add(room);
            }
        }

        foreach(var link in graphLinks){
            Room startNode = roomDic[link.source.ToString()];
            Room endNode = roomDic[link.destination.ToString()];

            // 인접 노드 생성.
            startNode.n_Node.Add(endNode);
            endNode.n_Node.Add(startNode);

            Vector3 startNodePosition = startNode.roomPosition;
            Vector3 endNodePosition = endNode.roomPosition;

            //Rotation Check.
            Vector3 vec = startNodePosition - endNodePosition;
            float y = Mathf.Atan2(vec.x, vec.y) * Mathf.Rad2Deg;

            //이 두 포지션의 중심이 link의 위치.
            Vector3 center = (startNodePosition + endNodePosition) / 2;
            Link newLink = new(startNode, endNode, center, Quaternion.Euler(new Vector3(0,y,0)));

            links.Add(newLink);
        }
    }

    void InitGate(){
        List<int> indexes = RandomIndex(links.Count);

        for(int i=0;i<links.Count;i++){
            Link link = links[i];
            if(!CheckWall(Physics.OverlapSphere(link.linkPosition, 5f))) continue;
            
            GameObject gatePrefab = indexes.Contains(i) ? doorData.lockedGate : doorData.openedGate;
            InterActiveGate gate = Instantiate(gatePrefab, link.linkPosition, link.quaternion).GetComponent<InterActiveGate>();
            link.gate = gate;
        }
    }

    List<int> RandomIndex(int size){
        List<int> list = new();
        for(int i =0;i<size;i++){
            list.Add(i);
        }

        List<int> res = RandomGenerator.RandomIntGenerate(list, lockCount);
        return res;
    }
#endregion



#region 그래프 탐색
    [ReadOnly] public string startNodeId;             // 탐색 시작 노드.

    // 활성화된 노드들 중 선택하여 Key를 설치한다.
    public Dictionary<Room, bool> visited = new();
    public List<Room> visitedRoom = new();
    public List<Room> activeRooms = new();

    void InitVisited(){
        foreach(var room in roomDic){
            visited[room.Value] = false;
        }
    }

    // 1. StartSearch를 통해 첫번째로 발견한 잠긴 문과, key를 설치할 수 있는 노드들을 찾는다.
    // 2. StartSeach를 통해 activeRooms에 key를 설치할 수 있는 key를 설치한다.
    // 3. key를 설치 완료후, StartSearch를 시작하여 Gate 개수만큼 위 과정을 반복한다.
    public InterActiveGate targetGate = null; 
    public List<InterActiveGate> gates = new();

    void SearchGraph(){
        SearchNode();
        // activeRooms.Clear();

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

                // Gate가 없는 경우를 고려한다. => Gate : nullable
                // 무시해도 되는 node의 조건.
                if(link.gate != null){
                    if(link.gate.type == GateType.LOCK) {
                        if(!gates.Contains(link.gate)){                         //아직 처리하지 못한 Gate에 대해서만 설정한다.
                            targetGate = link.gate;
                            gates.Add(link.gate);
                            //알맞은 gate를 찾았으면 key 생성하기.
        
                            AnswerKey key = MakeKey();
                            SendPairData(link.gate, key);
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
        MakeMark();
    }

    void MakeMark(){
        foreach(var room in activeRooms){
            Instantiate(tmpPrefab[searchCount], room.roomPosition, Quaternion.identity);
        }
        searchCount++;
    }

    public int spawnNodeNumber = 3;                 //test용

    // 다음에 탐색할 노드를 찾는다.
    void SearchNode(){
        Debug.Log($"탐색 시작할 노드 찾기...");
        if(targetGate == null){
            Debug.Log("최초 노드 찾기/...");

            var key = roomDic.Keys.ToList();
            startNodeId = roomDic[key[spawnNodeNumber]].roomId;     
        
            return;
        }
        
        Link link = links.Find(v => v.gate == targetGate);
        if(link == null){
            Debug.Log("link 없음...");
        }

        //해당 링크의 두개의 룸에 대하여 아직 방문하지 않은 노드가 시작 노드이다.
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
        
        // 키의 종류
        GameObject keyPrefab = answerKeyData.key[rnd];
        AnswerKey key = Instantiate(keyPrefab).GetComponent<AnswerKey>();

        if(key is NormalKey normalKey){
            int rnd1 = UnityEngine.Random.Range(0, activeRooms.Count);
            normalKey.transform.position = activeRooms[rnd].roomPosition + (Vector3.up * 2);
        }else if (key is ButtonKey buttonKey){
            int rnd1 = UnityEngine.Random.Range(0, activeRooms.Count);
            Vector3 pos1 = activeRooms[rnd1].roomPosition + (Vector3.up * 2);

            int rnd2 = -1;
            do{
                rnd2 = UnityEngine.Random.Range(0, activeRooms.Count);
            }while(rnd1 == rnd2);
            Vector3 pos2 = activeRooms[rnd2].roomPosition + (Vector3.up * 2);
            buttonKey.SetPosition(pos1, pos2);
        }
        return key;
    }
#endregion

    public void SendPairData(InterActiveGate gate, AnswerKey key)
    {
        //로직 매니저 에게 이 페어를 보낸다.
        StageLogicManager.I.SetPair(gate, key);
    }

    bool CheckWall(Collider[] colliders){
        int count = 0;
        foreach(var col in colliders){
            if(col.tag == "WALL") count++;
        }
        return count >= 1;
    }
}





[Serializable]              
public class Room{
    public string roomId;
    public Vector3 roomPosition;
    public List<Room> n_Node;           //인접 노드.
    public Room(string id, Vector3 position){
        this.roomId = id;
        this.roomPosition = position;
        n_Node = new();
    }
}

public enum GateType{ NONE = 0, LOCK, OPEN}

[Serializable]
public class Link{
    public (Room, Room) node;
    public Vector3 linkPosition;
    public Quaternion quaternion;
    
    //Door 설치
    public InterActiveGate gate;

    public Link(Room startNodeId, Room endNodeId, Vector3 linkPosition, Quaternion quaternion){
        this.node = (startNodeId,endNodeId);
        
        this.linkPosition = linkPosition;
        this.quaternion = quaternion;
    }
}