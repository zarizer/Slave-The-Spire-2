using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCard : MonoBehaviour
{
    public CharacterBase character;
    public bool is_picking = false;

    [SerializeField] RawImage texture;
    [SerializeField] TextMeshProUGUI character_name;
    [SerializeField] GameObject SelectionStamp;
    [SerializeField] TextMeshProUGUI SelectionNum;
    

    public CharacterMenuController menu;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void UpdateSelection(int num)
    {
        if (num <= 0) SelectionStamp.SetActive(false);
        else
        {
            SelectionStamp.SetActive(true);
            SelectionNum.text = num.ToString();
        }
    }

    public void OnSelected()
    {
        if (!is_picking)
        {
            menu.CharacterMenu.GetComponent<CharacterPageScript>().character = character;
            ResoursesDict.GetClass<MainMenuManager>().SetMenu(menu.CharacterMenu);
        }
        else
        {
            menu.OnSelect(this);
        }
    }
    public void UpdateInfo()
    {
        texture.texture = IconManager.PlayerIcons[character.id].texture;
        character_name.text = character.name;
        if (character.id == 0)
        {
            texture.texture = ProfileManager.profile_picture;
            character_name.text = ProfileManager.profile.name;
        }
        
    }
}
