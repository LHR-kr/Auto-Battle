using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorAttack : CharacterAttack
{
    // 12시 방향부터 시계방향으로 탐색
    private int[] AttackRangeX = { 0, 1, 1, 1, 0, -1, -1, -1 };
    private int[] AttackRangeY = { -1, -1, 0, 1, 1, 1, 0, -1 };
    
    
    public override List<CharacterComponent> GetAttackTarget()
    {
        List<CharacterComponent> attackTargets = new();
        
        TileComponent tile = character.GetTileUnderCharacter();
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
            CharacterComponent otherCharacter = TileManager.Instance.Tiles[attackYPos,attackXPos].GetCharacterOnTile();
            if(!otherCharacter) 
                continue;
            if (character.Team == otherCharacter.Team)
                continue;
            
            attackTargets.Add(otherCharacter);
        }
        return attackTargets;
    }
}
