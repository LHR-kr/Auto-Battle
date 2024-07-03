using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HPTextComponent : MonoBehaviour
{
    private TextMeshProUGUI textUI;
    private CharacterComponent character;
    void Start()
    {
        character = GetComponentInParent<CharacterComponent>();
        textUI = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        string t = string.Format("{0}/{1}", character.HpComponent.HP, character.HpComponent.MaxHP);
        textUI.SetText(t);
    }
}
