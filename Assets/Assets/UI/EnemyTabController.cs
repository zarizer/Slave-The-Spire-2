using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyTabController : TabController
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
    public bool IsVisible;

    public GameObject RollPrefab;
    public Transform MenuUIRolls;
    EnemyBase current_enemy;
    void Start()
    {

    }
    void Update()
    {

    }

    public override void RequestedUpdate(bool is_visible)
    {
        IsVisible = is_visible;

        if (!IsVisible)
        {
            gameObject.SetActive(false);
            return;
        }
        EnemyBase character = current_enemy;
        Debug.Log(CameraController.Target);
        if (character == null) return;
        gameObject.SetActive(true);
        name_text.text = character.name;
        hp_text.text = character.cur_hp + " / " + character.hp;
        def_text.text = character.cur_def.ToString();
        speed_text.text = character.cur_speed.ToString();
        moves_text.text = character.cur_moves.ToString();
        energy_text.text = character.cur_energy.ToString();
        atk_text.text = character.cur_base_dmg.ToString();
        description_text.text = character.description;

        StaticFuncs.DestroyChildren(MenuUIRolls);
        
        foreach (Roll roll in character.CurrentRolls)
        {
            
            GameObject menu_roll = Instantiate(RollPrefab, MenuUIRolls);
            menu_roll.GetComponent<RollScript>().UpdateRollStats(roll);
        }


    }

    private void OnDisable()
    {
        for (int i = 0; i < MenuUIRolls.childCount; i++)
        {
            Destroy(MenuUIRolls.GetChild(0).gameObject);
        }
    }

    public void SetCurrentEnemy(EnemyBase enemy) { current_enemy = enemy; }
}
