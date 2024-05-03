using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileComponent : MonoBehaviour
{
    private bool isPlayerOn;
    private Vector2 coordinate;

    void Start()
    {
        coordinate.x = transform.position.x;
        coordinate.y = transform.position.y;
    }

    public bool IsPlayerOn
    {
        get => isPlayerOn;
        set => isPlayerOn = value;
    }
    

    public Vector2 Coordinate
    {
        get => coordinate;
        set => coordinate = value;
    }
}
