using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float MaxTime;

    [SerializeField] private Button GameStartButton;
    [SerializeField] private TextMeshProUGUI RemainingTimeText;
    private float RemainingTime;
    
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

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    public void StartGame()
    {
        RemainingTime = MaxTime;
        StartCoroutine(SetGameTimer());

        RemainingTimeText.gameObject.SetActive(true);
        GameStartButton.gameObject.SetActive(false);
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
        }
        EndGame();
        
    }
    
}