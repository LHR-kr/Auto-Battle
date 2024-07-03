using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KnightAttack : CharacterAttack
{
    // 12시 방향부터 시계방향으로 탐색
    private int[] AttackRangeX = { 0, 1, 1, 1, 0, -1, -1, -1 };
    private int[] AttackRangeY = { -1, -1, 0, 1, 1, 1, 0, -1 };

    public override List<CharacterComponent> GetAttackTarget()
    {
        List<CharacterComponent> attackTargets = new();
        
        TileComponent tile = TileManager.Instance.GetTileUnderCharacter(character);
        if (!tile) return attackTargets;
        
        for (int i = 0; i < AttackRangeX.Length; i++)
        {
            //스프라이트의 방향에 따라 탐색 방향을 보정한다.
            //항상 캐릭터의 앞쪽부터 공격대상을 탐색한다.
            int attackDirX;
            if(spriteRenderer.flipX)
                attackDirX = -AttackRangeX[i];
            else
                attackDirX = AttackRangeX[i];
            int attackXPos = tile.xCoordinate + attackDirX;
            int attackYPos = tile.yCoordinate + AttackRangeY[i];

            // 공격 범위 체크
            if (attackXPos < 0 || attackXPos >= TileManager.Instance.Col
                               || attackYPos < 0 || attackYPos >= TileManager.Instance.Row)
                continue;

            // 공격 유효성 체크
            CharacterComponent otherCharacter = TileManager.Instance.Tiles[attackYPos, attackXPos].GetCharacterOnTile();
            if (!otherCharacter)
                continue;
            if (character.Team == otherCharacter.Team)
                continue;
            
            attackTargets.Add(otherCharacter);
            break;
        }
        return attackTargets;
    }
}
