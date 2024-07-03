using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;



public class ArcherAttack : CharacterAttack
{
    private int[] attackSearchX = { 0, 0, 1, -1 };
    private int[] attackSearchY = { 1, -1, 0, 0 };

    [SerializeField]private GameObject arrowPrefab;
    private int AttackRadius = 3;
   
    public override List<CharacterComponent> GetAttackTarget(CharacterComponent onwerCharacter)
    {
        List<CharacterComponent> targetCharacters = new();

        TileComponent tile = TileManager.Instance.GetTileUnderCharacter(onwerCharacter);
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

        TileComponent tileUnderCharacter = TileManager.Instance.GetTileUnderCharacter(onwerCharacter);
        q.Enqueue(new TileNode(tileUnderCharacter.xCoordinate, tileUnderCharacter.yCoordinate));
        isVisited[tileUnderCharacter.yCoordinate, tileUnderCharacter.xCoordinate] = true;

        while (q.Count > 0)
        {
            TileNode nowNode = q.Dequeue();
            int nowX = nowNode.x;
            int nowY = nowNode.y;

            CharacterComponent otherCharacter = TileManager.Instance.Tiles[nowY, nowX].GetCharacterOnTile();
            //공격 대상 찾으면 탐색 종료
            if (otherCharacter && onwerCharacter.Team != otherCharacter.Team)
            {
                targetCharacters.Add(otherCharacter);
                return targetCharacters;
            }
            
            for (int i = 0; i < attackSearchX.Length; i++)
            {
                int attackDirX;
                //스프라이트의 방향에 따라 탐색 방향을 보정한다.
                //항상 캐릭터의 앞쪽부터 공격대상을 탐색한다.
                if (spriteRenderer.flipX)
                    attackDirX = -attackSearchX[i];
                else
                    attackDirX = attackSearchX[i];
                int newX = nowX + attackDirX;
                int newY = nowY + attackSearchY[i];
                
                //탐색 유효성 검사
                if(newX < 0 || newX >=TileManager.Instance.Col
                   || newY < 0 || newY >= TileManager.Instance.Row ) 
                    continue;
                // 맨해튼 거리로 판별
                if (Mathf.Abs(newX - tileUnderCharacter.xCoordinate) + Mathf.Abs(newY - tileUnderCharacter.yCoordinate) > AttackRadius)
                    continue;
                if(isVisited[newY,newX]) 
                    continue;
                q.Enqueue(new TileNode(newX, newY));
                isVisited[newY, newX] = true;
            }
                
        }
        return targetCharacters;
    }
        
    

    public override void Attack(List<CharacterComponent> attackTarget)
    {
        CharacterComponent enemyCharacter = attackTarget[0];
        if(enemyCharacter.transform.position.x < transform.position.x)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
                
        animator.SetTrigger("Attack");
        // 투사체 발사

        Vector3 dir = enemyCharacter.transform.position - transform.position;
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