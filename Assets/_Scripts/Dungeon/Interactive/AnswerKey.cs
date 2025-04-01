using UnityEngine;

public class AnswerKey : InteractiveObject
{
    public void Use(Door door){
        bool complete = StageLogicManager.I.UseKey(door, this);
        if(complete){
            Debug.Log("문 열기 성공!!");
            Destroy(this.gameObject);
        }else{
            Debug.Log("옳지 못한 key 입니다.");    
        }
    }
}
