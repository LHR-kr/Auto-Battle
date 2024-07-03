using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CharacterComponent : MonoBehaviour,  IGameTickEventListener, IGameRestartEventListener
{
    
    public void HandleGameTickEvent()
    {
        Act();
    }

    public void HandleGameRestartEvent()
    {
        
        transform.position = startPos;
        if (team == ETEAM.Red)
            spriteRenderer.flipX = false;
        else if (team == ETEAM.Blue)
            spriteRenderer.flipX = true;
    }
}
