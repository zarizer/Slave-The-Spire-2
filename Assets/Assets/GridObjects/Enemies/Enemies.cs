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
        light_k = 2.5f;
        darkness_k = 0.1f;
        fire_k = 1.5f;
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
        none_k = 1.2f;
        water_k = 0.75f;
        fire_k = 1.2f;
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
        none_k = 1.2f;
        darkness_k = 0.75f;
        light_k = 1.75f;
        fire_k = 1.2f;
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
            //new List<PlayerSkill>() { DataDicts.EnemySkillSet[12], DataDicts.EnemySkillSet[13] },
            //new List<PlayerSkill>() { DataDicts.EnemySkillSet[12], DataDicts.EnemySkillSet[14] }
        };
        none_k = 0.9f;
        darkness_k = 0.75f;
        light_k = 1.2f;
        fire_k = 1f;
        dendro_k = 0.9f;
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