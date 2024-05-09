using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KnightComponent : CharacterComponent
{
    // 12시 방향부터 시계방향으로
    private int[] AttackRangeX = { 0, 1, 1, 1, 0, -1, -1, -1 };
    private int[] AttackRangeY = { -1, -1, 0, 1, 1, 1, 0, -1 };

    protected override List<CharacterComponent> GetAttackTarget()
    {
        List<CharacterComponent> attackTargets = new();
        
        TileComponent tile = GetTileUnderCharacter();
        if (!tile) return attackTargets;
        
        for (int i = 0; i < AttackRangeX.Length; i++)
        {
            int attackXPos = tile.xCoordinate + AttackRangeX[i];
            int attackYPos = tile.yCoordinate + AttackRangeY[i];

            // 공격 범위 체크
            if (attackXPos < 0 || attackXPos >= TileManager.Instance.Col
                               || attackYPos < 0 || attackYPos >= TileManager.Instance.Row)
                continue;

            // 공격 유효성 체크
            CharacterComponent character = TileManager.Instance.Tiles[attackYPos, attackXPos].GetCharacterOnTile();
            if (!character)
                continue;
            if (team == character.Team)
                continue;
            
            attackTargets.Add(character);
            break;
        }
        return attackTargets;
    }
}
