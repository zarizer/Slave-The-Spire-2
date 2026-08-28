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
        Dictionary<int, string> dif_names = new Dictionary<int, string>
        {
            { 1, "Нормис" },
            { 2, "Слон" },
            { 3, "Легенда" },
            { 4, "мастер качалки" },
            { 5, "шизоид" },
        };
        difficulty = (int)difficulty_slider.value;
        ResoursesDict.GetClass<BattleMain>().difficulty = difficulty;
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
            LevelId = 0; 
        }
    }

}
