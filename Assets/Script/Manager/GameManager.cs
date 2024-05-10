using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public enum ETEAM
{
    Red,Blue,None
}


public class GameManager : MonoBehaviour
{
    [SerializeField] private float MaxTime;
    
    private float remainingTime;
    private CharacterComponent[] characters;
    private List< CharacterComponent> redTeamCharacter;
    private List< CharacterComponent> blueTeamCharacter;
    private static GameManager instance = null;

    public delegate void GameStartDel();
    public delegate void GameTickDel();
    public delegate void GameTickEndDel();
    public delegate void GameEndDel();
    public delegate void GameRestartDel();

    
    public static event GameStartDel SendGameStartEvent;
    public static event GameTickDel SendGameTickEvent;
    public static event GameTickEndDel SendGameTickEndEvent;
    public static event GameEndDel SendGameEndEvent;
    public static event GameRestartDel SendGameRestartEvent;

    private Coroutine GameTimer;

    public float RemainingTime
    {
        get
        {
            return remainingTime;
        }
    }

    public CharacterComponent[] Characters
    {
        get
        {
            return characters;
        }
    }

    void Awake()
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


    public void StartGame()
    {
        remainingTime = MaxTime;
        SendGameStartEvent();
        GameTimer =  StartCoroutine(SetGameTimer());
    }



    public void RestartGame()
    {
        foreach (CharacterComponent charater in characters)
        {
            charater.gameObject.SetActive(true);
        }
        SendGameRestartEvent();
    }



    public void EndGame()
    {
        SendGameEndEvent();
    }
    

    IEnumerator SetGameTimer()
    {
        float tickDelayTime = 1.0f;
        float tickActTime = 0.7f;
        float tickAfterActTime = tickDelayTime - tickActTime;

        WaitForSeconds TickDelay = new(tickDelayTime);
        WaitForSeconds TickAtcDelay = new(tickActTime);
        WaitForSeconds TickAfterActDelay = new(tickAfterActTime);

        yield return TickDelay;
        remainingTime -= tickDelayTime;
        while (remainingTime >= 0)
        {
            SendGameTickEvent();
            yield return TickAtcDelay;
            SendGameTickEndEvent();
            yield return TickAfterActDelay;
            remainingTime -= tickDelayTime;


            bool isRedTeamAllDied = true;
            foreach (CharacterComponent character in redTeamCharacter)
            {
                isRedTeamAllDied = isRedTeamAllDied && !character.isActiveAndEnabled;
            }
            bool isBlueTeamAllDied = true;
            foreach (CharacterComponent character in blueTeamCharacter)
            {
                isBlueTeamAllDied = isBlueTeamAllDied && !character.isActiveAndEnabled;
            }

            if (isRedTeamAllDied || isBlueTeamAllDied)
            {
                StopCoroutine(GameTimer);
                EndGame();
            }
            
            
        }


        yield return TickDelay;
        EndGame();
        
    }
    
    public ETEAM GetWinnerTeam()
    {
        bool isRedTeamAllDied = true;
        foreach (CharacterComponent character in redTeamCharacter)
        {
           isRedTeamAllDied = isRedTeamAllDied && !character.isActiveAndEnabled;
        }
        bool isBlueTeamAllDied = true;
        foreach (CharacterComponent character in blueTeamCharacter)
        {
           isBlueTeamAllDied = isBlueTeamAllDied && !character.isActiveAndEnabled;
        }

        if (isRedTeamAllDied && !isBlueTeamAllDied)
           return ETEAM.Blue;
        else if (!isRedTeamAllDied && isBlueTeamAllDied)
           return ETEAM.Red;
        else
           return ETEAM.None;
    }
}