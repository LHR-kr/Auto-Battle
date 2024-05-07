using System;using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public abstract partial class CharacterComponent : MonoBehaviour, IGameStartEventListener, IGameTickEventListener, IGameRestartEvent
{
    private Vector3 startPos;
    protected float hp;
    
    [SerializeField] protected ETEAM team; 
    [SerializeField] protected float maxHP;
    protected Animator animator;
    
    public bool IsDead
    {
        get
        {
            return hp <= 0;
        }
    }
    
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
        hp = maxHP;
        startPos = transform.position;
    }

    private void OnEnable()
    {
        GameManager.SendGameTickEvent += HandleGameTickEvent;
    }

    private void OnDisable()
    {
        GameManager.SendGameTickEvent -= HandleGameTickEvent;
    }

    public void HandleGameStartEvent()
    {
        hp = maxHP;
    }

    public void HandleGameTickEvent()
    {
        Act();
    }

    public void HandleGameRestartEvent()
    {
        hp = maxHP;
        transform.position = startPos;
        if(team== ETEAM.Red)
            FlipSpriteX(false);
        else if(team == ETEAM.Blue)
            FlipSpriteX(true);
            
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
        hp -= Damage;
        if (hp <= 0) 
            this.gameObject.SetActive(false);
        
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
