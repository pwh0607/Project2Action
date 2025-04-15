using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

public class PoolableFeedback : PoolBehaviour
{
    [SerializeField] MMF_Player feedback;
    [SerializeField] TextMeshPro tmp;
    
    void Awake()
    {

    }

    void OnEnable()
    {
        feedback.RestoreInitialValues();
        feedback.ResetFeedbacks();
        feedback.PlayFeedbacks();
    }

    void OnDisable()
    {
        Despawn();
    }

    public void Deactivate(){
        gameObject.SetActive(false);
    }

    public void SetText(string text){
        tmp.text =text;
    }
}