using System;
using System.Collections.Generic;
using UnityEngine;
using DungeonArchitect;
using DungeonArchitect.Builders.GridFlow;
using System.Linq;

public class PropsGenerator : MonoBehaviour
{
    public List<Room> rooms = new();
    public List<Link> links = new();

    Dictionary<string, Vector3> roomPosition = new();
    public Dungeon dungeon;
    public int lockCount = 2;
    //Test
    public DoorData doorData;
 
    public float offset = 8f;          //cellSize * 2f;
        
 
    void Start()
    {
        InitGraphData();
        InitDoor();
    }

    void InitDoor(){
        List<int> indexes = new();
        for(int i=0;i<lockCount;i++){
            int idx = -1;       //UnityEngine.Random.Range(0,links.Count);
            // Find로 값 찾기.
        }
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
                Vector3 nodePosition = new Vector3(node.coord.x * 16 + offset, 0, node.coord.y * 16 + offset);

                Room room = new Room(node.nodeId.ToString(), nodePosition);
                rooms.Add(room);
                roomPosition[room.roomId] = room.roomPosition;
            }
        }

        foreach(var link in graphLinks){
            string startNodeId = link.source.ToString();
            string endNodeId = link.destination.ToString();

            Vector3 startNodePosition = roomPosition[startNodeId];
            Vector3 endNodePosition = roomPosition[endNodeId];

            //이 두 포지션의 중심이 link의 위치.
            Vector3 center = (startNodePosition + endNodePosition) / 2;
            Link newLink = new(startNodeId, endNodeId, center + Vector3.right * 2);
            links.Add(newLink);
        }
    }
}


//일종의 Node
[Serializable]              
public class Room{
    public string roomId;
    public Vector3 roomPosition;
    public Room(string id, Vector3 position){
        this.roomId = id;
        this.roomPosition = position;
    }
}

public class Link{
    public string startNodeId;
    public string endNodeId;
    public Vector3 linkPosition;

    public Link(string startNodeId, string endNodeId, Vector3 linkPosition){
        this.startNodeId = startNodeId;
        this.endNodeId = endNodeId;
        this.linkPosition = linkPosition;
    }
}