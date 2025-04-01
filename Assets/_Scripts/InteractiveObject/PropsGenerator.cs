using System;
using System.Collections.Generic;
using UnityEngine;
using DungeonArchitect;
using DungeonArchitect.Builders.GridFlow;

public class PropsGenerator : MonoBehaviour
{
    public Dungeon dungeon;
    public GameObject testPrefab;
    void Start()
    {
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
        Debug.Log($"총 {graphNodes.Count}개의 노드가 발견됨");
        Debug.Log($"총 {graphLinks.Count}개의 링크가 발견됨");

        foreach (var node in graphNodes)
        {
            if(node.active){
                Debug.Log($"노드 ID: {node.nodeId}, 위치: {node.coord}");
                
                // room 위치 측정하기.
                Vector3 newPosition = new Vector3(node.position.x, 0, node.position.y);
                Instantiate(testPrefab, newPosition, Quaternion.identity, transform);
            }
        }


        foreach(var link in graphLinks){
            Debug.Log($"Link ID :{link.linkId}, state : {link.destinationSubNode}-{link.destination}");
        }
    }

    // [SerializeField] int count;                                 //키 혹은 문의 개수.
    // [SerializeField] public Transform spawnPosition;

    // List<Room> visited = new();                                 //방문할 수 있는 방.
    // List<RoomEdge> installableEdges = new();

    // // 1. count 수만큼 문 설치하기.

    // void Start(){

    // }
    // void Init(){
    //     // 엣지 설정
    //     // Edge edge = 
    // }

    // //이동가능 범위 체크.
    // void CheckMovableArea(){
        
    // }
}

internal class FlowLayoutNodeRoomInfo
{
}

/*
    이동가능 범위를 확장한다.
    roomNumber를 이용한 이차원 배열속의 값에 상태정보를 저장한다. => 0은 열림, 1은 잠김.
*/


//일종의 Node
[Serializable]              
public class Room{
    public int roomNumber;
    public List<RoomEdge> edges;
}


[Serializable]
public class RoomNode{
    public Vector3 roomPosition;
    public List<Door> roomDoors;
}