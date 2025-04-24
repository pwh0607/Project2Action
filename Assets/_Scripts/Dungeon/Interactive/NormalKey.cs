using UnityEngine;

public class NormalKey : AnswerKey
{
    public override KeyType Key => KeyType.NORMAL;

    // 키를 생성하는 스크립트에서 Data와 index를 부여한다.
    public void PickUpKey(CharacterControl player){
        player.uiControl.GetItem(this);
    }
    
    public bool Use(LockedGate gate){
        Debug.Log(" 키 사용!");
        bool complete = StageLogicManager.I.OnUseKey(gate, this);
        if(complete){
            Debug.Log("문 열기 성공!!");
            Destroy(this.gameObject);
            return true;
        }else{
            Debug.Log("옳지 못한 key 입니다.");    
            return false;
        }
    }
}
