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
    public delegate void GameEndDel(ETEAM winner);
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
        Array.Sort(characters, (a, b) =>
        {
            return a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex());
        });
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
        bool isRedTeamAllDide = true;
        foreach (CharacterComponent character in redTeamCharacter)
        {
            isRedTeamAllDide = isRedTeamAllDide && !character.isActiveAndEnabled;
        }
        bool isBlueTeamAllDide = true;
        foreach (CharacterComponent character in blueTeamCharacter)
        {
            isBlueTeamAllDide = isBlueTeamAllDide && !character.isActiveAndEnabled;
        }
        
        if(isRedTeamAllDide && !isBlueTeamAllDide)
            SendGameEndEvent(ETEAM.Blue);
        else if (!isRedTeamAllDide && isBlueTeamAllDide)
            SendGameEndEvent(ETEAM.Red);
        else
            SendGameEndEvent(ETEAM.None);
    }
    

    IEnumerator SetGameTimer()
    {
        yield return new WaitForSeconds(1.0f);
        remainingTime -= 1.0f;
        while (remainingTime >= 0)
        {
            SendGameTickEvent();
            yield return new WaitForSeconds(0.7f);
            SendGameTickEndEvent();
            yield return new WaitForSeconds(0.3f);
            remainingTime -= 1.0f;


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
        
        
        yield return new WaitForSeconds(1.0f);
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