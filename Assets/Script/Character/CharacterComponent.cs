using System;using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public abstract partial class CharacterComponent : MonoBehaviour, IGameTickEventListener, IGameRestartEventListener
{
    private Vector3 startPos;

    
    protected SpriteRenderer spriteRenderer;
    [SerializeField] protected ETEAM team; 
    protected Animator animator;
    private CharacterMove characterMove;
    private HPComponent _hpComponent;

    public HPComponent hpComponent
    {
         get{
             return _hpComponent;
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
        GameManager.SendGameRestartEvent += HandleGameRestartEvent;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        characterMove = GetComponent<CharacterMove>();
        _hpComponent = GetComponent<HPComponent>();
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

    public void Act()
    {
        List<CharacterComponent> attackTargets = GetAttackTarget();
        if(attackTargets.Count > 0)
            Attack(attackTargets);
        else
        {
            TileComponent moveTileTarget = GetMoveTargetTile();
            TileComponent tileUnderCharacter = GetTileUnderCharacter();
            if(moveTileTarget && tileUnderCharacter)
                characterMove.Move(tileUnderCharacter, moveTileTarget);
        }
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

    TileComponent GetMoveTargetTile()
    {
        //가장 가까운 적 찾는다.
        CharacterComponent movetarget = null;
        foreach (CharacterComponent character in GameManager.Instance.Characters)
        {
            if(Team == character.Team) continue;
            if (!character.isActiveAndEnabled) continue;
            if (movetarget== null)
            {
                movetarget = character;
                continue;
            }

            if ((transform.position - character.transform.position).sqrMagnitude <
                (transform.position - movetarget.transform.position).sqrMagnitude)
                movetarget = character;
        }

        return movetarget.GetTileUnderCharacter();
    }
}
