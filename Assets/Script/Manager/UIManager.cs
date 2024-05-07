using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour, IGameStartEventListener, IGameTickEventListener, IGameEndEventListener, IGameRestartEvent
{
    private static UIManager instance = null;
    [SerializeField] private Button GameStartButton;
    [SerializeField] private TextMeshProUGUI RemainingTimeText;
    [SerializeField] private GameObject GameEndUIObject;
    [SerializeField] private TextMeshProUGUI GameEndText;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    public static UIManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Start()
    {
        GameManager.SendGameStartEvent += HandleGameStartEvent;
        GameManager.SendGameTickEvent += HandleGameTickEvent;
        GameManager.SendGameEndEvent += HandleGameEndEvent;
        GameManager.SendGameRestartEvent += HandleGameRestartEvent;
    }

    public void HandleGameStartEvent()
    {
        GameStartButton.gameObject.SetActive(false);
        GameEndUIObject.SetActive(false);
        RemainingTimeText.gameObject.SetActive(true);
        RemainingTimeText.SetText(GameManager.Instance.RemainingTime.ToString());
    }

    public void HandleGameTickEvent()
    {
        RemainingTimeText.SetText(GameManager.Instance.RemainingTime.ToString());
    }

    public void HandleGameEndEvent(ETEAM winner)
    {
        string text = "Game End!\n";
        RemainingTimeText.gameObject.SetActive(false);

        switch (winner)
        {
            case ETEAM.Red:
                text += "Red Team Win!";
                break;
            case ETEAM.Blue:
                text += "Blue Team Win!";
                break;
            case ETEAM.None:
                text += "Draw!";
                break;
            default:
                break;
        }
        GameEndText.SetText(text);
        GameEndUIObject.SetActive(true);
    }

    public void HandleGameRestartEvent()
    {
        GameStartButton.gameObject.SetActive(true);
        GameEndUIObject.SetActive(false);
    }
    
}
