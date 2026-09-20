using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : EnemyBase
{
    public override CharacterBase Init()
    {
        id = 0;
        hp = 100;
        def = 10;
        speed = 10;
        speed_dif = 2;
        dmg_k = 1f;
        moves = 3;
        energy = 20;
        name = "Линус Торвальдс";
        description = "YOU SHOULD DELETE WINDOWS. NOW!!!";
        SkillsPerTurn = new List<List<PlayerSkill>> {
            new List<PlayerSkill>() { DataDicts.EnemySkillSet[3], DataDicts.EnemySkillSet[4] },
            new List<PlayerSkill>() { DataDicts.EnemySkillSet[2], DataDicts.EnemySkillSet[0] }
        };
        init_light_k = 2.5f;
        init_darkness_k = 0.1f;
        init_fire_k = 1.5f;
        base.Init();
        return this;
    }

    public override void CreateStatsAccourdingToLevel()
    {
        base.CreateStatsAccourdingToLevel();
    }

}

public class EnemyVosh : EnemyBase
{
    public override CharacterBase Init()
    {
        id = 1;
        hp = 25;
        def = 0;
        speed = 2;
        speed_dif = 2;
        dmg_k = 1f;
        moves = 3;
        energy = 0;
        name = "Вош";
        description = "Типичная вошь, которая водится в колтушских лесах";
        SkillsPerTurn = new List<List<PlayerSkill>> {
            new List<PlayerSkill>() { DataDicts.EnemySkillSet[5], DataDicts.EnemySkillSet[7] },
            new List<PlayerSkill>() { DataDicts.EnemySkillSet[6], DataDicts.EnemySkillSet[7] }
        };
        init_none_k = 1.2f;
        init_water_k = 0.75f;
        init_fire_k = 1.2f;
        base.Init();
        return this;
    }

    public EnemyVosh()
    {
        passive_ids = new List<int>();
        passive_ids.Add(3);
        passive_ids.Add(4);
    }

    public override void CreateStatsAccourdingToLevel()
    {
        base.CreateStatsAccourdingToLevel();
    }
}

public class EnemyWitch : EnemyBase
{
    public override CharacterBase Init()
    {
        id = 2;
        hp = 50;
        def = 0;
        speed = 2;
        speed_dif = 2;
        dmg_k = 1f;
        moves = 4;
        energy = 0;
        name = "Ведьма";
        description = "Наводит порчу на понос, если не купить талисман на удачу";
        SkillsPerTurn = new List<List<PlayerSkill>> {
            new List<PlayerSkill>() { DataDicts.EnemySkillSet[9], DataDicts.EnemySkillSet[8] },
            new List<PlayerSkill>() { DataDicts.EnemySkillSet[9], DataDicts.EnemySkillSet[10] },
            new List<PlayerSkill>() { DataDicts.EnemySkillSet[9], DataDicts.EnemySkillSet[11] }
        };
        init_none_k = 1.2f;
        init_darkness_k = 0.75f;
        init_light_k = 1.75f;
        init_fire_k = 1.2f;
        base.Init();
        return this;
    }

    public EnemyWitch()
    {
        passive_ids = new List<int>();
        passive_ids.Add(5);
        passive_ids.Add(6);
    }

    public override void CreateStatsAccourdingToLevel()
    {
        base.CreateStatsAccourdingToLevel();
    }
}

public class EnemyCultist : EnemyBase
{
    public override CharacterBase Init()
    {
        id = 3;
        hp = 45;
        def = 0;
        speed = 2;
        speed_dif = 2;
        dmg_k = 1f;
        moves = 3;
        energy = 0;
        name = "культист";
        description = "член опасного лесного культа, который варит мет из жертв";
        SkillsPerTurn = new List<List<PlayerSkill>> {
            new List<PlayerSkill>() { DataDicts.EnemySkillSet[12], DataDicts.EnemySkillSet[13] },
            new List<PlayerSkill>() { DataDicts.EnemySkillSet[12], DataDicts.EnemySkillSet[14] }
        };
        init_none_k = 0.9f;
        init_darkness_k = 0.75f;
        init_light_k = 1.2f;
        init_fire_k = 1f;
        init_dendro_k = 0.9f;
        base.Init();
        return this;
    }

