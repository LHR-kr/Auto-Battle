using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour, IGameStartEventListener, IGameEndEventListener, IGameRestartEventListener
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
        GameManager.SendGameEndEvent += HandleGameEndEvent;
        GameManager.SendGameRestartEvent += HandleGameRestartEvent;
    }

    public void HandleGameStartEvent()
    {
        GameStartButton.gameObject.SetActive(false);
        GameEndUIObject.SetActive(false);
        RemainingTimeText.gameObject.SetActive(true);
    }


    public void HandleGameEndEvent()
    {;
        RemainingTimeText.gameObject.SetActive(false);   
        GameEndUIObject.SetActive(true);
    }

    public void HandleGameRestartEvent()
    {
        GameStartButton.gameObject.SetActive(true);
        GameEndUIObject.SetActive(false);
    }
    
}
