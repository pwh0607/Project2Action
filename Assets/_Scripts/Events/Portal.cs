using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider){
        if(collider.tag != "Player") return;

        Debug.Log("트리거 엔터!! 맵을 이동합니다.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);      
    }
}
