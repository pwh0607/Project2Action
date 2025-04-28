using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour
{
    static int nextScene;

    public static void LoadScene(int sceneindex)
    {
        nextScene = sceneindex;
        SceneManager.LoadScene("Scn0.Loading");
    }

    void Start()
    {
        StartCoroutine(LoadSceneProcess_Co());
    }   

    IEnumerator LoadSceneProcess_Co() {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone) {
            yield return null;

            timer += Time.unscaledDeltaTime;

            if (op.progress >= 0.9f) {
                op.allowSceneActivation = true;
            }
        }
    }
}
