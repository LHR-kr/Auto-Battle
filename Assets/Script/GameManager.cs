using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float MaxTime;
    [SerializeField] private Button GameStartButton;
    [SerializeField] private TextMeshProUGUI RemainingTimeText;
    private float RemainingTime;
    private CharacterComponent[] characters;
    private static GameManager instance = null;

    public delegate void GameStartDel();

    public delegate void GameTickDel();
    public static event GameStartDel SendGameStartEvent;
    public static event GameTickDel SendGameTickEvent;
    
    void Awake()
    {
        if (null == instance)
        {
            
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }

    private void Start()
    {
        characters = FindObjectsOfType<CharacterComponent>();
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                return null;
            else 
                return instance;
        }
    }

    public void StartGame()
    {
        // Init UI
        RemainingTime = MaxTime;
        StartCoroutine(SetGameTimer());

        RemainingTimeText.gameObject.SetActive(true);
        GameStartButton.gameObject.SetActive(false);
        
        // Send Game Start Event
        SendGameStartEvent();
    }



    public void RestartGame()
    {

    }



    public void EndGame()
    {
        
    }
    

    IEnumerator SetGameTimer()
    {
        while (RemainingTime > 0)
        {
            RemainingTime -= 1.0f;
            RemainingTimeText.SetText(RemainingTime.ToString());
            yield return new WaitForSeconds(1.0f);

            SendGameTickEvent();

        }
        EndGame();
        
    }

    public CharacterComponent[] Characters
    {
        get
        {
            return characters;
        }
    }
    
    
}