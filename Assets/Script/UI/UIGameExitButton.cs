using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameExitButton : MonoBehaviour
{
    public void HandleGameStartEvent()
    {
        hp = maxHP;
    }
    public void HandleGameTickEvent()
    {
        Act();
    }

    public void HandleGameRestartEvent()
    {
        hp = maxHP;
        transform.position = startPos;
        if(team== ETEAM.Red)
            FlipSpriteX(false);
        else if(team == ETEAM.Blue)
            FlipSpriteX(true);
            
    }

    public void HandleGameTickEndEvent()
    {
        if(hp<= 0)
            this.gameObject.SetActive(false);
    }
}
