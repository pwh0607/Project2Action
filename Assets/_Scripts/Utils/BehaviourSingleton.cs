using UnityEngine;

//  Template : 틀, 형
//  관리자, 전역, 하나(유일)
public abstract class BehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T I{
        get{ 
            if(_instance == null){
                _instance = FindFirstObjectByType<T>();

                if(_instance == null){
                    GameObject go = new GameObject(typeof(T).Name);
                    _instance = go.AddComponent<T>();               //실행되면 확정
                }
            }

            return _instance;
        }
    }

    protected abstract bool IsDontDestroy();

    protected virtual void Awake()
    {
        if(I != null && I != this){
            Destroy(gameObject);
            return;
        }
    
        if(IsDontDestroy()) 
            DontDestroyOnLoad(gameObject);
    }
}