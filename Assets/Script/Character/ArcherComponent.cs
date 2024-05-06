using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ArcherComponent : CharacterComponent
{

    [SerializeField]private GameObject arrowPrefab;
    private int AttackRadius = 3;
    private int[] AttackRangeX = { 0, 1, 1, 1, 0, -1, -1, -1 };
    private int[] AttackRangeY = { -1, -1, 0, 1, 1, 1, 0, -1 };


    protected override List<CharacterComponent> GetAttackTarget()
    {
        List<CharacterComponent> targetCharacters = new();

        TileComponent tile = GetTileUnderCharacter();
        if (!tile) return targetCharacters;


        for (int radius = 1; radius < AttackRadius + 1; radius++)
        {
            for (int i = 0; i < AttackRangeX.Length; i++)
            {
                //가까운 곳부터 공격
                int attackXPos = tile.xCoordinate + AttackRangeX[i] * radius;
                int attackYPos = tile.yCoordinate + AttackRangeY[i] * radius;

                if (attackXPos < 0 || attackXPos >= TileManager.Instance.Col
                                   || attackYPos < 0 || attackYPos >= TileManager.Instance.Row)
                    continue;

                // 공격 유효성 체크
                CharacterComponent character = TileManager.Instance.Tiles[attackYPos, attackXPos].GetCharacterOnTile();
                if (!character)
                    continue;
                if (team == character.Team)
                    continue;
                targetCharacters.Add(character);
            }
        }
        return targetCharacters;
    }

    protected override void Attack(List<CharacterComponent> attackTarget)
    {
        CharacterComponent character = attackTarget[0];
        if(character.transform.position.x < transform.position.x)
            FlipSpriteX(true);
        else
            FlipSpriteX(false);
                
        animator.SetTrigger("Attack");
        // 투사체 발사

        Vector3 dir = character.transform.position - transform.position;
        ArrowComponent arrow = Instantiate(arrowPrefab, transform.position,
            Quaternion.FromToRotation(Vector3.right, dir)).GetComponent<ArrowComponent>();
        arrow.AttackDamage = attackDamage;
        arrow.Owner = this.gameObject;
        return;
    }
}
