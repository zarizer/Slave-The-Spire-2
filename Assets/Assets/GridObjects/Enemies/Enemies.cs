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

}
