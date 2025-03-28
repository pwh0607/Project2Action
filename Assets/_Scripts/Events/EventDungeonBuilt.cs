using DungeonArchitect;
using Unity.AI.Navigation;
using UnityEngine;

public class EventDungeonBuilt : DungeonEventListener
{
    public bool isAuto = false;
    public NavMeshSurface nvSurface;

    //OnValidate : Editor 코드에서 주로 사용.
    // Nav Mesh를 자동으로 부여해주는 코드.
    void OnValidate()
    {
        if(!isAuto) return;

        if(nvSurface == null) nvSurface = FindFirstObjectByType<NavMeshSurface>();

        if(nvSurface == null) nvSurface = new GameObject("NavMeshSurFace").AddComponent<NavMeshSurface>();

        
    }

    public override void OnPostDungeonBuild(Dungeon dungeon, DungeonModel model){
        if(!isAuto) return;

        base.OnPostDungeonBuild(dungeon, model);
        nvSurface?.BuildNavMesh();
    }
}
