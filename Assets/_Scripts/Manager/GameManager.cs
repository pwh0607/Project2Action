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
        
    }

    public void ShowInfo(string info, float duration = 1f){
        if(feedbackInformation.IsPlaying) feedbackInformation.StopFeedbacks();

        information_TMP.text = info;
        feedbackInformation.GetFeedbackOfType<MMF_Pause>().PauseDuration = duration;
        feedbackInformation.PlayFeedbacks();
    }
}