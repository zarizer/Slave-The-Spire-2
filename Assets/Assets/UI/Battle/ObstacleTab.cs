using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ObstacleTabController : TabController
{
    public CameraController CameraController;
    public TextMeshProUGUI description_text;
    public TextMeshProUGUI name_text;
    public TextMeshProUGUI lv_text;
    public TextMeshProUGUI hp_text;
    public TextMeshProUGUI def_text;
    public TextMeshProUGUI energy_text;
    public TextMeshProUGUI atk_text;

    public TextMeshProUGUI None_k_text;
    public TextMeshProUGUI Fire_k_text;
    public TextMeshProUGUI Water_k_text;
    public TextMeshProUGUI Dendro_k_text;
    public TextMeshProUGUI Light_k_text;
    public TextMeshProUGUI Darkness_k_text;

    public TextMeshProUGUI skill1_uses;
    public TextMeshProUGUI skill2_uses;
    public TextMeshProUGUI skill3_uses;
    public TextMeshProUGUI skill4_uses;

    public List<GameObject> skill_list;


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
        Debug.Log(is_visible);
        if (IsLocked) return;
        IsVisible = is_visible;

        if (!IsVisible)
        {
            gameObject.SetActive(false);
            return;
        }
        CharacterBase character = CameraController.Target.gameObject.GetComponent<GriddableObject>().GetCharacter();
        if (character == null) return;
        gameObject.SetActive(true);
        name_text.text = character.name;
        lv_text.text = "lv " + character.level;
        hp_text.text = character.cur_hp + " / " + character.start_hp;
        def_text.text = character.cur_def.ToString();
        
        energy_text.text = character.cur_energy.ToString();
        atk_text.text = character.cur_dmg_k.ToString();
        if (((ObstacleBase)character).is_showing_passives)
        {
            description_text.text = character.skills_description;
        }
        else 
        {
            description_text.text = ((ObstacleBase)character).description;
        }

        None_k_text.text = character.none_k.ToString();
        Fire_k_text.text = character.fire_k.ToString();
        Water_k_text.text = character.water_k.ToString();
        Dendro_k_text.text = character.dendro_k.ToString();
        Light_k_text.text = character.light_k.ToString();
        Darkness_k_text.text = character.darkness_k.ToString();

        MakeSkillText(1, skill1_uses, (ObstacleBase)character);
        MakeSkillText(2, skill2_uses, (ObstacleBase)character);
        MakeSkillText(3, skill3_uses, (ObstacleBase)character);
        MakeSkillText(4, skill4_uses, (ObstacleBase)character);

        if (((ObstacleBase)character).Destroyable == false) 
        {
            def_text.text = "-";
            hp_text.text = "-";
            None_k_text.text = "-";
            Fire_k_text.text = "-";
            Water_k_text.text = "-";
            Dendro_k_text.text = "-";
            Light_k_text.text = "-";
            Darkness_k_text.text = "-";
        }

        for (int i = 0; i < 4; i++)
        {
            skill_list[i].SetActive(false);
            if (i < ((ObstacleBase)character).InteractVariants) skill_list[i].SetActive(true);
        }
    }

    public void OnstacleActivate(int skill_num)
    {
        GridField field = CameraController.Target.GetComponent<GridObstacle>().field_;
        var obstacle = (ObstacleBase)CameraController.Target.GetComponent<GridObstacle>().GetCharacter();
        
        if (obstacle.cur_use_count <= 0) return; 
        if (skill_num == 1 && obstacle.InteractReq1(field))
        {
            obstacle.OnInteract1(field);
        }
        if (skill_num == 2 && obstacle.InteractReq2(field))
        {
            obstacle.OnInteract2(field);
        }
        if (skill_num == 3 && obstacle.InteractReq3(field))
        {
            obstacle.OnInteract3(field);
        }
        if (skill_num == 4 && obstacle.InteractReq4(field))
        {
            obstacle.OnInteract4(field);
        }
        RequestedUpdate(true);
    }

    void MakeSkillText(int num, TextMeshProUGUI text_use, ObstacleBase character)
    {
        text_use.text = "";

        text_use.text = (character.cur_use_count).ToString() + "/" + character.use_count.ToString();
    }

}

