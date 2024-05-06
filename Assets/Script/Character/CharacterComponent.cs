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

   
    protected abstract List<CharacterComponent> GetAttackTarget();
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
        List<CharacterComponent> attackTargets = GetAttackTarget();
        if(attackTargets.Count > 0)
            Attack(attackTargets);
        else
            Move();
        
    }
    protected virtual void Attack(List<CharacterComponent> attackTarget)
    {
        foreach (CharacterComponent character in attackTarget)
        {
            if(character.transform.position.x < transform.position.x)
                FlipSpriteX(true);
            else
                FlipSpriteX(false);
            animator.SetTrigger("Attack");
            // 데미지 
            character.TakeDamage(attackDamage);
            // 한 놈만 때림.
        }
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

    protected void FlipSpriteX(bool isFlipped)
    {
        GetComponent<SpriteRenderer>().flipX = isFlipped;
    }


    
}
