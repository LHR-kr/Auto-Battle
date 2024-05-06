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
    private List< CharacterComponent> redTeamCharacter;
    private List< CharacterComponent> blueTeamCharacter;
    private static GameManager instance = null;

    public delegate void GameStartDel();

    public delegate void GameTickDel();
    public static event GameStartDel SendGameStartEvent;
    public static event GameTickDel SendGameTickEvent;

    private Coroutine GameTimer;
    
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
        redTeamCharacter = new();
        blueTeamCharacter = new();
        
        foreach (CharacterComponent character in characters)
        {
            if(character.Team == ETEAM.Blue)
                blueTeamCharacter.Add(character);
            else if (character.Team == ETEAM.Red)
                redTeamCharacter.Add(character);
        }
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
        GameTimer =  StartCoroutine(SetGameTimer());

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
        Debug.Log("end game");
    }
    

    IEnumerator SetGameTimer()
    {
        while (RemainingTime > 0)
        {
            
            //생존자 확인
            bool isRedTeamAllDide = true;
            foreach (CharacterComponent character in redTeamCharacter)
            {
                isRedTeamAllDide = isRedTeamAllDide && character.IsDead;
            }
            bool isBlueTeamAllDide = true;
            foreach (CharacterComponent character in blueTeamCharacter)
            {
                isBlueTeamAllDide = isBlueTeamAllDide && character.IsDead;
            }

            if (isRedTeamAllDide || isBlueTeamAllDide)
            {
                StopCoroutine(GameTimer);
                EndGame();
            }
            
            RemainingTimeText.SetText(RemainingTime.ToString());
            
            yield return new WaitForSeconds(1.0f);
            RemainingTime -= 1.0f;
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