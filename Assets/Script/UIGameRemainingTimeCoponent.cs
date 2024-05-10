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
        GameManager.SendGameTickEvent += HandleGameTickEvent;
    }

    private void OnDisable()
    {
        GameManager.SendGameTickEvent -= HandleGameTickEvent;
    }

    public void HandleGameTickEvent()
    {
        RemainingTimeText.SetText(GameManager.Instance.RemainingTime.ToString());
    }
}
