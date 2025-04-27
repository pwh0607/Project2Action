using UnityEngine;

public class BGMManager : MonoBehaviour
{
    private AudioSource bgm;

    void Start()
    {
        TryGetComponent(out bgm);
    }

    void SetVolum(float volum){
        bgm.volume = volum;
    }
}
