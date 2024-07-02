using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract partial class CharacterComponent : MonoBehaviour,  IGameTickEventListener, IGameRestartEventListener
{
    [SerializeField] protected float attackDamage;
    
    protected abstract List<CharacterComponent> GetAttackTarget();
    
    protected virtual void Attack(List<CharacterComponent> attackTarget)
    {
        foreach (CharacterComponent character in attackTarget)
        {
            if(character.transform.position.x < transform.position.x)
                FlipSpriteX(true);
            else
                FlipSpriteX(false);
            animator.SetTrigger("Attack");
            character.hpComponent.TakeDamage(attackDamage);
        }
    }

}
