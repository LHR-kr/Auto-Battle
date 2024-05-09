using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;


public class ArcherComponent : CharacterComponent
{
    private int[] moveX = { 0, 0, 1, -1 };
    private int[] moveY = { 1, -1, 0, 0 };

    [SerializeField]private GameObject arrowPrefab;
    private int AttackRadius = 3;
   
    protected override List<CharacterComponent> GetAttackTarget()
    {
        List<CharacterComponent> targetCharacters = new();

        TileComponent tile = GetTileUnderCharacter();
        if (!tile) return targetCharacters;

        // 가까이 있는 캐릭터부터 공격하기 위해 bfs 사용
        Queue<TileNode> q = new();
        bool[,] isVisited = new bool[TileManager.Instance.Row, TileManager.Instance.Col];
        for (int i = 0; i < TileManager.Instance.Row; i++)
        {
            for (int j = 0; j < TileManager.Instance.Col; j++)
            {
                isVisited[i, j] = false;
            }
        }

        TileComponent tileUnderCharacter = GetTileUnderCharacter();
        q.Enqueue(new TileNode(tileUnderCharacter.xCoordinate, tileUnderCharacter.yCoordinate));
        isVisited[tileUnderCharacter.yCoordinate, tileUnderCharacter.xCoordinate] = true;

        while (q.Count > 0)
        {
            TileNode nowNode = q.Dequeue();
            int nowX = nowNode.x;
            int nowY = nowNode.y;

            CharacterComponent character = TileManager.Instance.Tiles[nowY, nowX].GetCharacterOnTile();
            if (character && team != character.Team && !character.IsDead)
            {
                targetCharacters.Add(character);
                return targetCharacters;
            }
            
            for (int i = 0; i < moveX.Length; i++)
            {
                int newX = nowX + moveX[i];
                int newY = nowY + moveY[i];
                
                if(newX < 0 || newX >=TileManager.Instance.Col
                   || newY < 0 || newY >= TileManager.Instance.Row ) 
                    continue;
                if(newX < tileUnderCharacter.xCoordinate -AttackRadius || newX > tileUnderCharacter.xCoordinate + AttackRadius 
                    || newY < tileUnderCharacter.yCoordinate-AttackRadius || newY > tileUnderCharacter.yCoordinate +AttackRadius)
                    continue;
                if(isVisited[newY,newX]) 
                    continue;
                q.Enqueue(new TileNode(newX, newY));
                isVisited[newY, newX] = true;
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


    private struct TileNode
    {
        public int x;
        public int y;

        public TileNode(int newX,int newY)
        {
            x = newX;
            y = newY;
        }
        
    }
}
