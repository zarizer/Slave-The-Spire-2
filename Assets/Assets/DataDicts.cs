using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataDicts 
{

    public static Dictionary<int, PlayerSkill> SkillSet = new Dictionary<int, PlayerSkill>()
    {
        { -1, new TestSkill1() },
        { -2, new TestSkill2() },
    };

    public static Dictionary<int, CharacterBase> CharacterSet = new Dictionary<int, CharacterBase>()
    {
        { -1, new TestCharacter() },
    };

    public static Dictionary<int, EnemyBase> EnemySet = new Dictionary<int, EnemyBase>()
    {
        { -1, new TestEnemy() },
    };
}
