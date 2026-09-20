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
    [SerializeField] private CharacterMenuController character_menu;
    [SerializeField] private GameObject CharactersList;
    public int LevelId;
    public int difficulty;
    public int max_difficulty;
    [SerializeField] private GameObject SaveData;
    [SerializeField] private TextMeshProUGUI SaveValue;
    [SerializeField] private TextMeshProUGUI CompaignValue;
    [SerializeField] private TextMeshProUGUI ChapterValue;
    [SerializeField] private TextMeshProUGUI LevelNumValue;
    [SerializeField] private TextMeshProUGUI LevelIdValue;
    [SerializeField] private TextMeshProUGUI DifficultyValue;

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
        character_menu.is_picking = true;
        CharactersList.transform.SetParent(this.transform);
        character_menu.LoadData();
        CharacterMenuController.selected_characters.Clear();

        OnDifficultyChange();

        if (ProfileManager.profile.is_in_game)
        {
            SaveValue.text = "ЕСТЬ";
            SaveData.SetActive(true);
            CompaignValue.text = ProfileManager.profile.compaign_num.ToString();
            ChapterValue.text = ProfileManager.profile.chapter_num.ToString();
            LevelNumValue.text = ProfileManager.profile.level_num.ToString();
            LevelIdValue.text = ProfileManager.profile.level_id.ToString();
            DifficultyValue.text = dif_names[ProfileManager.profile.difficulty];
        }
        else
        {
            SaveValue.text = "НЕТ";
            SaveData.SetActive(false);
        }
    }

    private void OnDisable()
    {
        LeanTween.delayedCall(0.1f, () =>
        {
            CharactersList.transform.SetParent(character_menu.transform);
        });
        character_menu.is_picking = false;

    }

    public void OnDifficultyChange() {
        
        difficulty = (int)difficulty_slider.value;
        ResoursesDict.GetClass<BattleMain>().difficulty = difficulty;
        DifficultyName.text = dif_names[difficulty];
    }

    Dictionary<int, string> dif_names = new Dictionary<int, string>
        {
            { 1, "Нормис" },
            { 2, "Слон" },
            { 3, "Легенда" },
            { 4, "мастер качалки" },
            { 5, "шизоид" },
        };
    public void OnLevelIdChange(TMP_InputField field)
    {
        string level_id = field.text;
        if (int.TryParse(level_id, out int id))
        {
            if (LevelData.GetLevelData(id) != null) { LevelId = id; ProfileManager.profile.is_custom_level = true; }
            else LevelId = 0;
        }
        else 
        {
            LevelId = 0; 
        }
    }

}
