using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : EnemyBase
{
    public override CharacterBase Init()
    {
        id = -1;
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
            new List<PlayerSkill>() { DataDicts.SkillSet[-3], DataDicts.SkillSet[-4] },
            new List<PlayerSkill>() { DataDicts.SkillSet[-5] }
        };
        light_k = 2.5f;
        darkness_k = 0.1f;
        fire_k = 1.5f;
        
        return this;
    }
}
