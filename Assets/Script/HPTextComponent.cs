using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HPTextComponent : MonoBehaviour
{
    private TextMeshProUGUI text;
    private CharacterComponent character;
    void Start()
    {
        character = GetComponentInParent<CharacterComponent>();
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        string t = string.Format("{0}/{1}", character.HP, character.MaxHP);
        text.SetText(t);
    }
}
