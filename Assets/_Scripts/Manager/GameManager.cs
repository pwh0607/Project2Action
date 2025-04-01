using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

// 관리, 이벤트 송출.
public class GameManager : BehaviourSingleton<GameManager>
{
    protected override bool IsDontDestroy() => true;
    
    void OnEnable(){
        cts?.Dispose();
        cts = new();
    }

    void OnDisable(){
        cts.Cancel();

    }
    void OnDestroy()
    {
        cts.Cancel();
        cts.Dispose();
    }

    CancellationTokenSource cts = new CancellationTokenSource();
    public async UniTaskVoid DelayCallAsync(int millisec, Action onComplte){
        try{
            await UniTask.Delay(1000, cancellationToken:cts.Token);         //취소 명령이 떨어지면 이 작업도 취소하라 [할당 받기를 대기하지않고 취소할 것]
            onComplte?.Invoke();
        }
        catch(Exception e){
            Debug.LogException(e);
        }
    }

}