using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCharacter : CharacterBase
{

    public override CharacterBase Init()
    {
        id = 0;
        hp = 100;
        def = 10;
        speed = 10;
        level = 1;
        speed_dif = 2;
        dmg_k = 1f;
        moves = 4;
        energy = 20;
        name = "глава";
        description = "Глава банды отморозков.\n\nПретендует на звание мастера качалки.\n\nРуководит отморозками в бою.";
        skill_id1 = 1;
        skill_id2 = 1;
        skill_id3 = 1;
        skill_id4 = 0;
        none_k = 1;
        fire_k = 1;
        water_k = 1;
        dendro_k = 1;
        light_k = 1;
        darkness_k = 1;
        base.Init();
        return this;
    }

    public override void OnSpawn(GridField field_data)
    {   
        base.OnSpawn(field_data);
        var character = (GridCharacter)field_data.current_object;
        ProfileManager.LoadImage(ProfileManager.profile.profile_picture_path);
        character.TexturePlane.GetComponent<RawImage>().texture = ProfileManager.profile_picture;
        character.name = ProfileManager.profile.name;
        character.GetCharacter().name = ProfileManager.profile.name;
        moves = 5;
    }
    public int GetMoves() { return moves; }

    public MainCharacter()
    {
        base.Init();
        passive_ids = new List<int>();
        passive_ids.Add(0);
        passive_ids.Add(1);
        passive_ids.Add(2);
    }

    public override void CreateStatsAccourdingToLevel()
    {
        base.CreateStatsAccourdingToLevel();
    }
}
