using UnityEngine;
using UnityEngine.InputSystem;

public class CursorControl : MonoBehaviour
{
    [Space(20)]
    public bool IsShow = false;

    // eyePoint : 플레이어 눈 위치.
    // hitPoint : 마우스와 레벨 충돌 위치.
    // cursorPoint : 마우스와 레벨 충돌 위치를 플레이어 눈높이 맞게 수정.
    [Space(20), SerializeField] Transform hitPoint, cursorPoint; 
    public Transform eyePoint;
    public Transform CursorPoint {get => transform; set => eyePoint = value;}
    private LineRenderer line;

    private Camera cam;

    void Start()
    {       
        cam = Camera.main;
        if(!TryGetComponent(out line)){
            Debug.Log("LineRenderer 없음");
        }

        line.enabled = IsShow;
        hitPoint.GetComponent<MeshRenderer>().enabled = IsShow;
        cursorPoint.GetComponent<MeshRenderer>().enabled = IsShow;
    }

    void Update()
    {
        if(cam == null || eyePoint == null) return;
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(Physics.Raycast(ray, out var hit)) {
            hitPoint.position = hit.point;
            cursorPoint.position = new Vector3(hit.point.x, eyePoint.position.y, hit.point.z);
            transform.position = hit.point;
        }
        DrawLine();
    }

    void DrawLine(){
        if(IsShow == false) return;

        line.SetPosition(0, hitPoint.position);
        line.SetPosition(1, cursorPoint.position);
    }
}