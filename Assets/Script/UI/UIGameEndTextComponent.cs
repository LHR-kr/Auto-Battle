using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

public class UIGameEndTextComponent : MonoBehaviour
{
    TextMeshProUGUI textUI;
    private void Awake()
    {
        textUI = GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        string text = "Game End!\n";
        ETEAM winnerTeam = GameManager.Instance.GetWinnerTeam();
        switch (winnerTeam)
        {
            case ETEAM.Red:
                textUI.color = Color.red;
                text += "Red Team Win!";
                break;
            case ETEAM.Blue:
                textUI.color = Color.blue;
                text += "Blue Team Win!";
                break;
            case ETEAM.None:
                textUI.color = Color.white;
                text += "Draw!";
                break;
            default:
                break;
        }
        textUI.SetText(text);
    }
}