    public EnemyCultist()
    {
        passive_ids = new List<int>();
        passive_ids.Add(7);
        passive_ids.Add(8);
    }

    public override void CreateStatsAccourdingToLevel()
    {
        base.CreateStatsAccourdingToLevel();
    }
}

public class Sniper : EnemyBase
{
    public override CharacterBase Init()
    {
        id = 4;
        hp = 38;
        def = 0;
        speed = 2;
        speed_dif = 2;
        dmg_k = 2f;
        moves = 2;
        energy = 0;
        name = "Снайпер";
        MovingTowardsPlayer = false;
        description = "Один из австралийских снайперов, которые приехали охотится на снюсоедов, но в итоге выяснили, что они помогают делать отменный банкате. Кооперирование не заставило долго ждать";
        SkillsPerTurn = new List<List<PlayerSkill>> {
            new List<PlayerSkill>() { DataDicts.EnemySkillSet[16], DataDicts.EnemySkillSet[16], DataDicts.EnemySkillSet[15] },
            new List<PlayerSkill>() { DataDicts.EnemySkillSet[16], DataDicts.EnemySkillSet[16], DataDicts.EnemySkillSet[15] },
            new List<PlayerSkill>() { DataDicts.EnemySkillSet[16], DataDicts.EnemySkillSet[16], DataDicts.EnemySkillSet[17] }
        };
        init_none_k = 1.3f;
        init_darkness_k = 0.75f;
        init_light_k = 0.75f;
        base.Init();
        return this;
    }

    public Sniper()
    {
        passive_ids = new List<int>();
        passive_ids.Add(14);
        passive_ids.Add(15);
    }

    public override void CreateStatsAccourdingToLevel()
    {
        base.CreateStatsAccourdingToLevel();
    }
}

public class Snusoed : EnemyBase
{
    public override CharacterBase Init()
    {
        id = 5;
        hp = 64;
        def = 0;
        speed = 2;
        speed_dif = 2;
        dmg_k = 1.2f;
        moves = 3;
        energy = 0;
        name = "Снюсоед";
        description = "Уникальный представитель фауны колтушского леса, питается найденными в земле закладками";
        SkillsPerTurn = new List<List<PlayerSkill>> {
            new List<PlayerSkill>() { DataDicts.EnemySkillSet[19], DataDicts.EnemySkillSet[18], DataDicts.EnemySkillSet[18] },
            new List<PlayerSkill>() { DataDicts.EnemySkillSet[18], DataDicts.EnemySkillSet[19], DataDicts.EnemySkillSet[18] },
            new List<PlayerSkill>() { DataDicts.EnemySkillSet[18], DataDicts.EnemySkillSet[18] }
        };
        init_none_k = 0.8f;
        init_darkness_k = 0.7f;
        init_light_k = 1.4f;
        init_water_k = 1.4f;
        base.Init();
        return this;
    }

    public Snusoed()
    {
        passive_ids = new List<int>();
        passive_ids.Add(16);
        passive_ids.Add(17);
    }

    public override void CreateStatsAccourdingToLevel()
    {
        base.CreateStatsAccourdingToLevel();
    }
}

public class Elephant : EnemyBase
{
    public override CharacterBase Init()
    {
        id = 6;
        hp = 150;
        def = 10;
        speed = 1;
        speed_dif = 2;
        dmg_k = 1.6f;
        moves = 1;
        energy = 0;
        name = "Слоняра";
        description = "Наш слоняра - особый красноснокнижный вид карликовых слоняр, обитающий исключительно в лесах под Питером";
        SkillsPerTurn = new List<List<PlayerSkill>> {
            new List<PlayerSkill>() { DataDicts.EnemySkillSet[20]},
            new List<PlayerSkill>() { DataDicts.EnemySkillSet[22], DataDicts.EnemySkillSet[23]},
            new List<PlayerSkill>() { DataDicts.EnemySkillSet[21] }
        };
        init_none_k = 1.4f;
        init_darkness_k = 0.8f;
        init_light_k = 0.8f;
        init_water_k = 0.8f;
        init_fire_k = 0.8f;
        init_dendro_k = 0.8f;

        base.Init();
        return this;
    }

