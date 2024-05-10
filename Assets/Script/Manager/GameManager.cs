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



    // GameManager 오브젝트가 생성되지 않았는데도 다른 오브젝트의 OnEnable 함수에서 함수를 바인딩하여 NullException이 생기는 오류가 생긴다.
    // 이것을 해결하기 위하여 Static으로 선언한다.
    public static event Action SendGameStartEvent;
    public static event Action SendGameTickEvent;
    public static event Action SendGameTickEndEvent;
    public static event Action SendGameEndEvent;
    public static event Action SendGameRestartEvent;

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