using UnityEngine;
using MoreMountains.Feedbacks;
using TMPro;

public class GameManager : BehaviourSingleton<GameManager>
{
    [SerializeField] MMF_Player feedbackInformation;
    [SerializeField] TextMeshProUGUI information_TMP;
    protected override bool IsDontDestroy() => true;

    void Start()
    {
        information_TMP.text = "";
    }

    public void ShowInfo(string info, float duration = 1f){
        if(feedbackInformation.IsPlaying) feedbackInformation.StopFeedbacks();
    }
}

/*

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

*/