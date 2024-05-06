using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class HPBarComponent : MonoBehaviour
{
    private CharacterComponent character;
    void Start()
    {
        character = GetComponentInParent<CharacterComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (character)
        {
            Debug.Log(character.HP);
            this.GetComponent<Image>().fillAmount = (character.HP / character.MaxHP);
        }
    }
}
