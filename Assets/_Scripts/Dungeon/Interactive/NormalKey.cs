using UnityEngine;

public class NormalKey : AnswerKey
{
    public override KeyType Key => KeyType.NORMAL;
    public void PickUpKey(){

    }
    
    public void Use(InterActiveGate gate){
        bool complete = StageLogicManager.I.UseKey(gate, this);
        if(complete){
            Debug.Log("문 열기 성공!!");
            Destroy(this.gameObject);
        }else{
            Debug.Log("옳지 못한 key 입니다.");    
        }
    }
}
