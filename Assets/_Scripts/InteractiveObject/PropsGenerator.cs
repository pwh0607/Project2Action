using System;
using System.Collections.Generic;
using UnityEngine;
using DungeonArchitect;
using DungeonArchitect.Builders.GridFlow;
using System.Linq;

public class PropsGenerator : MonoBehaviour
{
    public Dictionary<string, Room> rooms = new();
    public List<Link> links = new();
    
    public Dictionary<(string, string), Link> dungeonGraph = new();
    // Dictionary<string, Vector3> roomPosition = new();

    public Dungeon dungeon;
    public int lockCount = 2;
    public GameObject tmpPrefab;
    //Test
    public DoorData doorData;
 
    public Vector3 offset = new Vector3(5f,0,5f);
 
    void Start()
    {
        InitGraphData();
        InitDoor();
    }

    void InitDoor(){
        List<int> indexes = RandomIndex(links.Count);
        for(int i=0;i<links.Count;i++){
            // link 양옆에 Wall이 없으면 유효한 door의 위치가 아니므로 제거.
            if(!CheckWall(Physics.OverlapSphere(links[i].linkPosition, 5f))) continue;
            
            if(indexes.Contains(i)){
               Instantiate(doorData.lockedGate, links[i].linkPosition, links[i].quaternion);
               links[i].isLocked = true;          //잠김.
            }else{
               Instantiate(doorData.openedGate, links[i].linkPosition, links[i].quaternion);    
            }
        }
    }

    List<int> RandomIndex(int count){
        List<int> res = new List<int>();
        res.Add(1);
        res.Add(3);
        return res;
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

        foreach (var node in graphNodes)
        {
            if(node.active){
                // room 위치 측정하기.          ( O )
                Vector3 nodePosition = new Vector3(node.coord.x * 20, 0, node.coord.y * 20) + offset;

                Room room = new Room(node.nodeId.ToString(), nodePosition);
                rooms[node.nodeId.ToString()] = room;
            }
        }

        foreach(var link in graphLinks){
            string startNodeId = link.source.ToString();
            string endNodeId = link.destination.ToString();

            // 인접 노드 생성.
            rooms[startNodeId].n_Node.Add(rooms[endNodeId]);
            rooms[endNodeId].n_Node.Add(rooms[startNodeId]);

            Vector3 startNodePosition = rooms[startNodeId].roomPosition;
            Vector3 endNodePosition = rooms[endNodeId].roomPosition;

            //Rotation Check.
            Vector3 vec = startNodePosition - endNodePosition;
            float y = Mathf.Atan2(vec.x, vec.y) * Mathf.Rad2Deg;

            //이 두 포지션의 중심이 link의 위치.
            Vector3 center = (startNodePosition + endNodePosition) / 2;
            Link newLink = new(startNodeId, endNodeId, center, Quaternion.Euler(new Vector3(0,y,0)));

            links.Add(newLink);
            
            //Dictionary형 그래프 생성.
            dungeonGraph[(startNodeId, endNodeId)] = newLink;
        }
    }

    #region 그래프 탐색
    string spawnNodeId = "startNode";             // 탐색 시작 노드.
    public List<Room> visitedRoom;

    #endregion



    bool CheckWall(Collider[] colliders){
        int count = 0;
        foreach(var col in colliders){
            if(col.tag == "Wall") count++;
        }
        return count >= 1;
    }
}




//일종의 Node
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

public class Link{
    public string startNodeId;
    public string endNodeId;
    public Vector3 linkPosition;
    public Quaternion quaternion;
    public bool isLocked;
    public Link(string startNodeId, string endNodeId, Vector3 linkPosition, Quaternion quaternion, bool isLocked = false){
        this.startNodeId = startNodeId;
        this.endNodeId = endNodeId;

        this.linkPosition = linkPosition;
        this.quaternion = quaternion;
        
        this.isLocked = isLocked;
    }
}