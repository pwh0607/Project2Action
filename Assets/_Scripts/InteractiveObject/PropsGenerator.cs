using System;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using DungeonArchitect;
using DungeonArchitect.Builders.GridFlow;

public class PropsGenerator : MonoBehaviour
{

    public int lockCount = 2;

    [HorizontalLine("DungeonGraph"), HideInInspector] public bool h_s1;
    public Dungeon dungeon;
    public Dictionary<string, Room> roomDic = new();
    public List<Room> rooms = new();
    public List<Link> links = new();
    [HorizontalLine(color:FixedColor.Cyan), HideInInspector] public bool h_e1;
    
    [Space(20)]
    public GameObject tmpPrefab;
    //Test
    public DoorData doorData;
 
    public Vector3 offset = new Vector3(5f,0,5f);
 
    void Start()
    {
        InitGraphData();
        InitGate();

        StartSearch();
    }

    void InitGraphData(){
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
        
        foreach(var node in graphNodes){
            // 첫번째 노드 찾기.
            if(node.active){
                spawnNodeId = node.nodeId.ToString();
                break;
            }
        }

        foreach (var node in graphNodes)
        {
            if(node.active){
                // room 위치 측정하기.          ( O )
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

    List<int> RandomIndex(int count){
        List<int> res = new List<int>();
        res.Add(1);
        res.Add(3);
        return res;
    }

    #region 그래프 탐색
    [ReadOnly] public string spawnNodeId;             // 탐색 시작 노드.

    // 활성화된 노드들 중 선택하여 Key를 설치한다.
    public Dictionary<Room, bool> visited = new();
    public List<Room> activeRooms = new();
    void InitVisited(){
        foreach(var room in roomDic){
            visited[room.Value] = false;
        }
    }

    // 1. StartSearch를 통해 첫번째로 발견한 잠긴 문과, key를 설치할 수 있는 노드들을 찾는다.
    // 2. StartSeach를 통해 activeRooms에 key를 설치할 수 있는 key를 설치한다.
    // 3. key를 설치 완료후, StartSearch를 시작하여 Gate 개수만큼 위 과정을 반복한다.

    void StartSearch(){
        Debug.Log("그래프 탐색 시작.");
        InitVisited();
        
        Queue<Room> queue = new();
        Room startRoom = roomDic[spawnNodeId];
        activeRooms.Add(startRoom);
        queue.Enqueue(startRoom);
        visited[startRoom] = true;

        InterActiveGate targetGate = null;         //이 게이트에 해당하는 키를 설치!

        while(queue.Count >= 1){
            Room r = queue.Dequeue();
            
            foreach(var node in r.n_Node){
                Link link = links.Find(v => (v.node.Item1.Equals(r) && v.node.Item2.Equals(node)) || (v.node.Item1.Equals(node) && v.node.Item2.Equals(r)));

                if(link == null || visited[node]) continue;
                //Gate가 없는 경우를 고려한다. => Gate는 nullable 이다.
                if(link.gate != null && link.gate.type == GateType.LOCK) {
                    if(targetGate == null){
                        targetGate = link.gate;
                    }
                    continue;
                }
                queue.Enqueue(node);
                activeRooms.Add(node);
                visited[node] = true;
            }
        }
        Debug.Log("그래프 탐색 종료.");
        MakeMark();
        //탐색 완료 후, 랜덤한 방에 키를 설치.
        MakeKey();
    }

    void MakeMark(){
        foreach(var room in activeRooms){
            Instantiate(tmpPrefab, room.roomPosition, Quaternion.identity);
        }
    }

    public GameObject keyPrefab;
    void MakeKey(){
        int rnd = UnityEngine.Random.Range(0,activeRooms.Count);
        Instantiate(keyPrefab, activeRooms[rnd].roomPosition + (Vector3.up *2), Quaternion.identity);
    }
    #endregion

    bool CheckWall(Collider[] colliders){
        int count = 0;
        foreach(var col in colliders){
            if(col.tag == "Wall") count++;
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