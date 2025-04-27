using DungeonArchitect;
using DungeonArchitect.Builders.GridFlow;
using UnityEngine;

public class DungeonBuilder
{
    private Dungeon dungeon;
    private DungeonContext context;
    private Vector3 offset;

    public DungeonBuilder(Dungeon dungeon, DungeonContext context, Vector3 offset)
    {
        this.dungeon = dungeon;
        this.context = context;
        this.offset = offset;
    }
    
    public void InitDungeonData(){

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
                context.roomDic[node.nodeId.ToString()] = room;
                context.rooms.Add(room);
            }
        }

        foreach(var link in graphLinks){
            Room startNode = context.roomDic[link.source.ToString()];
            Room endNode = context.roomDic[link.destination.ToString()];

            startNode.n_Node.Add(endNode);
            endNode.n_Node.Add(startNode);

            Vector3 startNodePosition = startNode.roomPosition;
            Vector3 endNodePosition = endNode.roomPosition;

            Vector3 vec = startNodePosition - endNodePosition;
            float y = Mathf.Atan2(vec.x, vec.y) * Mathf.Rad2Deg;

            Vector3 center = (startNodePosition + endNodePosition) / 2;
            Link newLink = new(startNode, endNode, center, Quaternion.Euler(new Vector3(0,y,0)));

            context.links.Add(newLink);
        }
    }    

    public void InitVisited()
    {
        foreach (var room in context.rooms)
        {
            context.visited[room] = false;
        }
    }
}