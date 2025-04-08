// abstract와의 차이
/*
    abstract        |          interface
    직계 상속        |          먼 친척.
    변수 선언 가능   |          변수 선언 불가
*/
public interface IActorControl
{
    public ActorProfile Profile{get; set;}              //** {get; set;} 형태로 자식이 반드시 정의하도록 제한을 한다.
}