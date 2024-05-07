using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract partial class CharacterComponent : MonoBehaviour, IGameStartEventListener, IGameTickEventListener, IGameRestartEvent
{
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

}
