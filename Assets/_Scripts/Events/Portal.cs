using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider){
        if(collider.tag != "Player") return;

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        LoadingController.LoadScene(nextSceneIndex);
    }
}
