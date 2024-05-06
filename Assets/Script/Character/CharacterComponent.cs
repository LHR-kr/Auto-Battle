using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public enum ETEAM
{
    Red,Blue
}


public abstract class CharacterComponent : MonoBehaviour
{
    protected float hp;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected ETEAM team; 
    [SerializeField] protected float maxHP;
    protected Animator animator;
    abstract protected void Attack();

    public float HP
    {
        get
        {
            return hp;
        }
    }

    public float MaxHP
    {
        get
        {
            return maxHP;
        }
    }
    
    public ETEAM Team
    {
        get
        {
            return team;
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        hp = maxHP;
    }

    public void InitCharacter()
    {
        hp = maxHP;
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
        hp -= Damage;
        Debug.Log(hp);
    }

    protected TileComponent GetTileUnderCharacter()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.forward, Mathf.Infinity, LayerMask.GetMask("Tile"));
        return hit.collider.GetComponent<TileComponent>();
    }
    
}
