using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileComponent : MonoBehaviour, IComparable
{
    
    public Vector2 coordinate;
    public CharacterComponent gameObjectOnTile;
    public bool isPlayerOn;

    public int idx;
    public int CompareTo(object obj)
    {
        if (transform.position.y > (obj as TileComponent).transform.position.y)
            return -1;
        else if (transform.position.y == (obj as TileComponent).transform.position.y
        && transform.position.x < (obj as TileComponent).transform.position.x)
            return -1;
        else
            return 1;
    }

    public CharacterComponent GetCharacterOnTile()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward, Mathf.Infinity, LayerMask.GetMask("Character"));
        return hit.collider.GetComponent<CharacterComponent>();
        
    }
}
