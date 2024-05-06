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
        
        // Init Character
        foreach(CharacterComponent character in characters)
        {
            character.InitCharacter();
        }
    }

    public void PauseGame()
    {

    }

    public void ContinueGame()
    {

    }

    public void RestartGame()
    {

    }

    public void StopGame()
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

            foreach (CharacterComponent character in characters)
            {
                character.Act();
            }
            
        }
        EndGame();
        
    }
    
}