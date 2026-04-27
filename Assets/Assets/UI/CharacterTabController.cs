using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CharacterTabController : MonoBehaviour
{
    public CameraController CameraController;
    public TextMeshProUGUI name_text;
    public TextMeshProUGUI hp_text;
    public TextMeshProUGUI def_text;
    public TextMeshProUGUI speed_text;
    public TextMeshProUGUI moves_text;
    public TextMeshProUGUI energy_text;
    public TextMeshProUGUI atk_text;
    public bool IsVisible;
    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public void RequestedUpdate(bool is_visible)
    {
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
        atk_text.text = character.cur_base_dmg.ToString();
    }   
}
