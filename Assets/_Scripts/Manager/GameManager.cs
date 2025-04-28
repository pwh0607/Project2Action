using UnityEngine;
using MoreMountains.Feedbacks;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : BehaviourSingleton<GameManager>
{
    protected override bool IsDontDestroy() => true;
    
    [SerializeField] MMF_Player feedbackInformation;
    [SerializeField] MMF_Player feedbackFinish;

    [SerializeField] TextMeshProUGUI information_TMP;
    
    public void ShowInfo(string info, float duration = 1f){

        if(feedbackInformation.IsPlaying) feedbackInformation.StopFeedbacks();

        information_TMP.text = info;
        feedbackInformation.GetFeedbackOfType<MMF_Pause>().PauseDuration = duration;
        feedbackInformation.PlayFeedbacks();
    }

    public void GameOver(float duration = 10f){
        if(feedbackFinish.IsPlaying) feedbackFinish.StopFeedbacks();
        feedbackFinish.GetFeedbackOfType<MMF_Pause>().PauseDuration = duration;
        feedbackFinish.PlayFeedbacks();

        StartCoroutine(LoadLobby_Co());
    }

    IEnumerator LoadLobby_Co(){
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }
}