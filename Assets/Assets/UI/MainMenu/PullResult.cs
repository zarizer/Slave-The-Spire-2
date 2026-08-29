using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PullResult : MonoBehaviour
{
    public RawImage image;
    public TextMeshProUGUI count;
    public Loot loot;
    public CharacterBase character = null;
    public ParticleSystem particles;
    public PullsMenuController pull_manager;

    public void UpdateData()
    {
        if (loot.item == Item.character)
        {
            character = loot.character;
            image.texture = IconManager.PlayerIcons[character.id].texture;
            if (character.id == 0)
            {
                image.texture = ProfileManager.profile_picture;
            }
            count.text = "";
            particles.Play();
        }
        else
        {
            image.texture = InventoryManager.GetItemData(loot.item).icon;
            count.text = loot.count.ToString();
        }

    }
    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
