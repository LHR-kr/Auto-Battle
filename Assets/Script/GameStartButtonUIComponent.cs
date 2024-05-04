using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartButtonUI : MonoBehaviour
{
    public void OnClickButton()
    {
        GameManager.Instance.StartGame();
    }
    
}