    public Elephant()
    {
        passive_ids = new List<int>();
        passive_ids.Add(18);
        passive_ids.Add(19);
    }

    public override void CreateStatsAccourdingToLevel()
    {
        base.CreateStatsAccourdingToLevel();
    }
}

public class Drevo : EnemyBase
{
    public override CharacterBase Init()
    {
        id = 7;
        hp = 120;
        def = 0;
        speed = 0;
        speed_dif = 0;
        dmg_k = 1f;
        moves = 0;
        energy = 0;
        name = "Мудрое Древо";
        description = "Оно было обычным дерево, однако под ним осказалось закопано слишком много закладок, что вызвало появление сознания и обретение мудрости";
        SkillsPerTurn = new List<List<PlayerSkill>> {
            new List<PlayerSkill>() { DataDicts.EnemySkillSet[25], DataDicts.EnemySkillSet[26]},
        };
        init_none_k = 1f;
        init_darkness_k = 1f;
        init_light_k = 1f;
        init_water_k = 0.6f;
        init_fire_k = 1.3f;
        init_dendro_k = 0.6f;

        base.Init();
        return this;
    }

    public Drevo()
    {
        passive_ids = new List<int>();
        passive_ids.Add(18);
        passive_ids.Add(23);
    }

    public override void CreateStatsAccourdingToLevel()
    {
        base.CreateStatsAccourdingToLevel();
    }
}

public class Python : EnemyBase
{
    public override CharacterBase Init()
    {
        id = 8;
        hp = 67;
        def = 0;
        speed = 3;
        speed_dif = 0;
        dmg_k = 0.9f;
        moves = 2;
        energy = 0;
        name = "Гигантский питон";
        description = "Он запасается энергией на зиму";
        SkillsPerTurn = new List<List<PlayerSkill>> {
            new List<PlayerSkill>() { DataDicts.EnemySkillSet[28], DataDicts.EnemySkillSet[29]},
            new List<PlayerSkill>() { DataDicts.EnemySkillSet[28], DataDicts.EnemySkillSet[29]},
            new List<PlayerSkill>() { DataDicts.EnemySkillSet[28], DataDicts.EnemySkillSet[29], DataDicts.EnemySkillSet[29]},
        };
        init_none_k = 1.2f;
        init_darkness_k = 0.8f;
        init_light_k = 0.8f;
        init_water_k = 0.8f;
        init_fire_k = 1.2f;
        init_dendro_k = 1f;

        base.Init();
        return this;
    }

    public Python()
    {
        passive_ids = new List<int>();
        passive_ids.Add(24);
        passive_ids.Add(25);
    }

    public override void CreateStatsAccourdingToLevel()
    {
        base.CreateStatsAccourdingToLevel();
    }
}

public class PunishingBird : EnemyBase
{
    public override CharacterBase Init()
    {
        id = 9;
        hp = 666;
        def = 0;
        speed = 10;
        speed_dif = 0;
        dmg_k = 6.66f;
        moves = 10;
        energy = 0;
        name = "Карающая птица";
        description = "";
        SkillsPerTurn = new List<List<PlayerSkill>>();
        init_none_k = 0.66f;
        init_darkness_k = 0.66f;
        init_light_k = 0.66f;
        init_water_k = 0.66f;
        init_fire_k = 0.66f;
        init_dendro_k = 0.66f;

        base.Init();
        return this;
    }

    public PunishingBird()
    {
        passive_ids = new List<int>();
        passive_ids.Add(26);
    }

    public override void CreateStatsAccourdingToLevel()
    {
        base.CreateStatsAccourdingToLevel();
    }
}