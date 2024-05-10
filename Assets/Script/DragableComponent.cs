using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DragableComponent : MonoBehaviour, IGameStartEventListener,IGameRestartEventListener
{
    private Vector3 StartPos;
    private Vector3 PrevPos;

    private bool isDragable;
    void Start()
    {
        StartPos = transform.position;
        isDragable = true;
        GameManager.SendGameStartEvent += HandleGameStartEvent;
        GameManager.SendGameRestartEvent += HandleGameRestartEvent;
    }

    private void OnMouseDrag()
    {
        if (!isDragable) return;
        transform.position = GetTouchPosition();
    }

    private void OnMouseDown()
    {
        if (!isDragable) return;
        PrevPos = transform.position;
    }
    private void OnMouseUp()
    {
        if (!isDragable) return;
        Vector3 rayStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // 마우스 클릭 뗐을 때, 마우스 커서 아래에 자기 자신이 아닌 다른 캐릭터가 있다면 드래그 드랍 안 되도록 한다.
        RaycastHit2D[] hitCharacter = Physics2D.RaycastAll(rayStartPos, transform.forward, Mathf.Infinity, LayerMask.GetMask("Character"));
        foreach(RaycastHit2D hit in hitCharacter)
        {
            if (hit.collider.gameObject != this.gameObject)
            {
                transform.position = PrevPos;
                return;
            }
        }
        
        
        RaycastHit2D hitTile = Physics2D.Raycast(rayStartPos, transform.forward, 20.0f, LayerMask.GetMask("Tile"));
        if (hitTile)
        {
            Vector3 newPos = new Vector3(hitTile.transform.position.x, hitTile.transform.position.y,StartPos.z);
            transform.position = newPos;
        }
        else
        {
            transform.position = PrevPos;
        }
    }

    private Vector3 GetTouchPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = StartPos.z;

        return mousePos; 
    }
    public void HandleGameStartEvent()
    {
        isDragable = false;
    }

    public void HandleGameRestartEvent()
    {
        isDragable = true;
    }
}
