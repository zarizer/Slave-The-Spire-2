using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrepareBattleScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI DifficultyName;
    [SerializeField] private Slider difficulty_slider;
    [SerializeField] private TextMeshProUGUI LevelIdText;
    public int LevelId;
    public int difficulty;
    public int max_difficulty;
    void Start()
    {

    }


    void Update()
    {

    }


    private void OnEnable()
    {
        ProfileManager.LoadProfile();
        max_difficulty = ProfileManager.profile.max_difficulty;
        difficulty_slider.maxValue = max_difficulty;
        OnDifficultyChange();
    }

    public void OnDifficultyChange() {
        Dictionary<int, string> dif_names = new Dictionary<int, string>
        {
            { 1, "нормис" },
            { 2, "наш слоняра" },
            { 3, "хардкорщик" },
            { 4, "легенда" },
            { 5, "мастер качалки" },
        };
        difficulty = (int)difficulty_slider.value;
        DifficultyName.text = dif_names[difficulty];
    }

    public void OnLevelIdChange(TMP_InputField field)
    {
        string level_id = field.text;
        if (int.TryParse(level_id, out int id))
        {
            if (LevelData.GetLevelData(id) != null) { LevelId = id; }
            else LevelId = 0;
        }
        else 
        {
            Debug.Log("Не парситься");
            LevelId = 0; 
        }
    }

}
