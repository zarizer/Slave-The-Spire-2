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

    public static Dictionary<int, CharacterBase> CharacterSet = new Dictionary<int, CharacterBase>()
    {
        { 0, new TestCharacter().Init() },
    };

    public static Dictionary<int, EnemyBase> EnemySet = new Dictionary<int, EnemyBase>()
    {
        { 0, (EnemyBase)new TestEnemy().Init() },
    };

    public static Dictionary<int, ObstacleBase> ObstacleSet = new Dictionary<int, ObstacleBase>()
    {
        { 0, (ObstacleBase)new SimpleStone().Init() },
        { 1, (ObstacleBase)new CharacterSpawn().Init() },
    };



}
