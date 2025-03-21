using UnityEngine;

public class TriggerCube : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
        Debug.Log("공격 위치 도착");
    }

    void OnTriggerExit(Collider other){
        Debug.Log("공격 위치 탈출");
    }
}