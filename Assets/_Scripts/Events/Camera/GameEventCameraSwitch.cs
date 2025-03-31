using UnityEngine;
using Unity.Cinemachine;

[CreateAssetMenu(menuName = "GameEvent/GameEventCameraSwitch")]
public class GameEventCameraSwitch : GameEvent<GameEventCameraSwitch>
{
    public override GameEventCameraSwitch Item => this;

    //필요한 데이터 추가
    //캐릭터 진입시 카메라 스위칭용.
    public bool inout;

}