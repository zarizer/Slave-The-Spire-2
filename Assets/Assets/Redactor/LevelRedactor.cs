using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class LevelRedactor : MonoBehaviour
{
    [SerializeField] private Camera battle_camera;
    [SerializeField] private Camera ui_camera;
    [SerializeField] private GameObject battle_ui;
    [SerializeField] private GameObject ui_ui;
    [SerializeField] private GameObject redactor_ui;
    [SerializeField] private GameObject LevelIdInput;
    [SerializeField] private TMP_InputField LevelXInput;
    [SerializeField] private TMP_InputField LevelYInput;
    [SerializeField] private TMP_InputField LevelMinLevelInput;
    [SerializeField] private TMP_InputField LevelMaxLevelInput;
    public int current_id;

    public GridField current_field;



    void Start()
    {

    }

    void Update()
    {

    }

    public void OpenRedactor()
    {
        battle_ui.SetActive(true);
        ui_camera.gameObject.SetActive(false);
        battle_camera.gameObject.SetActive(true);
        redactor_ui.SetActive(true);
        LevelData.Init();
        IconManager.Init();
    }

    public void UpdateCurrentID(TMP_InputField input)
    {

        string level_id = input.text;
        if (int.TryParse(level_id, out int id))
        {
            current_id = id;
        }
        else
        {
            ResoursesDict.GetClass<SoundMain>().Restrict();
            current_id = 0;
        }

        ResoursesDict.GetClass<BattleMain>().SetBackGroundAccourdingToLevelId(current_id);
    
    }

    public void SaveLevel()
    {
        current_field.DebugLevelSaveId = current_id;
        string min_level = LevelMinLevelInput.text;
        if (int.TryParse(min_level, out int level))
        {
            if (level < 0) level = 0;
            current_field.DebugMinLevel = level;
            LevelMinLevelInput.text = level.ToString();
        }
        else
        {
            ResoursesDict.GetClass<SoundMain>().Restrict();
            current_field.DebugMinLevel = 0;
        }

        string max_level = LevelMaxLevelInput.text;
        if (int.TryParse(max_level, out int level_max))
        {
            current_field.DebugMaxLevel = level_max;
            if (level_max < level) level_max = level;
            LevelMaxLevelInput.text = level_max.ToString();
        }
        else
        {
            ResoursesDict.GetClass<SoundMain>().Restrict();
            current_field.DebugMaxLevel = 100;
        }
        current_field.SaveLevelData();
    }

    public void GetLevel()
    {
        Skills.Init();
        if (current_field != null) Destroy(current_field.gameObject);
        current_field = Instantiate(ResoursesDict.ObjectSet["Field"]).GetComponent<GridField>();
        current_field.is_redactor = true;
        ResoursesDict.GetClass<BattleMain>().current_field = current_field;
        ResoursesDict.GetClass<CameraController>().Target = current_field.transform;
        current_field.GetLevelData(current_id);
        LevelMaxLevelInput.text = current_field.DebugMaxLevel.ToString();
        LevelMinLevelInput.text = current_field.DebugMinLevel.ToString();
    }

    public void NewLevel()
    {
        Skills.Init();
        if (current_field != null) Destroy(current_field.gameObject);
        current_field = Instantiate(ResoursesDict.ObjectSet["Field"]).GetComponent<GridField>();
        ResoursesDict.GetClass<BattleMain>().current_field = current_field;
        ResoursesDict.GetClass<CameraController>().Target = current_field.transform;

        string SizeX = LevelXInput.text;
        if (int.TryParse(SizeX, out int size_x))
        {
            if (size_x < 1) size_x = 1;
            current_field.SizeX_ = size_x;
            LevelXInput.text = size_x.ToString();
        }
        else
        {
            ResoursesDict.GetClass<SoundMain>().Restrict();
            current_field.SizeX_ = 10;
            LevelXInput.text = "10";
        }

        string SizeY = LevelYInput.text;
        if (int.TryParse(SizeY, out int size_y))
        {
            if (size_y < 1) size_y = 1;
            current_field.SizeY_ = size_y;
            LevelYInput.text = size_y.ToString();
        }
        else
        {
            ResoursesDict.GetClass<SoundMain>().Restrict();
            current_field.SizeY_ = 10;
            LevelYInput.text = "10";
        }
        current_field.is_redactor = true;
        current_field.CreateField();
    }
}
