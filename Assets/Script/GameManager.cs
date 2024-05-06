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
    
    private float remainingTime;
    private CharacterComponent[] characters;
    private List< CharacterComponent> redTeamCharacter;
    private List< CharacterComponent> blueTeamCharacter;
    private static GameManager instance = null;

    public delegate void GameStartDel();

    public delegate void GameTickDel();
    public delegate void GameEndDel();
    
    public static event GameStartDel SendGameStartEvent;
    public static event GameTickDel SendGameTickEvent;
    public static event GameEndDel SendGameEndEvent;

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

    }



    public void EndGame()
    {
        SendGameEndEvent();
    }
    

    IEnumerator SetGameTimer()
    {
        while (remainingTime > 0)
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
            
            
            yield return new WaitForSeconds(1.0f);
            remainingTime -= 1.0f;
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