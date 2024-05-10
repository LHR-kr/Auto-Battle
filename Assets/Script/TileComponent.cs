using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileComponent : MonoBehaviour
{
    public int xCoordinate;
    public int yCoordinate;
    public bool isReservedByCharaterMove = true;
    public CharacterComponent GetCharacterOnTile()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.back, Mathf.Infinity, LayerMask.GetMask("Character"));

        if (!hit)
            return null;
        return hit.collider.GetComponent<CharacterComponent>();
        
    }
}
