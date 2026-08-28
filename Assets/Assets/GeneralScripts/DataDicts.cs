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
        { 1, typeof(Shaman) },
    };

    public static Dictionary<int, Type> EnemyTypes = new Dictionary<int, Type>()
    {
        {  0, typeof(TestEnemy) },
        {  1, typeof(EnemyVosh) },
        {  2, typeof(EnemyWitch) },
        {  3, typeof(EnemyCultist) },
    };

    public static Dictionary<int, Type> ObstacleTypes = new Dictionary<int, Type>()
    {
        { 0, typeof(SimpleStone) },
        { 1, typeof(CharacterSpawn) },
        { 2, typeof(LevelChange) },
        { 3, typeof(Chapter1RandomObstacle) },
        { 4, typeof(BuffAltar) },
        { 5, typeof(Totem) },
        { 6, typeof(Chest1) },
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
        {3, typeof(PassiveVosh1) },
        {4, typeof(PassiveVosh2) },
        {5, typeof(PassiveWitch1) },
        {6, typeof(PassiveWitch2) },
        {7, typeof(PassiveCultist1) },
        {8, typeof(PassiveCultist2) },
        {9, typeof(PassiveShaman2) },
        {10, typeof(PassiveShaman3) },
    };

    public static Dictionary<int, Type> EffectTypes = new Dictionary<int, Type>()
    {
        {0, typeof(EffectPoison) },
        {1, typeof(EffectBurn) },
        {2, typeof(EffectHealProcentBySource) },
        {1001, typeof(EffectPowerUp) },
        {1002, typeof(EffectPowerDown) },
        {1003, typeof(EffectAtkUp) },
        {1004, typeof(EffectAtkDown) }

    };
}
