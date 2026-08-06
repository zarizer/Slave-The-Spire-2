using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RedactorObjectTab : TabController
{
    bool IsVisible = false;
    public TMP_InputField Level;
    public TMP_InputField SpecialValue;
    public Toggle is_static_level;
    public GriddableObject griddableObject;

    

    public override void RequestedUpdate(bool is_visible)
    {
        IsVisible = is_visible;

        if (IsVisible) gameObject.SetActive(true);
        else { gameObject.SetActive(false); return; }

        GriddableObject obj = ResoursesDict.GetClass<CameraController>().Target.GetComponent<GriddableObject>();
        griddableObject = obj;

        is_static_level.isOn = obj.IsCustomLevel;
        SpecialValue.text = obj.specialValue.ToString();
        Level.text = obj.GetCharacter().level.ToString();
    }
    
    public void CheckSpecialValue()
    {
        if (int.TryParse(SpecialValue.text, out int value))
        {
            griddableObject.specialValue = value;
        }
        else
        {
            ResoursesDict.GetClass<SoundMain>().Restrict();
            griddableObject.specialValue = 0;
        }
    } 
    public void CheckStaticLevel(Toggle toggle)
    {
        if (toggle.isOn)
        {
            griddableObject.IsCustomLevel = true;
            if (int.TryParse(Level.text, out int level))
            {
                griddableObject.CustomLevel = level;
                griddableObject.GetCharacter().level = level;
                griddableObject.GetCharacter().Init();
            }
            else
            {
                ResoursesDict.GetClass<SoundMain>().Restrict();
            }
        }
        else
        {
            griddableObject.IsCustomLevel = false;
            griddableObject.CustomLevel = 0;
            griddableObject.GetCharacter().Init();
            if (!int.TryParse(Level.text, out int level))
            {
                ResoursesDict.GetClass<SoundMain>().Restrict();
            }
        }
    } 

    public void DeleteObject()
    {
        ResoursesDict.GetClass<CameraController>().field_.RemoveObject(griddableObject, true);
    }

    public void MoveObject()
    {
        ResoursesDict.GetClass<CameraController>().is_redactor_moving = true;
    }


}

