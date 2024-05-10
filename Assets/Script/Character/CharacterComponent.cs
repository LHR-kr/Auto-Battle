using System;using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public abstract partial class CharacterComponent : MonoBehaviour, IGameStartEventListener, IGameTickEventListener, IGameRestartEventListener, IGameTickEndEventListener
{
    private Vector3 startPos;
    protected float hp;
    
    protected SpriteRenderer spriteRenderer;
    [SerializeField] protected ETEAM team; 
    [SerializeField] protected float maxHP;
    protected Animator animator;

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
        GameManager.SendGameStartEvent += HandleGameStartEvent;
        GameManager.SendGameRestartEvent += HandleGameRestartEvent;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hp = maxHP;
        startPos = transform.position;
    }

    private void OnEnable()
    {
        GameManager.SendGameTickEvent += HandleGameTickEvent;
        GameManager.SendGameTickEndEvent += HandleGameTickEndEvent;
    }

    private void OnDisable()
    {
        GameManager.SendGameTickEvent -= HandleGameTickEvent;
        GameManager.SendGameTickEndEvent -= HandleGameTickEndEvent;
    }

    public void Act()
    {
        List<CharacterComponent> attackTargets = GetAttackTarget();
        if(attackTargets.Count > 0)
            Attack(attackTargets);
        else
            Move();
        
    }

   

    public void TakeDamage(float Damage)
    {
        hp = Mathf.Max(0, hp - Damage);
    }

    protected TileComponent GetTileUnderCharacter()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.forward, Mathf.Infinity, LayerMask.GetMask("Tile"));
        return hit.collider.GetComponent<TileComponent>();
    }

    protected void FlipSpriteX(bool isFlipped)
    {
        spriteRenderer.flipX = isFlipped;
    }
}
