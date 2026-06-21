using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RollScript : MonoBehaviour
{
    public Roll roll;
    public GameObject rollMenuUI;
    public CharacterBase character;

    public TextMeshProUGUI MinMaxText;
    public RawImage RollImage;
    void Start()
    {
        rollMenuUI = gameObject;
    }
    void Update()
    {
        
    }

    public void UpdateRollStats(Roll new_roll)
    {
        roll = new_roll;
        MinMaxText.text = roll.GetMinRoll().ToString() + "-" + roll.GetMaxRoll().ToString();
        RollImage.color = GetImageColor();
        
    }

    Color GetImageColor()
    {
        Color color;
        if (roll.rollType == RollType.Def)
        {
            color = new Color(90f / 255f, 90f / 255f, 255f / 255f);
        }
        else if (roll.rollType == RollType.Evade)
        {
            color = new Color(150f / 255f, 150f / 255f, 255f / 255f);
        }
        else if (roll.rollType == RollType.Effect)
        {
            color = new Color(150f / 255f, 190f / 255f, 150f / 255f);
        }
        else if (roll.rollType == RollType.Other)
        {
            color = new Color(20f / 255f, 20f / 255f, 20f / 255f);
        }
        else
        {
            if (roll.element == Element.None)
            {
                color = new Color(230f / 255f, 230f / 255f, 230f / 255f);
            }
            else if (roll.element == Element.fire)
            {
                color = new Color(255f / 255f, 130f / 255f, 70f / 255f);
            }
            else if (roll.element == Element.water)
            {
                color = new Color(40f / 255f, 40f / 255f, 255f / 255f);
            }
            else if (roll.element == Element.dendro)
            {
                color = new Color(200f / 255f, 255f / 255f, 200f / 255f);
            }
            else if (roll.element == Element.light)
            {
                color = new Color(255f / 255f, 255f / 255f, 200f / 255f);
            }
            else if (roll.element == Element.darkness)
            {
                color = new Color(0f / 255f, 0f / 255f, 40f / 255f);
            }
            else
            {
                color = new Color(20f / 255f, 20f / 255f, 20f / 255f);
            }
        }

        return color;
    }
}


public enum RollType
{
    Def,
    Atk,
    Evade,
    Effect,
    Other
}

public enum Element
{
    fire,
    water,
    dendro,
    darkness,
    light,
    None
}

public enum RollDist
{
    StLine,
    DgLine,
    Radius,
    Any,
    Other
}

public enum RollRadius
{
    Single,
    StLine,
    DgLine,
    PlayerRadius,
    TargetRadius,
    Field,
    Other
}