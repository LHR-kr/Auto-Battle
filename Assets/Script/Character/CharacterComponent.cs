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
    [SerializeField] protected float attackDamage;
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
        Attack();
    }

    private void Move()
    {
        
    }

    public void TakeDamage(float Damage)
    {
        HP -= Damage;
        Debug.Log(HP);
    }

    protected TileComponent GetTileUnderCharacter()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.forward, Mathf.Infinity, LayerMask.GetMask("Tile"));
        return hit.collider.GetComponent<TileComponent>();
    }
    
}
