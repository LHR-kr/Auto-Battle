using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum ETEAM
{
    Red,Blue
}


public abstract class CharacterComponent : MonoBehaviour
{
    protected float HP;
    [SerializeField] protected ETEAM team;
    [SerializeField] protected float MaxHP;
    [SerializeField] protected Animator animator;
    abstract protected void Attack();
    public ETEAM Team
    {
        get
        {
            return team;
        }
    }

    public void InitCharacter()
    {
        HP = MaxHP;
    }
    
    
    public void Act()
    {
        
    }

    private void Move()
    {
        
    }

    public void TakeDamage(float Damage)
    {
        
    }

    protected Vector2 GetTileCoordinate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward, 20.0f, LayerMask.GetMask("Tile"));

        TileComponent tile = hit.collider.GetComponent<TileComponent>();
        return tile.coordinate;
        
    }
    
}
