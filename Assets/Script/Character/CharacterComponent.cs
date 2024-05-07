using System;using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public abstract class CharacterComponent : MonoBehaviour, IGameStartEventListener, IGameTickEventListener
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
        GameManager.SendGameStartEvent += HandleGameStartEvent;
        
        animator = GetComponent<Animator>();
        hp = maxHP;
    }

    private void OnEnable()
    {
        GameManager.SendGameTickEvent += HandleGameTickEvent;
    }

    private void OnDisable()
    {
        GameManager.SendGameTickEvent -= HandleGameTickEvent;
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
            character.TakeDamage(attackDamage);
        }
    }
    private void Move()
    {
        //게임 보드가 가로로 더 넓으니, 가로 방향으로 우선 이동할 수 있도록 순서 배치
        int[] moveX = { -1, 1, 0, 0 };
        int[] moveY = { 0, 0, 1, -1 };
        
        //가장 가까운 적 찾는다.
        CharacterComponent movetarget = null;
        foreach (CharacterComponent character in GameManager.Instance.Characters)
        {
            if(Team == character.Team) continue;
            if(character.IsDead) continue;
            if (movetarget== null)
            {
                movetarget = character;
                continue;
            }

            if ((transform.position - character.transform.position).sqrMagnitude <
                (transform.position - movetarget.transform.position).sqrMagnitude)
                movetarget = character;
        }

        if (movetarget == null) return;
        TileComponent tileUnderCharacter = GetTileUnderCharacter();
        TileComponent tileUnderMoveTarget = movetarget.GetTileUnderCharacter();
        
        // Manhattan Distance가 가장 작은 방향으로 이동한다.
        int moveDirX = 0;
        int moveDirY = 0;
        int manhattanDistance = int.MaxValue;
        for (int i = 0; i < moveX.Length; i++)
        {
            int newX = tileUnderCharacter.xCoordinate + moveX[i];
            int newY = tileUnderCharacter.yCoordinate + moveY[i];
            
            // check is valid move
            if(newX < 0 || newX >= TileStart.Instance.Col ||
               newY < 0 || newY >= TileStart.Instance.Row)
                continue;
            if(TileStart.Instance.Tiles[newY,newX].GetCharacterOnTile()) 
                continue;
            if (!TileStart.Instance.Tiles[newY, newX].isValidTile)
                continue;
            int newManhattanDistance = Mathf.Abs(tileUnderMoveTarget.xCoordinate - newX) +
                                       Mathf.Abs(tileUnderMoveTarget.yCoordinate - newY);
            if (newManhattanDistance < manhattanDistance)
            {
                manhattanDistance = newManhattanDistance;
                moveDirX = moveX[i];
                moveDirY = moveY[i];
            }

        }

        if(moveDirX<0 ) FlipSpriteX(true);
        else FlipSpriteX(false);
        
        Vector3 movePos = TileStart.Instance
            .Tiles[tileUnderCharacter.yCoordinate + moveDirY, tileUnderCharacter.xCoordinate + moveDirX].transform.position;
        TileStart.Instance.Tiles[tileUnderCharacter.yCoordinate + moveDirY, tileUnderCharacter.xCoordinate + moveDirX]
            .isValidTile = false;
        TileStart.Instance.Tiles[tileUnderCharacter.yCoordinate, tileUnderCharacter.xCoordinate].isValidTile = true;
        StartCoroutine(MoveCoroutine(movePos));
    }

    public void TakeDamage(float Damage)
    {
        hp -= Damage;
        if(hp<=0)
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

    public bool IsDead
    {
        get
        {
            return hp <= 0;
        }
    }

    private IEnumerator MoveCoroutine(Vector3 moveTargetPos)
    {
        float moveTime = 0.7f;
        float time = 0.0f;
        animator.SetBool("Move", true);

        float deltaDistance = (moveTargetPos - transform.position).magnitude * Time.fixedDeltaTime / moveTime;
        while (time < moveTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTargetPos,deltaDistance);
            time += Time.fixedDeltaTime;

            yield return null;
        }
        animator.SetBool("Move", false);
    }

    public void HandleGameStartEvent()
    {
        hp = maxHP;
    }

    public void HandleGameTickEvent()
    {
        Act();
    }
}
