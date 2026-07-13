using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class DataDicts 
{

    public static Dictionary<int, PlayerSkill> PlayerSkillSet = new Dictionary<int, PlayerSkill>()
    {
        { 0, new PlayerSkill(Skills.player_skills[0]) },
        { 1, new PlayerSkill(Skills.player_skills[1]) },
    };

    public static Dictionary<int, PlayerSkill> EnemySkillSet = new Dictionary<int, PlayerSkill>()
    {
        { 0, new PlayerSkill(Skills.enemy_skills[0]) },
        { 1, new PlayerSkill(Skills.enemy_skills[1]) },
        { 2, new PlayerSkill(Skills.enemy_skills[2]) },
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
    };


}
