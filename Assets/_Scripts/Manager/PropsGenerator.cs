using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DungeonArchitect;
using DungeonArchitect.Builders.GridFlow;
using CustomInspector;

public class PropsGenerator : MonoBehaviour
{
    [HorizontalLine("DungeonProps"), HideInInspector] public bool h_s1;
    private int searchCount = 0;
    public int lockCount;
    [HorizontalLine(color:FixedColor.Cyan), HideInInspector] public bool h_e1;

    [Space(20)]
    [HorizontalLine("DungeonData"), HideInInspector] public bool h_s3;
    [ReadOnly] public Dungeon dungeon;
    public Dictionary<string, Room> roomDic = new();
    public List<Room> rooms = new();
    public List<Link> links = new();
    [HorizontalLine(color:FixedColor.Cyan), HideInInspector] public bool h_e3;
    
    [Space(20)]
    public List<GameObject> markPrefab;

    //Test
    public DoorData doorData;
    public Vector3 offset = new Vector3(5f,0,5f);

    void Start()
    {
        dungeon = FindAnyObjectByType<Dungeon>();

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
            if(!CheckWall(Physics.OverlapSphere(link.linkPosition, 5f)))
                continue;
            
            GameObject gatePrefab = indexes.Contains(i) ? doorData.lockedGate : doorData.openedGate;
            InterActiveGate gate = Instantiate(gatePrefab, link.linkPosition, link.quaternion).GetComponent<InterActiveGate>();
            link.gate = gate;
        }
    }

    List<int> RandomIndex(int size){
        List<int> res = RandomGenerator.RandomIntGenerate(size, lockCount);
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

                // Gate가 없는 경우를 고려한다. => Gate : nullable
                // 무시해도 되는 node의 조건.
                if(link.gate != null){
                    if(link.gate.type == GateType.LOCK) {
                        if(!gates.Contains(link.gate)){                         //아직 처리하지 못한 Gate에 대해서만 설정한다.
                            targetGate = link.gate;
                            gates.Add(link.gate);

                            AnswerKey key = MakeKey();
                            SendPairData(targetGate, key);                      // logic을 관리하는 매니저에게 전달.
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
        // MakeMark();
    }

    void MakeMark(){
        foreach(var room in activeRooms){
            Instantiate(markPrefab[searchCount], room.roomPosition, Quaternion.identity);
        }
        searchCount++;
    }

    public int spawnNodeNumber;

    // 다음에 탐색할 노드를 찾는다.
    void SearchNode(){
        if(targetGate == null){
            var key = roomDic.Keys.ToList();
            startNodeId = roomDic[key[spawnNodeNumber]].roomId;     
        
            return;
        }
        
        Link link = links.Find(v => v.gate == targetGate);
        if(link == null) return;

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
        
        // 랜덤 키 생성.
        GameObject keyPrefab = answerKeyData.key[rnd];
        AnswerKey key = Instantiate(keyPrefab).GetComponent<AnswerKey>();

        return key;
    }

    void SetKeyPosition(AnswerKey key){
        if(key is NormalKey normalKey){
            int rnd1 = UnityEngine.Random.Range(0, activeRooms.Count);

            Vector3 center = activeRooms[rnd1].roomPosition;
            normalKey.transform.position = RandomPositionCheck(center) + Vector3.up * 3f;
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

        Vector3 res = tile == null ? Vector3.zero : tile.transform.position;
        return res;
    }

    public void SendPairData(InterActiveGate gate, AnswerKey key)
    {
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

public enum GateType{ NONE = 0, LOCK, OPEN }

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