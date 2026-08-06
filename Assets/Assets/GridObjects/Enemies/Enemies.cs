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