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
        def = 7;
        speed = 6;
        level = 1;
        speed_dif = 2;
        dmg_k = 1f;
        moves = 4;
        energy = 15;
        name = "глава";
        description = "Глава банды отморозков.\n\nПретендует на звание мастера качалки.\n\nРуководит отморозками в бою.";
        skill_id1 = 2;
        skill_id2 = 3;
        skill_id3 = 4;
        skill_id4 = 5;
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

public class Shaman : CharacterBase
{

    public override CharacterBase Init()
    {
        id = 1;
        hp = 167;
        def = 0;
        speed = 10;
        level = 1;
        speed_dif = 2;
        dmg_k = 0.9f;
        moves = 3;
        energy = 15;
        name = "шаман";
        description = "обычный колтушский шаман\n\nспециализируется на тёмных(пивных) искусствах";
        skill_id1 = 6;
        skill_id2 = 7;
        skill_id3 = 8;
        skill_id4 = 9;
        none_k = 1.5f;
        fire_k = 0.8f;
        water_k = 1.4f;
        dendro_k = 0.7f;
        light_k = 1.1f;
        darkness_k = 0.6f;
        base.Init();
        return this;
    }


    public int GetMoves() { return moves; }

    public Shaman()
    {
        base.Init();
        passive_ids = new List<int>();
        passive_ids.Add(7);
        passive_ids.Add(9);
        passive_ids.Add(10);
    }

    public override void CreateStatsAccourdingToLevel()
    {
        base.CreateStatsAccourdingToLevel();
    }
}

public class Salty : CharacterBase
{

    public override CharacterBase Init()
    {
        id = 2;
        hp = 75;
        def = 0;
        speed = 2;
        level = 1;
        speed_dif = 2;
        dmg_k = 1.4f;
        moves = 2;
        energy = 0;
        name = "Солевой";
        description = "Обычный колтушский упырь, который согласился отправится в приключение, в Питер, так как в Колтушах кончились закладки";
        skill_id1 = 10;
        skill_id2 = 11;
        skill_id3 = 12;
        skill_id4 = 13;
        none_k = 1.5f;
        fire_k = 0.8f;
        water_k = 0.8f;
        dendro_k = 0.8f;
        light_k = 1.3f;
        darkness_k = 1;
        base.Init();
        return this;
    }


    public Salty()
    {
        base.Init();
        passive_ids = new List<int>();
        passive_ids.Add(11);
        passive_ids.Add(12);
        passive_ids.Add(13);
    }

    public override void CreateStatsAccourdingToLevel()
    {
        base.CreateStatsAccourdingToLevel();
    }
}

public class DisignCutie : CharacterBase
{

    public override CharacterBase Init()
    {
        id = 3;
        hp = 1;
        def = 0;
        speed = 5;
        level = 1;
        speed_dif = 2;
        dmg_k = 0.9f;
        moves = 5;
        energy = 25;
        name = "Милашка с дизайна";
        description = "Приехала с факультета дизайна университета ИТМО, чтобы пофотографировать уникальный архитектурный стиль Колтушей";
        skill_id1 = 14;
        skill_id2 = 15;
        skill_id3 = 16;
        skill_id4 = 17;
        none_k = 1.8f;
        fire_k = 1.2f;
        water_k = 0.6f;
        dendro_k = 0.9f;
        light_k = 0.5f;
        darkness_k = 0.5f;
        base.Init();
        return this;
    }


    public DisignCutie()
    {
        base.Init();
        passive_ids = new List<int>();
        passive_ids.Add(20);
        passive_ids.Add(21);
        passive_ids.Add(22);
    }

    public override void CreateStatsAccourdingToLevel()
    {
        base.CreateStatsAccourdingToLevel();
    }
}