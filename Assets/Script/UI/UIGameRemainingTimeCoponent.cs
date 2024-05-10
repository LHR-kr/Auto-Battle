using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGameRemainingTimeCoponent : MonoBehaviour, IGameTickEventListener
{
    private TextMeshProUGUI RemainingTimeText;

    private void OnEnable()
    {
        RemainingTimeText = GetComponent<TextMeshProUGUI>();
        RemainingTimeText.SetText(GameManager.Instance.RemainingTime.ToString());
    }



    private void Start()
    {
        GameManager.SendGameTickEvent += HandleGameTickEvent;
        RemainingTimeText.SetText(GameManager.Instance.RemainingTime.ToString());
    }

    public void HandleGameTickEvent()
    {
        RemainingTimeText.SetText(GameManager.Instance.RemainingTime.ToString());
    }
}
