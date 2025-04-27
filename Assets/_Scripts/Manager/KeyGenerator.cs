using System.Collections.Generic;
using System.Linq;
using CustomInspector;
using UnityEngine;
using UnityEngine.Events;

public class KeyGenerator : PropGenerator
{
    private int spawnNodeNumber;

    [ReadOnly] public string startNodeId;
    [ReadOnly] public LockedGate targetGate = null;
    [ReadOnly] AnswerKeyData data;

    public UnityAction<LockedGate, AnswerKey> OnPairCompleted;
    int lockCount = 2;

    public KeyGenerator(DungeonContext context, AnswerKeyData data, int spawnNodeNumber): base(context)
    {
        this.spawnNodeNumber = spawnNodeNumber;
        this.data = data;
    }
    public void Register(UnityAction<LockedGate, AnswerKey> action){
        OnPairCompleted += action;
    }
    public override void Initialize()
    {
        while(lockCount > 0){
            SearchDungeon();
            lockCount--;
        }
    }

    void SearchDungeon(){
        SearchRoom();
        
        Queue<Room> queue = new();
        Room startRoom = context.roomDic[startNodeId];
        context.activeRooms.Add(startRoom);
        queue.Enqueue(startRoom);
        
        context.visited[startRoom] = true;

        while(queue.Count >= 1){
            Room r = queue.Dequeue();
            
            foreach(var node in r.n_Node){
                Link link = context.links.Find(v => (v.node.Item1.Equals(r) && v.node.Item2.Equals(node)) || (v.node.Item1.Equals(node) && v.node.Item2.Equals(r)));

                if(context.visited[node] || link == null) continue;

                if(link.gate != null){
                    if(link.gate.type == GateType.LOCK) {
                        if(!context.gates.Contains(link.gate)){                         
                            targetGate = link.gate as LockedGate;
                            context.gates.Add(link.gate);

                            AnswerKey key = MakeKey();
                            OnPairCompleted?.Invoke(targetGate, key); 
                            SetKeyPosition(key);
                        }
                        continue;
                    }    
                }

                queue.Enqueue(node);
                context.activeRooms.Add(node);
                context.visited[node] = true;
            }
        }
    }
    
    Vector3 RandomPositionCheck(Vector3 center){
        GameObject tile = null;
        
        while(tile == null){
            Vector3 sphereRandom = Random.insideUnitSphere * 8f;
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

    AnswerKey MakeKey(){
        int rnd = Random.Range(0, data.key.Count);
        
        GameObject keyPrefab = data.key[rnd];
        AnswerKey key = GameObject.Instantiate(keyPrefab).GetComponent<AnswerKey>();

        return key;
    }

    void SetKeyPosition(AnswerKey key){
        if(key is NormalKey normalKey){
            int rnd1 = Random.Range(0, context.activeRooms.Count);

            Vector3 center = context.activeRooms[rnd1].roomPosition;
            normalKey.transform.position = RandomPositionCheck(center) + Vector3.up * 0.3f;
        }else if (key is ButtonKey buttonKey){
            List<int> randomIndex = RandomGenerator.RandomIntGenerate(context.activeRooms.Count, 2);

            Vector3 center1 = Vector3.zero; 
            Vector3 center2 = Vector3.zero; 
            
            if(context.activeRooms.Count <= 1){
                center1 = context.activeRooms[0].roomPosition;
                center2 = context.activeRooms[0].roomPosition;
            }else{
                center1 = context.activeRooms[randomIndex[0]].roomPosition;
                center2 = context.activeRooms[randomIndex[1]].roomPosition;
            }

            Vector3 pos1 = RandomPositionCheck(center1) + Vector3.up;
            Vector3 pos2 = RandomPositionCheck(center2) + Vector3.up * 0.1f;

            buttonKey.SetPosition(pos1, pos2);
        }
    }

    void SearchRoom(){
        if(targetGate == null){
            var key = context.roomDic.Keys.ToList();
            startNodeId = context.roomDic[key[spawnNodeNumber]].roomId;     
        
            return;
        }
        
        Link link = context.links.Find(v => v.gate == targetGate);
        if(link == null) return;

        var room1 = link.node.Item1;
        var room2 = link.node.Item2;

        if(!context.visited[room1]){
            startNodeId = room1.roomId;
            return;
        }
        
        if(!context.visited[room2]){
            startNodeId = room2.roomId;
            return;
        }

        targetGate = null;
    }
  
}