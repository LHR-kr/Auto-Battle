using System;using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public partial class CharacterComponent : MonoBehaviour, IGameTickEventListener, IGameRestartEventListener
{
    private Vector3 startPos;

    
    protected SpriteRenderer spriteRenderer;
    [SerializeField] protected ETEAM team; 
    private CharacterMove _characterMove;
    private HPComponent _hpComponent;
    private CharacterAttack _characterAttack;

    public HPComponent HpComponent
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
        spriteRenderer = GetComponent<SpriteRenderer>();
        _characterMove = GetComponent<CharacterMove>();
        _characterAttack = GetComponent<CharacterAttack>();
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
        List<CharacterComponent> attackTargets = _characterAttack.GetAttackTarget(this);
        if(attackTargets.Count > 0)
            _characterAttack.Attack(attackTargets);
        else
        {
            TileComponent moveTileTarget = _characterMove.FindMoveTargetTile(this);
            TileComponent tileUnderCharacter = TileManager.Instance.GetTileUnderCharacter(this);
            if(moveTileTarget && tileUnderCharacter)
                _characterMove.Move(tileUnderCharacter, moveTileTarget);
        }
    }
    
}
