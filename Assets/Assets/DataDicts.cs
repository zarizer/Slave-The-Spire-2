using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataDicts 
{

    public static Dictionary<int, PlayerSkill> SkillSet = new Dictionary<int, PlayerSkill>()
    {
        { -1, new TestSkill1().Init() },
        { -2, new TestSkill2().Init() },
        { -3, new TestEnemySkill1().Init() },
        { -4, new TestEnemySkill2().Init() },
        { -5, new TestEnemySkill3().Init() },
    };

    public static Dictionary<int, CharacterBase> CharacterSet = new Dictionary<int, CharacterBase>()
    {
        { -1, new TestCharacter().Init() },
    };

    public static Dictionary<int, EnemyBase> EnemySet = new Dictionary<int, EnemyBase>()
    {
        { -1, (EnemyBase)new TestEnemy().Init() },
    };



}
