using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterAttack : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    protected CharacterComponent character;
    protected Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        character = GetComponent<CharacterComponent>();
        animator = GetComponent<Animator>();
    }


    
    [SerializeField] protected float attackDamage;
    
    public abstract List<CharacterComponent> GetAttackTarget();
    
    public virtual void Attack(List<CharacterComponent> attackTarget)
    {
        foreach (CharacterComponent enemyCharacter in attackTarget)
        {
            if(enemyCharacter.transform.position.x < transform.position.x)
                spriteRenderer.flipX = true;
            else
                spriteRenderer.flipX = false;
            animator.SetTrigger("Attack");
            enemyCharacter.HpComponent.TakeDamage(attackDamage);
        }
    }
}
