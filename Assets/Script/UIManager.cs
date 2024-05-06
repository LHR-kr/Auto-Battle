using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour, IGameStartEventListener, IGameTickEventListener
{
    private static UIManager instance = null;
    [SerializeField] private Button GameStartButton;
    [SerializeField] private TextMeshProUGUI RemainingTimeText;
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
    }

    public void HandleGameStartEvent()
    {
        GameStartButton.gameObject.SetActive(false);
        RemainingTimeText.gameObject.SetActive(true);
        RemainingTimeText.SetText(GameManager.Instance.RemainingTime.ToString());
    }

    public void HandleGameTickEvent()
    {
        RemainingTimeText.SetText(GameManager.Instance.RemainingTime.ToString());
    }
}
