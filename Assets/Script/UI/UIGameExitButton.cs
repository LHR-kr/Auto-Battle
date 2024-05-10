using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIGameExitButton : MonoBehaviour
{
    [SerializeField] private GameObject GameExitUIObject;

    public void OnExitButtonClick()
    {
        GameExitUIObject.SetActive(true);
    }

    public void OnClickYes()
    {
        Application.Quit();
    }
    public void OnClickNo()
    {
        GameExitUIObject.SetActive(false);
    }
}
