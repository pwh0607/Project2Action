using System.Collections;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    private Transform mainCam;
    [SerializeField] Transform Offset;
    [SerializeField] float offsetForce = 1f;
    IEnumerator Start()
    {
        yield return new WaitUntil(() => Camera.main != null);
        mainCam = Camera.main.transform;
    }

    void Update()
    {   
        if(mainCam == null) return;

        Vector3 direction = (mainCam.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(-direction, mainCam.transform.up);

        if(Offset == null) return;
            Offset.position = transform.position - mainCam.forward * offsetForce;
    }
}   