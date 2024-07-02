using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract partial class CharacterComponent : MonoBehaviour,  IGameTickEventListener, IGameRestartEventListener
{
    
    public void HandleGameTickEvent()
    {
        Act();
    }

    public void HandleGameRestartEvent()
    {
        
        transform.position = startPos;
        if (team == ETEAM.Red)
            FlipSpriteX(false);
        else if (team == ETEAM.Blue)
            FlipSpriteX(true);

    }

    
}
