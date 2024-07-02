using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void Move(TileComponent tileUnderCharacter, TileComponent tileUnderTarget)
    {


        TileComponent nextTile = FindTileToMove(tileUnderCharacter, tileUnderTarget);
        if(nextTile.transform.position.x - transform.position.x <0 && !_spriteRenderer.flipX) _spriteRenderer.flipX  = true;
        else if(nextTile.transform.position.x - transform.position.x > 0 && _spriteRenderer.flipX) _spriteRenderer.flipX = false;
        
        nextTile.isReservedByCharaterMove = false;
        tileUnderCharacter.isReservedByCharaterMove = true;
        StartCoroutine(MoveCoroutine(nextTile));
    }
    
    private IEnumerator MoveCoroutine(TileComponent moveTargetTile)
    {
        Vector3 moveTargetPos = moveTargetTile.transform.position;
        float moveTime = 0.35f;
        float time = 0.0f;
        _animator.SetBool("Move", true);

        float deltaDistance = (moveTargetPos - transform.position).magnitude * Time.fixedDeltaTime / moveTime;
        while (time < moveTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTargetPos,deltaDistance);
            time += Time.fixedDeltaTime;

            yield return null;
        }
        _animator.SetBool("Move", false);
        moveTargetTile.isReservedByCharaterMove = true;
    }

    private bool IsVaildMove(int newX, int newY)
    {
        if(newX < 0 || newX >= TileManager.Instance.Col ||
           newY < 0 || newY >= TileManager.Instance.Row)
            return false;
        if(TileManager.Instance.Tiles[newY,newX].GetCharacterOnTile()) 
            return false;
        if (!TileManager.Instance.Tiles[newY, newX].isReservedByCharaterMove)
            return false;
        return true;
    }

    private TileComponent FindTileToMove(TileComponent tileUnderCharacter, TileComponent tileUnderTarget)
    {
        int[] moveX = { -1, 1, 0, 0 };
        int[] moveY = { 0, 0, 1, -1 };
        
        // target과의 거리가 가장 작은 타일로 이동한다.
        int moveDirX = 0;
        int moveDirY = 0;
        float distance = float.MaxValue;
        for (int i = 0; i < moveX.Length; i++)
        {
            int newX = tileUnderCharacter.xCoordinate + moveX[i];
            int newY = tileUnderCharacter.yCoordinate + moveY[i];
            
            // check is valid move
            if(!IsVaildMove(newX,newY)) continue;
            float newDistance = (tileUnderTarget.transform.position - TileManager.Instance.Tiles[newY,newX].transform.position).sqrMagnitude;
            if (newDistance < distance)
            {
                distance = newDistance;
                moveDirX = moveX[i];
                moveDirY = moveY[i];
            }

        }
        
        int targetTileX = tileUnderCharacter.xCoordinate + moveDirX;
        int targetTileY = tileUnderCharacter.yCoordinate + moveDirY;
        return TileManager.Instance.Tiles[targetTileY, targetTileX];
    }
}
