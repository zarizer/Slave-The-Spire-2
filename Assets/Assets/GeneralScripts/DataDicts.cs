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
        { 2, typeof(Salty) },
        { 3, typeof(DisignCutie) },
    };

    public static Dictionary<int, Type> EnemyTypes = new Dictionary<int, Type>()
    {
        {  0, typeof(TestEnemy) },
        {  1, typeof(EnemyVosh) },
        {  2, typeof(EnemyWitch) },
        {  3, typeof(EnemyCultist) },
        {  4, typeof(Sniper) },
        {  5, typeof(Snusoed) },
        {  6, typeof(Elephant) },
        {  7, typeof(Drevo) },
        {  8, typeof(Python) },
        {  9, typeof(PunishingBird) },
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
        { 7, typeof(InvisibleWall) },
        { 8, typeof(Shop) },
        { 9, typeof(Campfire) },
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
        {11, typeof(PassiveSalty1) },
        {12, typeof(PassiveSalty2) },
        {13, typeof(PassiveSalty3) },
        {14, typeof(PassiveSniper1) },
        {15, typeof(PassiveSniper2) },
        {16, typeof(PassiveSnusoed1) },
        {17, typeof(PassiveSnusoed2) },
        {18, typeof(PassiveUnmovable) },
        {19, typeof(PassiveElephant2) },
        {20, typeof(PassiveDisignCutie1) },
        {21, typeof(PassiveDisignCutie2) },
        {22, typeof(PassiveDisignCutie3) },
        {23, typeof(PassiveDrevo1) },
        {24, typeof(PassiveVenomous) },
        {25, typeof(PassivePython1) },
        {26, typeof(PassivePunishingBird1) },
    };

    public static Dictionary<int, Type> EffectTypes = new Dictionary<int, Type>()
    {
        {0, typeof(EffectPoison) },
        {1, typeof(EffectBurn) },
        {2, typeof(EffectHealProcentBySource) },
        {3, typeof(EffectSteam) },
        {1001, typeof(EffectPowerUp) },
        {1002, typeof(EffectPowerDown) },
        {1003, typeof(EffectAtkUp) },
        {1004, typeof(EffectAtkDown) },
        {1005, typeof(EffectSpeedUp) },
        {1006, typeof(EffectSpeedDown) },

    };

    public static Dictionary<int, Type> SurafaceTypes = new Dictionary<int, Type>()
    {
        {0, typeof(SurfaceBasic) },
        {1, typeof(SurfaceSwamp) },
    };
}
