using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CharacterTabController : TabController
{
    public CameraController CameraController;
    public TextMeshProUGUI description_text;
    public TextMeshProUGUI name_text;
    public TextMeshProUGUI hp_text;
    public TextMeshProUGUI def_text;
    public TextMeshProUGUI speed_text;
    public TextMeshProUGUI moves_text;
    public TextMeshProUGUI energy_text;
    public TextMeshProUGUI atk_text;

    public TextMeshProUGUI skill1_energy;
    public TextMeshProUGUI skill2_energy;
    public TextMeshProUGUI skill3_energy;
    public TextMeshProUGUI skill4_energy;
    public TextMeshProUGUI skill1_uses;
    public TextMeshProUGUI skill2_uses;
    public TextMeshProUGUI skill3_uses;
    public TextMeshProUGUI skill4_uses;
    public TextMeshProUGUI move_button_text;

    public TextMeshProUGUI None_k_text;
    public TextMeshProUGUI Fire_k_text;
    public TextMeshProUGUI Water_k_text;
    public TextMeshProUGUI Dendro_k_text;
    public TextMeshProUGUI Light_k_text;
    public TextMeshProUGUI Darkness_k_text;
    

    public bool IsVisible;
    public bool IsLocked = false;
    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public override void RequestedUpdate(bool is_visible)
    {
        Debug.Log("update");
        if (IsLocked) return;
        IsVisible = is_visible;
        
        if (!IsVisible)
        {
            gameObject.SetActive(false);
            return;
        }
        CharacterBase character = CameraController.Target.gameObject.GetComponent<GridCharacter>().character_;
        if (character == null) return;
        gameObject.SetActive(true);
        name_text.text = character.name;
        hp_text.text = character.cur_hp + " / " + character.hp;
        def_text.text = character.cur_def.ToString();
        speed_text.text = character.cur_speed.ToString();
        moves_text.text = character.cur_moves.ToString();
        energy_text.text = character.cur_energy.ToString();
        atk_text.text = character.cur_dmg_k.ToString();
        description_text.text = character.description;

        MakeSkillText(1, skill1_energy, skill1_uses, character);
        MakeSkillText(2, skill2_energy, skill2_uses, character);
        MakeSkillText(3, skill3_energy, skill3_uses, character);
        MakeSkillText(4, skill4_energy, skill4_uses, character);
        move_button_text.text = character.cur_moves.ToString();

        None_k_text.text = character.none_k.ToString();
        Fire_k_text.text = character.fire_k.ToString();
        Water_k_text.text = character.water_k.ToString();
        Dendro_k_text.text = character.dendro_k.ToString();
        Light_k_text.text = character.light_k.ToString();
        Darkness_k_text.text = character.darkness_k.ToString();

    }   


    void MakeSkillText(int num, TextMeshProUGUI text, TextMeshProUGUI text_use, CharacterBase character) 
    {
        text.text = "";
        text_use.text = "";

        PlayerSkill skill = null;

        if (num == 1) skill = character.Skill1;
        if (num == 2) skill = character.Skill2;
        if (num == 3) skill = character.Skill3;
        if (num == 4) skill = character.Skill4;

        if (skill.energy <= 0)
        {
            text.text += "+";
            text.color = new Color(0.5f, 1f, 0.5f);
        }
        else
        {
            text.text += "-";
            text.color = new Color(1f, 0.5f, 0.5f);
        }
        text.text += Math.Abs(skill.energy).ToString();

        text_use.text = skill.cur_use_count.ToString() + "/" + skill.max_use_count.ToString();
    }
}


public class TabController : MonoBehaviour
{
    public virtual void RequestedUpdate(bool is_visible)
    {

    }
}