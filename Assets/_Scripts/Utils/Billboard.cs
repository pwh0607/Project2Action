using UnityEngine;

public class Billboard : MonoBehaviour
{

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        Vector3 direction = (mainCam.transform.position - transform.position).normalized;
        transform.rotation =  Quaternion.LookRotation(-direction, mainCam.transform.up);
    }
}   