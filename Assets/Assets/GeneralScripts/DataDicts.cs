using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class DataDicts 
{

    public static Dictionary<int, PlayerSkill> PlayerSkillSet = new Dictionary<int, PlayerSkill>()
    {

    };

    public static Dictionary<int, PlayerSkill> EnemySkillSet = new Dictionary<int, PlayerSkill>()
    {

    };

    public static Dictionary<int, Type> CharacterTypes = new Dictionary<int, Type>()
    {
        { 0, typeof(MainCharacter) },
    };

    public static Dictionary<int, Type> EnemyTypes = new Dictionary<int, Type>()
    {
        {  0, typeof(TestEnemy) },
    };

    public static Dictionary<int, Type> ObstacleTypes = new Dictionary<int, Type>()
    {
        { 0, typeof(SimpleStone) },
        { 1, typeof(CharacterSpawn) },
        { 2, typeof(LevelChange) },
        { 3, typeof(Chapter1RandomObstacle) },
        { 4, typeof(BuffAltar) },
    };

    public static Dictionary<int, Type> BaffTypes = new Dictionary<int, Type>()
    {
        {0, typeof(HPBuff) },
        {1, typeof(DefBuff) },
        {2, typeof(StrBuff) },
        {3, typeof(ElementDefBuff) },
        {4, typeof(SpeedBuff) },
    };

    public static Dictionary<int, Type> PassiveTypes = new Dictionary<int, Type>()
    {
        {0, typeof(PassiveMain1) },
        {1, typeof(PassiveMain2) },
        {2, typeof(PassiveMain3) },
    };
}
