using System;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public enum CursorType {NORMALE, INTERACT, ATTACK, DIALOGUE}

[Serializable]
public class CursorData{
    public CursorType type;
    public Texture2D texture;
    public Vector2 offset;
}

public class CursorControl : MonoBehaviour
{
    [Space(20)]
    public bool IsShowDebugCursor = false;

    // eyePoint : 플레이어 눈 위치. [카메라 포커싱]
    // cursorPoint : 마우스 커서 위치 (충돌 위치)
    // cursorFixedPoint : 실제 커서 위치를 캐릭터의 눈높이로 보정한 위치
    [ReadOnly] public Transform eyePoint;
    [Space(20), SerializeField] Transform cursorPoint;
    [SerializeField] Transform cursorFixedPoint;
    public Transform CursorFixedPoint => cursorFixedPoint;
    [SerializeField] LineRenderer line;

    private Camera cam;
    [SerializeField] CursorType cursorType = CursorType.NORMALE;

    //커서
    [Space(20)]
    [SerializeField] List<CursorData> cursors = new List<CursorData>();
    void Start()
    {       
        cam = Camera.main;

        line.enabled = IsShowDebugCursor;
        cursorPoint.GetComponent<MeshRenderer>().enabled = IsShowDebugCursor;
        cursorFixedPoint.GetComponent<MeshRenderer>().enabled = IsShowDebugCursor;

        // 커스텀 커서 적용
        SetCursor(CursorType.NORMALE);
    }

    
    [ReadOnly] public CursorSelectable currentHover;            // 현재 호버 되어있는 아이템...
    [ReadOnly] public CursorSelectable previousHover;

    void Update()
    {
        if(cam == null || eyePoint == null) return;

        previousHover = currentHover;

        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if(Physics.Raycast(ray, out var hit)) {
            currentHover = hit.collider.gameObject.GetComponent<CursorSelectable>();

            if(currentHover != previousHover)
                OnHoverEnter();

            // 위치 조정.
            cursorPoint.position = hit.point;
            cursorFixedPoint.position = new Vector3(hit.point.x, eyePoint.position.y, hit.point.z);
            transform.position = hit.point;      
            DrawLine();
        }else{
            if(previousHover != null) OnHoverExit();
            currentHover = null;
        }

    }

    void DrawLine(){
        if(IsShowDebugCursor == false) return;

        line.SetPosition(0, cursorPoint.position);
        line.SetPosition(1, cursorFixedPoint.position);
    }

    // 커서 이미지 변경
    public void SetCursor(CursorType type){
        var cursor = cursors.Find( v => v.type == type);
        if(cursor != null)
            Cursor.SetCursor(cursor.texture, cursor.offset, CursorMode.Auto);
    }

    private void OnHoverEnter(){
        if(previousHover != null){
            previousHover.gameObject.layer = LayerMask.NameToLayer("Default");
            previousHover.Select(false);
            SetCursor(CursorType.NORMALE);
        }

        if(currentHover == null) return;
        
        // Selecteable 가능한 오브젝트와만 커서-상호작용을 한다.
        var sel = currentHover.GetComponentInParent<CursorSelectable>();
        if(sel == null) return;

        if(currentHover != null){
            currentHover.Select(true);
            SetCursor(sel.type);
        }
    }

    private void OnHoverExit(){
        if(previousHover != null)
            previousHover.gameObject.layer = LayerMask.NameToLayer("Default");
    }
}