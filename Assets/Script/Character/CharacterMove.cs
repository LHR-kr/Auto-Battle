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
    
    public void Move(TileComponent tileUnderCharacter, TileComponent tileUnderMoveTarget)
    {
        //게임 보드가 가로로 더 넓으니, 가로 방향으로 우선 이동할 수 있도록 순서 배치
        int[] moveX = { -1, 1, 0, 0 };
        int[] moveY = { 0, 0, 1, -1 };
        
        // 이동 후, target과의 거리가 가장 작은 타일로 이동한다.
        int moveDirX = 0;
        int moveDirY = 0;
        float distance = float.MaxValue;
        for (int i = 0; i < moveX.Length; i++)
        {
            int newX = tileUnderCharacter.xCoordinate + moveX[i];
            int newY = tileUnderCharacter.yCoordinate + moveY[i];
            
            // check is valid move
            if(newX < 0 || newX >= TileManager.Instance.Col ||
               newY < 0 || newY >= TileManager.Instance.Row)
                continue;
            if(TileManager.Instance.Tiles[newY,newX].GetCharacterOnTile()) 
                continue;
            if (!TileManager.Instance.Tiles[newY, newX].isReservedByCharaterMove)
                continue;
            float newDistance = (tileUnderMoveTarget.transform.position - TileManager.Instance.Tiles[newY,newX].transform.position).sqrMagnitude;
            if (newDistance < distance)
            {
                distance = newDistance;
                moveDirX = moveX[i];
                moveDirY = moveY[i];
            }

        }

        if(moveDirX<0 && !_spriteRenderer.flipX) _spriteRenderer.flipX  = true;
        else if(moveDirX > 0 && _spriteRenderer.flipX) _spriteRenderer.flipX = false;

        int targetTileX = tileUnderCharacter.xCoordinate + moveDirX;
        int targetTileY = tileUnderCharacter.yCoordinate + moveDirY;
        
        TileManager.Instance.Tiles[targetTileY, targetTileX].isReservedByCharaterMove = false;
        TileManager.Instance.Tiles[tileUnderCharacter.yCoordinate, tileUnderCharacter.xCoordinate].isReservedByCharaterMove = true;
        StartCoroutine(MoveCoroutine(TileManager.Instance.Tiles[targetTileY, targetTileX]));
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

}
