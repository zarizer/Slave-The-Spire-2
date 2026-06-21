using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class FloatingRollMenu : FloatingInfoMenu
{
    Roll roll;
    public RawImage ElementImage;
    public RawImage RollTypeImage;
    public RawImage RollDistImage;
    public RawImage RollRadiusImage;
    public TextMeshProUGUI RollDescription;
    public int TextSize;

    public void Start()
    {
    }
    public void Update()
    {

    }
    public override void UpdateInfo()
    {
        roll = ParantObj.GetComponent<RollScript>().roll;
        var skill = roll.skill;
        Debug.Log(skill);

        if (roll.element == Element.None) ElementImage.texture = ResoursesDict.TextureSet["NoneElement"];
        else if (roll.element == Element.fire) ElementImage.texture = ResoursesDict.TextureSet["FireElement"];
        else if (roll.element == Element.water) ElementImage.texture = ResoursesDict.TextureSet["WaterElement"];
        else if (roll.element == Element.dendro) ElementImage.texture = ResoursesDict.TextureSet["DendroElement"];
        else if (roll.element == Element.light) ElementImage.texture = ResoursesDict.TextureSet["LightElement"];
        else if (roll.element == Element.darkness) ElementImage.texture = ResoursesDict.TextureSet["DarknessElement"];

        if (roll.rollType == RollType.Atk) RollTypeImage.texture = ResoursesDict.TextureSet["atk_icon"];
        else if (roll.rollType == RollType.Def) RollTypeImage.texture = ResoursesDict.TextureSet["def_icon"];
        else if (roll.rollType == RollType.Evade) RollTypeImage.texture = ResoursesDict.TextureSet["evade_icon"];

        if (skill.rollDist == RollDist.Any) RollDistImage.texture = ResoursesDict.TextureSet["any"];
        else if (skill.rollDist == RollDist.Radius) RollDistImage.texture = ResoursesDict.TextureSet["radius"];
        else if (skill.rollDist == RollDist.StLine) RollDistImage.texture = ResoursesDict.TextureSet["st_line"];
        else if (skill.rollDist == RollDist.DgLine) RollDistImage.texture = ResoursesDict.TextureSet["dg_line"];
        else if (skill.rollDist == RollDist.Other) RollDistImage.texture = ResoursesDict.TextureSet["custom"];

        if (roll.rollRadius == RollRadius.Single) RollRadiusImage.texture = ResoursesDict.TextureSet["single"];
        else if (roll.rollRadius == RollRadius.TargetRadius) RollRadiusImage.texture = ResoursesDict.TextureSet["target_radius"];
        else if (roll.rollRadius == RollRadius.PlayerRadius) RollRadiusImage.texture = ResoursesDict.TextureSet["player_radius"];
        else if (roll.rollRadius == RollRadius.Field) RollRadiusImage.texture = ResoursesDict.TextureSet["field"];
        else if (roll.rollRadius == RollRadius.StLine) RollRadiusImage.texture = ResoursesDict.TextureSet["st_line"];
        else if (roll.rollRadius == RollRadius.DgLine) RollRadiusImage.texture = ResoursesDict.TextureSet["dg_line"];
        else if (roll.rollRadius == RollRadius.Other) RollRadiusImage.texture = ResoursesDict.TextureSet["custom"];
    }
} 