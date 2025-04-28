using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : BehaviourSingleton<LobbyManager>
{
    protected override bool IsDontDestroy() => false;

    public void OnClickStart(){
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        LoadingController.LoadScene(nextSceneIndex);
    }

    public void OnClickExit(){
        Application.Quit();
        
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }
}
