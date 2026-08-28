using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPageScript : MonoBehaviour
{
    public CharacterBase character;
    public RawImage character_icon;
    public TextMeshProUGUI name_text;
    public TextMeshProUGUI lv_text;
    public TextMeshProUGUI hp_text;
    public TextMeshProUGUI def_text;
    public TextMeshProUGUI moves_text;
    public TextMeshProUGUI energy_text;
    public TextMeshProUGUI atk_text;
    public TextMeshProUGUI None_k_text;
    public TextMeshProUGUI Fire_k_text;
    public TextMeshProUGUI Water_k_text;
    public TextMeshProUGUI Dendro_k_text;
    public TextMeshProUGUI Light_k_text;
    public TextMeshProUGUI Darkness_k_text;
    public TextMeshProUGUI XpNeedText;
    public TextMeshProUGUI XpHasText;
    void Start()
    {
        
    }


    void Update()
    {
        
    }

    private void OnEnable()
    {

        UpdateData();
    }

    public void OnUpgrade()
    {
        if (character.level >= 100) { ResoursesDict.GetClass<SoundMain>().Restrict();  Debug.Log(222); return; }
        if (character.GetXpTicketAmountToUpgrade() > InventoryManager.GetItemCount(Item.xp_ticket)) { ResoursesDict.GetClass<SoundMain>().Restrict(); Debug.Log(333); return; }
        ResoursesDict.GetClass<SoundMain>().Accept();
        InventoryManager.RemoveItem(Item.xp_ticket, character.GetXpTicketAmountToUpgrade());
        character.level++;
        ProfileManager.profile.CharacterLevels[ProfileManager.GetJSONIdByCharacterId(character.id)]++;
        ProfileManager.SaveProfile();
        character.CreateStatsAccourdingToLevel();
        UpdateData();
    }

    void UpdateXtText()
    {
        XpHasText.text = InventoryManager.GetItemCount(Item.xp_ticket).ToString();
        XpNeedText.text = character.GetXpTicketAmountToUpgrade().ToString();
    }

    public void UpdateData()
    {
        character.CreateStatsAccourdingToLevel();
        if (character == null) return;
        gameObject.SetActive(true);
        if (character.id == 0) { name_text.text = ProfileManager.profile.name; }
        else { name_text.text = character.name; }
        lv_text.text = "lv " + character.level;
        hp_text.text = character.cur_hp + " / " + character.start_hp;
        def_text.text = character.cur_def.ToString();
        moves_text.text = character.cur_moves.ToString();
        energy_text.text = character.cur_energy.ToString();
        atk_text.text = character.cur_dmg_k.ToString();

        if (character.id == 0) { character_icon.texture = ProfileManager.profile_picture; }
        else { character_icon.texture = IconManager.PlayerIcons[character.id].texture; }

        //MakeSkillText(1, skill1_energy, skill1_uses, character);
        //MakeSkillText(2, skill2_energy, skill2_uses, character);
        //MakeSkillText(3, skill3_energy, skill3_uses, character);
        //MakeSkillText(4, skill4_energy, skill4_uses, character);
        //move_button_text.text = character.cur_moves.ToString();

        None_k_text.text = character.none_k.ToString();
        Fire_k_text.text = character.fire_k.ToString();
        Water_k_text.text = character.water_k.ToString();
        Dendro_k_text.text = character.dendro_k.ToString();
        Light_k_text.text = character.light_k.ToString();
        Darkness_k_text.text = character.darkness_k.ToString();
        UpdateXtText();
    }
}
