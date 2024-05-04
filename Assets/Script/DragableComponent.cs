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
        RaycastHit2D[] hit = Physics2D.RaycastAll(rayStartPos, transform.forward, 20.0f, LayerMask.GetMask("Tile"));
        if (hit.Length > 0)
        {
            Vector3 newPos = new Vector3(hit[0].transform.position.x, hit[0].transform.position.y,StartPos.z);
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
