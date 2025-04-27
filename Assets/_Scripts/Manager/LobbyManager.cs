using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : BehaviourSingleton<LobbyManager>
{
    protected override bool IsDontDestroy() => false;

    public void OnClickStart(){
        int nextSceneIndext = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndext);
    }

    public void OnClickExit(){
        Application.Quit();
        
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }
}
