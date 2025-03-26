using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : BehaviourSingleton<PoolManager>
{
    protected override bool IsDontDestroy() => false;

    private Dictionary<PoolBehaviour, ObjectPool<PoolBehaviour>> prefabs;
    private Dictionary<PoolBehaviour, ObjectPool<PoolBehaviour>> instances;

    protected override void Awake()
    {
        base.Awake();

        prefabs = new Dictionary<PoolBehaviour, ObjectPool<PoolBehaviour>>();
        instances = new Dictionary<PoolBehaviour, ObjectPool<PoolBehaviour>>();
    }

    public void ClearPool(){
        prefabs.ToList().ForEach(p => p.Value.Clear());
        prefabs.Clear();


        instances.ToList().ForEach(i => i.Value.Clear());
        instances.Clear();
    }

    public void WarmPool(PoolBehaviour pb, int size = 10){
        if(prefabs.ContainsKey(pb)){
            Debug.LogWarning($"이미 생성한 프리팹입니다. {pb.name}");
        }

        var pool = new ObjectPool<PoolBehaviour>(
            createFunc: () => {
                PoolBehaviour p = Instantiate(pb);
                p.poolManager = this;
                return p;
            },
            actionOnGet: (v) => {
                v.gameObject.SetActive(true);
            },
            actionOnRelease : (v) =>{
                v.gameObject.SetActive(false);
            },
            actionOnDestroy: (v) => {
                Destroy(v.gameObject);
            },
            maxSize: size
        );
        prefabs[pb] = pool;
    }

    public PoolBehaviour Spawn(PoolBehaviour pb, Vector3 pos, Quaternion rot, Transform parent){
        if(!prefabs.ContainsKey(pb)){
            WarmPool(pb);
        }

        var pool = prefabs[pb];
        var clone = pool.Get();
        clone.transform.SetPositionAndRotation(pos, rot);
        clone.transform.SetParent(parent ?? transform);         // ?? parent가 null이 아니면 parent를 쓰고, parent가 null이면 transform을 써라!
        
        instances[clone] = pool;

        return clone;
    }

    public void Despawn(PoolBehaviour pb){
        if(instances.ContainsKey(pb) == false){
            Debug.LogWarning($"PoolManager ] 오브젝트 풀에 {pb.name} 없음");
            return;
        }
        
        instances[pb].Release(pb);
        instances.Remove(pb);
    }
}