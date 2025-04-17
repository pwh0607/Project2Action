using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.FeedbacksForThirdParty;

public class FeedbackControl : MonoBehaviour
{
    [SerializeField] MMF_Player feedbackImpact;
    [SerializeField] MMF_Player feedbackSwingTrail;

    // 피격시 반짝임 효과.
    public void PlayImpact(){
        feedbackImpact?.PlayFeedbacks();
    }

    public void PlayerSwingTrail(bool on){
        if(feedbackSwingTrail == null) return;

        var swing= feedbackSwingTrail.GetFeedbackOfType<MMF_VisualEffect>();

        if(swing == null) return;
        
        swing.Mode = on ? MMF_VisualEffect.Modes.Play : MMF_VisualEffect.Modes.Stop;
        feedbackSwingTrail.PlayFeedbacks();
    }
}