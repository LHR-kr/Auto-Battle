using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DragableComponent : MonoBehaviour
{
    private Vector3 StartPos;
    private Vector3 PrevPos;
    void Start()
    {
        StartPos = transform.position;
    }

    private void OnMouseDrag()
    {
        transform.position = GetTouchPosition();
    }

    private void OnMouseDown()
    {
        PrevPos = transform.position;
    }
    private void OnMouseUp()
    {
        Vector3 rayStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // 마우스 클릭 뗐을 때, 마우스 커서 아래에 자기 자신이 아닌 다른 캐릭터가 있다면 드래그 드랍 안 되도록 한다.
        RaycastHit2D[] hitCharacter = Physics2D.RaycastAll(rayStartPos, transform.forward, 20.0f, LayerMask.GetMask("Character"));
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
}
