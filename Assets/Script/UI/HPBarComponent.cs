using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class HPBarComponent : MonoBehaviour
{
    private CharacterComponent character;
    private Image image;
    void Start()
    {
        character = GetComponentInParent<CharacterComponent>();
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        image.fillAmount = (character.hpComponent.HP / character.hpComponent.MaxHP);
    }
}
