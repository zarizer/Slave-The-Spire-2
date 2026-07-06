using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Skills
{
    public static List<PlayerSkill> player_skills = new List<PlayerSkill>();
    public static List<PlayerSkill> enemy_skills = new List<PlayerSkill>();

    public static void Init()
    {

        string playerPath = Path.Combine(Application.streamingAssetsPath, "skills/PlayerSkillList.json");
        string enemyPath = Path.Combine(Application.streamingAssetsPath, "skills/EnemySkillList.json");

        string playerJson, enemyJson;

        #if UNITY_ANDROID && !UNITY_EDITOR
            playerJson = ReadFileFromStreamingAssets(playerPath);
            enemyJson = ReadFileFromStreamingAssets(enemyPath);
        #else
            playerJson = File.ReadAllText(playerPath);
            enemyJson = File.ReadAllText(enemyPath);
        #endif

        JObject JPlayerSkillsList = JObject.Parse(playerJson);
        JObject JEnemySkillsList = JObject.Parse(enemyJson);

        JArray PlayerSkillsArray = JPlayerSkillsList["skills"] as JArray;
        JArray EnemySkillsArray = JEnemySkillsList["skills"] as JArray;

        foreach (JToken obj in PlayerSkillsArray)
        {
            player_skills.Add(CreateSkill(obj, true));
        }

        foreach (JToken obj in EnemySkillsArray)
        {
            enemy_skills.Add(CreateSkill(obj, false));
        }
    }

    static PlayerSkill CreateSkill(JToken obj, bool is_player)
    {
        PlayerSkill skill = new PlayerSkill();
        skill.id = obj["id"].Value<int>();
        skill.name = obj["name"].Value<string>();
        skill.rollDist = GetDist(obj["rollDist"].Value<string>());
        skill.dist = obj["dist"].Value<int>();
        if (is_player)
        {
            skill.energy = obj["energy"].Value<int>();
            skill.max_use_count = obj["maxUseCount"].Value<int>();
        }
        foreach (JToken roll_obj in obj["rolls"] as JArray)
        {
            Roll roll = new Roll();
            roll.minRoll = roll_obj["minRoll"].Value<int>();
            roll.maxRoll = roll_obj["maxRoll"].Value<int>();
            roll.skill = skill;
            roll.radius = roll_obj["radius"].Value<int>();
            roll.rollRadius = GetRadius(roll_obj["rollRadius"].Value<string>());
            roll.rollType = GetType(roll_obj["rollType"].Value<string>());
            roll.element = GetElement(roll_obj["element"].Value<string>());
            //—ƒ≈À¿“‹ –≈¿À»«¿÷»ﬁ —œ≈÷»¿À‹Õ€’ ›‘‘≈ “Œ¬
            roll.MakeDamagePositions();
            skill.rolls.Add(roll);
        }


        return skill;
    }

    static RollDist GetDist(string rollDist)
    {
        RollDist dist = RollDist.Any;
        if (rollDist == "Any") { dist = RollDist.Any; }
        else if (rollDist == "StLine") { dist = RollDist.StLine; }
        else if (rollDist == "DgLine") { dist = RollDist.DgLine; }
        else if (rollDist == "Radius") { dist = RollDist.Radius; }
        else if (rollDist == "Other") { dist = RollDist.Other; }
        return dist;
    }

    static RollRadius GetRadius(string rollRadius)
    {
        RollRadius radius = RollRadius.Field;
        if (rollRadius == "Field") { radius = RollRadius.Field; }
        else if (rollRadius == "PlayerRadius") { radius = RollRadius.PlayerRadius; }
        else if (rollRadius == "TargetRadius") { radius = RollRadius.TargetRadius; }
        else if (rollRadius == "Single") { radius = RollRadius.Single; }
        else if (rollRadius == "StLine") { radius = RollRadius.StLine; }
        else if (rollRadius == "DgLine") { radius = RollRadius.DgLine; }
        else if (rollRadius == "Other") { radius = RollRadius.Other; }
        return radius;
    }

    static Element GetElement(string rollElement)
    {
        Element element = Element.None;
        if (rollElement == "None") { element = Element.None; }
        else if (rollElement == "Fire") { element = Element.fire; }
        else if (rollElement == "Water") { element = Element.water; }
        else if (rollElement == "Dendro") { element = Element.dendro; }
        else if (rollElement == "Light") { element = Element.light; }
        else if (rollElement == "Darkness") { element = Element.darkness; }
        return element;
    }

    static RollType GetType(string rollType)
    {
        RollType type = RollType.Atk;
        if (rollType == "Atk") { type = RollType.Atk; }
        else if (rollType == "Evade") { type = RollType.Evade; }
        else if (rollType == "Def") { type = RollType.Def; }
        else if (rollType == "Effect") { type = RollType.Effect; }
        else if (rollType == "Other") { type = RollType.Other; }
        return type;
    }
};