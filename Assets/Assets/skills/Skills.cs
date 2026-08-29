using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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

        foreach (var skill in player_skills)
        {
            DataDicts.PlayerSkillSet[skill.id] = skill;
        }

        foreach (var skill in enemy_skills)
        {
            DataDicts.EnemySkillSet[skill.id] = skill;
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
            if (roll_obj["selfDamage"] == null || roll_obj["selfDamage"].Value<int>() == 1)
            {
                roll.selfDamage = true;
            }
            else
            {
                roll.selfDamage = false;
            }
            roll.Description = roll_obj["description"].Value<string>();
            roll.radius = roll_obj["radius"].Value<int>();
            roll.rollRadius = GetRadius(roll_obj["rollRadius"].Value<string>());
            roll.rollType = GetType(roll_obj["rollType"].Value<string>());
            roll.element = GetElement(roll_obj["element"].Value<string>());

            // Новая система SpecialEffects
            if (roll_obj["specialEffects"] != null)
            {
                foreach (JToken effect_obj in roll_obj["specialEffects"] as JArray)
                {
                    SkillEffect effect = new SkillEffect();

                    // Базовые параметры эффекта
                    effect.roll = roll;
                    effect.id = effect_obj["id"]?.Value<int>() ?? 0;
                    effect.effectType = GetEffectType(effect_obj["effectType"]?.Value<string>() ?? "ApplyStatus");
                    effect.targetType = GetTargetType(effect_obj["target"]?.Value<string>() ?? "Self");
                    effect.triggerType = GetTriggerType(effect_obj["trigger"]?.Value<string>() ?? "OnHit");
                    effect.value = effect_obj["value"]?.Value<int>() ?? 0;
                    effect.duration = effect_obj["duration"]?.Value<int>() ?? 0;
                    effect.chance = effect_obj["chance"]?.Value<float>() ?? 100f;
                    effect.element = GetElement( effect_obj["element"]?.Value<string>() ?? "None");


                    // Дополнительные параметры для разных типов эффектов
                    if (effect_obj["statusId"] != null)
                        effect.statusId = effect_obj["statusId"].Value<int>();

                    if (effect_obj["skillId"] != null)
                        effect.skillId = effect_obj["skillId"].Value<int>();

                    if (effect_obj["modifierType"] != null)
                        effect.modifierType = effect_obj["modifierType"].Value<string>();

                    if (effect_obj["modifierValue"] != null)
                        effect.modifierValue = effect_obj["modifierValue"].Value<float>();

                    if (effect_obj["healAmount"] != null)
                        effect.healAmount = effect_obj["healAmount"].Value<int>();

                    if (effect_obj["shieldAmount"] != null)
                        effect.shieldAmount = effect_obj["shieldAmount"].Value<int>();

                    if (effect_obj["damageModifier"] != null)
                        effect.damageModifier = effect_obj["damageModifier"].Value<float>();

                    if (effect_obj["SummonObjectType"] != null)
                        effect.SummonObjectType = effect_obj["SummonObjectType"].Value<int>();

                    if (effect_obj["SummonObjectId"] != null)
                        effect.SummonObjectId = effect_obj["SummonObjectId"].Value<int>();

                    if (effect_obj["CustomId"] != null)
                        effect.customId = effect_obj["CustomId"].Value<int>();

                    // Добавляем эффект
                    roll.effects.Add(effect);
                }
            }

            roll.MakeDamagePositions();
            skill.rolls.Add(roll);
        }

        return skill;
    }

    // Новые методы для парсинга
    static EffectType GetEffectType(string effectType)
    {
        return Enum.TryParse(effectType, true, out EffectType result) ? result : EffectType.ApplyStatus;
    }

    static TargetType GetTargetType(string target)
    {
        return Enum.TryParse(target, true, out TargetType result) ? result : TargetType.Self;
    }

    static TriggerType GetTriggerType(string trigger)
    {
        return Enum.TryParse(trigger, true, out TriggerType result) ? result : TriggerType.OnHit;
    }

    static RollDist GetDist(string rollDist)
    {
        return Enum.TryParse(rollDist, true, out RollDist result) ? result : RollDist.Any;
    }

    static RollRadius GetRadius(string rollRadius)
    {
        return Enum.TryParse(rollRadius, true, out RollRadius result) ? result : RollRadius.Field;
    }

    static Element GetElement(string rollElement)
    {
        return Enum.TryParse(rollElement, true, out Element result) ? result : Element.None;
    }

    static RollType GetType(string rollType)
    {
        return Enum.TryParse(rollType, true, out RollType result) ? result : RollType.Atk;
    }
}

// ===== НОВЫЕ КЛАССЫ ДЛЯ СИСТЕМЫ ЭФФЕКТОВ =====

/// <summary>
/// Тип эффекта
/// </summary>
public enum EffectType
{
    ApplyStatus,        
    RemoveStatus,       // Снятие статуса
    Heal,               // Лечение
    HealPercent,        // Лечение в процентах от здоровья цели
    Damage,            // Наносит урон
    Defence,            // Даёт статическую защиту
    Energy,             // Даёт эренгию
    Moves,              // Даёт скорость
    //ModifyStat,        // Изменение статов
    ModifyDamage,      // Изменение урона
    //ModifyCooldown,    // Изменение кулдауна
    //AddResource,       // Добавление ресурса
    //RemoveResource,    // Удаление ресурса
    Summon,            // Призыв
    //Teleport,          // Телепортация
    //Clone,             // Клонирование
    //ModifySpeed,       // Изменение скорости
    //Invulnerability,   // Неуязвимость
    //ReflectDamage,     // Отражение урона
    //Execute,           // Казнь
    Custom             // Пользовательское событие (по ID)
}

/// <summary>
/// Тип цели эффекта
/// </summary>
public enum TargetType
{
    Self,              
    Target,            
    AllEnemies,        
    AllAllies,         
    RandomEnemy,       
    RandomAlly,        
    All,       
    Cells,
    Custom             // Пользовательский (по ID)
}

public enum TriggerType
{
    OnHit,
    OnMiss,
    OnTarget,
    OnUse,
    OnEmptyCell,
    Custom             // Пользовательский (по ID)
}

/// <summary>
/// Класс эффекта навыка
/// </summary>
[System.Serializable]
public class SkillEffect
{
    public Roll roll;
    public int id;                      // ID эффекта
    public EffectType effectType;       // Тип эффекта
    public TargetType targetType;       // Тип цели
    public TriggerType triggerType;     // Тип триггера
    public int value;                   // Основное значение
    public int duration;                // Длительность (в ходах)
    public float chance;               // Шанс срабатывания (0-100)

    // Дополнительные параметры для разных типов эффектов
    public int statusId;               // ID статуса для ApplyStatus/RemoveStatus
    public int skillId;                // ID навыка для ModifyCooldown или Summon
    public string modifierType;        // Тип модификации для ModifyStat (health, damage, defense, speed)
    public float modifierValue;        // Значение модификации
    public int healAmount;             // Количество лечения
    public int shieldAmount;           // Количество щита
    public float damageModifier;       // Модификатор урона (для ModifyDamage)
    public int customId;              // ID для пользовательских эффектов
    public int SummonObjectType; //0 - character, 1 - enemy, 2 - obstacle
    public int SummonObjectId; // ID для призыва объекта
    public Element element;

    // Вызов эффекта
    public void Execute(CharacterBase caster, CharacterBase target, RollContext context)
    {
        // Проверка шанса
        if (UnityEngine.Random.Range(0f, 100f) > chance)
            return;

        // Проверка триггера
        if (!CheckTrigger(context))
            return;

        // Определяем цель
        List<CharacterBase> targets = GetTargets(caster, target, context);
        // Применяем эффект к каждой цели
        foreach (var t in targets)
        {
            ApplyEffect(caster, t, context);
        }
    }

    private bool CheckTrigger(RollContext context)
    {
        // Если контекст null, пропускаем
        if (context == null) return false;

        switch (triggerType)
        {
            case TriggerType.OnEmptyCell:
                return context.TargetCell.object_ == null ;
            case TriggerType.OnHit:
                return context.IsHit;

            case TriggerType.OnMiss:
                return context.IsMiss;

            case TriggerType.OnTarget:
                return true;

            case TriggerType.OnUse:
                return true; // OnUse всегда срабатывает при использовании

            case TriggerType.Custom:
                // Для пользовательских триггеров проверяем по ID
                return CheckCustomTrigger(context);

            default:
                return false;
        }
    }

    // Дополнительный метод для пользовательских триггеров
    private bool CheckCustomTrigger(RollContext context)
    {
        // Если есть кастомные данные в контексте
        if (context.CustomData != null && context.CustomData.ContainsKey("customTriggerId"))
        {
            int customId = (int)context.CustomData["customTriggerId"];
            return customId == this.customId; // Сравниваем с ID эффекта
        }
        return false;
    }

    private List<CharacterBase> GetTargets(CharacterBase caster, CharacterBase target, RollContext context)
    {
        List<CharacterBase> targets = new List<CharacterBase>();

        switch (targetType)
        {
            case TargetType.Cells:
                targets.Add(caster);//при наведении на клетки не используется
                break;
            case TargetType.Self:
                targets.Add(caster);
                break;
            case TargetType.Target:
                if (target != null) targets.Add(target);
                break;
            case TargetType.AllEnemies:
                if (caster.object_.GetComponent<GriddableObject>().GType_ == GriddableObject.GriddableObjectType.Character)
                {
                    foreach (var t in ResoursesDict.GetClass<CameraController>().field_.GridEnemies)
                    {
                        targets.Add(t.GetCharacter());
                    }
                }
                if (caster.object_.GetComponent<GriddableObject>().GType_ == GriddableObject.GriddableObjectType.Enemy)
                {
                    foreach (var t in ResoursesDict.GetClass<CameraController>().field_.GridCharacters)
                    {
                        targets.Add(t.GetCharacter());
                    }
                }
                break;
            case TargetType.AllAllies:
                if (caster.object_.GetComponent<GriddableObject>().GType_ == GriddableObject.GriddableObjectType.Character)
                {
                    foreach (var t in ResoursesDict.GetClass<CameraController>().field_.GridCharacters)
                    {
                        targets.Add(t.GetCharacter());
                    }
                }
                if (caster.object_.GetComponent<GriddableObject>().GType_ == GriddableObject.GriddableObjectType.Enemy)
                {
                    foreach (var t in ResoursesDict.GetClass<CameraController>().field_.GridEnemies)
                    {
                        targets.Add(t.GetCharacter());
                    }
                }
                break;
            case TargetType.RandomEnemy:
                if (caster.object_.GetComponent<GriddableObject>().GType_ == GriddableObject.GriddableObjectType.Character)
                {
                    int rand = StaticFuncs.RandomRangeInclusive(0, ResoursesDict.GetClass<CameraController>().field_.GridEnemies.Count - 1);
                    targets.Add((ResoursesDict.GetClass<CameraController>().field_.GridEnemies[rand]).GetCharacter());
                }
                if (caster.object_.GetComponent<GriddableObject>().GType_ == GriddableObject.GriddableObjectType.Enemy)
                {
                    int rand = StaticFuncs.RandomRangeInclusive(0, ResoursesDict.GetClass<CameraController>().field_.GridCharacters.Count - 1);
                    targets.Add((ResoursesDict.GetClass<CameraController>().field_.GridCharacters[rand]).GetCharacter());
                }
                break;
            case TargetType.RandomAlly:
                if (caster.object_.GetComponent<GriddableObject>().GType_ == GriddableObject.GriddableObjectType.Character)
                {
                    int rand = StaticFuncs.RandomRangeInclusive(0, ResoursesDict.GetClass<CameraController>().field_.GridCharacters.Count - 1);
                    targets.Add((ResoursesDict.GetClass<CameraController>().field_.GridCharacters[rand]).GetCharacter());
                }
                if (caster.object_.GetComponent<GriddableObject>().GType_ == GriddableObject.GriddableObjectType.Enemy)
                {
                    int rand = StaticFuncs.RandomRangeInclusive(0, ResoursesDict.GetClass<CameraController>().field_.GridEnemies.Count - 1);
                    targets.Add((ResoursesDict.GetClass<CameraController>().field_.GridEnemies[rand]).GetCharacter());
                }
                break;
            case TargetType.All:
                foreach (var t in ResoursesDict.GetClass<CameraController>().field_.GridEnemies)
                {
                    targets.Add(t.GetCharacter());
                }
                foreach (var t in ResoursesDict.GetClass<CameraController>().field_.GridCharacters)
                {
                    targets.Add(t.GetCharacter());
                }
                break;
            case TargetType.Custom:
                //CUSTOM
                break;
        }

        return targets;
    }

    private void ApplyEffect(CharacterBase caster, CharacterBase target, RollContext context)
    {
        switch (effectType)
        {
            case EffectType.ApplyStatus:
                ApplyStatus(target, caster);
                break;
            case EffectType.RemoveStatus:
                RemoveStatus(target);
                break;
            case EffectType.Heal:
                HealTarget(target);
                break;
            case EffectType.Damage:
                DamageTarget(caster, target);
                break;
            case EffectType.HealPercent:
                HealPercent(caster, target);
                break;
            case EffectType.Defence:
                ApplyShield(target);
                break;
            case EffectType.Energy:
                ApplyEnergy(target);
                break;
            case EffectType.Moves:
                ApplyMoves(caster, target);
                break;
            case EffectType.ModifyDamage:
                ModifyDamage(target);
                break;
                /*case EffectType.ModifyStat:
                    ModifyStat(target);
                    break;
                
                case EffectType.ModifyCooldown:
                    ModifyCooldown(target);
                    break;
                case EffectType.AddResource:
                    AddResource(target);
                    break;
                case EffectType.RemoveResource:
                    RemoveResource(target);
                    break;
                */
                case EffectType.Summon:
                    SummonUnit(caster, context);
                    break;
                /*
                case EffectType.Teleport:
                    TeleportUnit(target);
                    break;
                case EffectType.Clone:
                    CloneUnit(caster);
                    break;
                case EffectType.ModifySpeed:
                    ModifySpeed(target);
                    break;
                case EffectType.Invulnerability:
                    ApplyInvulnerability(target);
                    break;
                case EffectType.ReflectDamage:
                    ApplyReflectDamage(target);
                    break;
                case EffectType.Execute:
                    ExecuteTarget(caster, target);
                    break;*/
                case EffectType.Custom:
                    ExecuteCustomEffect(caster, target, context);
                    break;
        }
    }

    // Реализация различных эффектов

    private void ApplyEnergy(CharacterBase target)
    {
        // Применяем статус по ID
        target.GetEnergy(value);
    }
    private void ApplyStatus(CharacterBase target, CharacterBase caster)
    {
        // Применяем статус по ID
        GridCharacter.ApplyBattleEffect(DataDicts.EffectTypes[statusId], value, duration, target, caster);
    }

    private void RemoveStatus(CharacterBase target)
    {
        // Снимаем статус по ID
        Debug.Log($"Removing status {statusId} from {target.name}");
    }

    private void HealTarget(CharacterBase target)
    {
        target.Heal(healAmount, roll.skill.character);
    }

    private void DamageTarget(CharacterBase caster, CharacterBase target)
    {
        Damage damage = new Damage(value, element, caster);
        target.GetDamage(damage);
    }

    private void HealPercent(CharacterBase caster, CharacterBase target)
    {
        target.Heal((int)(((float)target.start_hp/100)*value), caster);
    }
   
    private void ApplyShield(CharacterBase target)
    {
        target.GetDefence(value, roll.skill.character);
    }

    private void ApplyMoves(CharacterBase caster, CharacterBase target)
    {
        target.GetMoves(value, caster);
    }

    private void ModifyStat(CharacterBase target)
    {
        Debug.Log($"Modifying stat {modifierType} by {modifierValue} on {target.name}");
    }

    private void ModifyDamage(CharacterBase target)
    {
        target.GetDamageK(value);
    }

    private void ModifyCooldown(CharacterBase target)
    {
        Debug.Log($"Modifying cooldown of skill {skillId} on {target.name}");
    }

    private void AddResource(CharacterBase target)
    {
        Debug.Log($"Adding {value} resource to {target.name}");
    }

    private void RemoveResource(CharacterBase target)
    {
        Debug.Log($"Removing {value} resource from {target.name}");
    }

    private void SummonUnit(CharacterBase caster, RollContext context)
    {
        if (context.TargetCell.object_ == null)
        {
            LevelObject obj_data = new LevelObject();
            obj_data.isCustomLevel = true;
            obj_data.level = caster.level;
            LevelData level_data = new LevelData();
            level_data.minLevel = caster.level;
            level_data.maxLevel = caster.level;
            GriddableObject obj = null;
            if (SummonObjectType == 0) { 
                obj = ResoursesDict.GetClass<CameraController>().field_.CreateGridObject(GriddableObject.GriddableObjectType.Character,
                SummonObjectId, context.TargetCell.x_, context.TargetCell.y_, obj_data, level_data);
            }
            if (SummonObjectType == 1)
            {
                obj = ResoursesDict.GetClass<CameraController>().field_.CreateGridObject(GriddableObject.GriddableObjectType.Enemy,
                    SummonObjectId, context.TargetCell.x_, context.TargetCell.y_, obj_data, level_data);
            }
            if (SummonObjectType == 2)
            {
                obj = ResoursesDict.GetClass<CameraController>().field_.CreateGridObject(GriddableObject.GriddableObjectType.Obstacle,
                     SummonObjectId, context.TargetCell.x_, context.TargetCell.y_, obj_data, level_data);
            }
            if (obj != null) {
                if (caster.object_.GetComponent<GriddableObject>().player_) obj.specialValue = 1;
                else obj.specialValue = 0;
            }
        }
    }

    private void TeleportUnit(CharacterBase target)
    {
        Debug.Log($"Teleporting {target.name}");
    }

    private void CloneUnit(CharacterBase caster)
    {
        Debug.Log($"Cloning {caster.name}");
    }

    private void ModifySpeed(CharacterBase target)
    {
        Debug.Log($"Modifying speed by {modifierValue} on {target.name}");
    }

    private void ApplyInvulnerability(CharacterBase target)
    {
        Debug.Log($"Applying invulnerability to {target.name} for {duration} turns");
    }

    private void ApplyReflectDamage(CharacterBase target)
    {
        Debug.Log($"Applying damage reflection to {target.name} for {duration} turns");
    }

    private void ExecuteTarget(CharacterBase caster, CharacterBase target)
    {
        // Казнь - мгновенное убийство цели с низким HP
        Debug.Log($"Executing {target.name}");
    }

    private void ExecuteCustomEffect(CharacterBase caster, CharacterBase target, RollContext context)
    {
        Debug.Log($"Executing custom effect {customId} on {target.name}");
        if (customId == 0)
        {
            int k = caster.cur_energy / 5;
            GridCharacter.ApplyBattleEffect(DataDicts.EffectTypes[1002], k, 0, target, caster, true);
            caster.cur_energy = 0;
        }
    }
}

/// <summary>
/// Контекст для роллов
/// </summary>
public class RollContext
{
    public bool IsHit { get; set; }
    public bool IsCrit { get; set; }
    public bool IsMiss { get; set; }
    public int RollValue { get; set; }
    public CharacterBase Attacker { get; set; }
    public CharacterBase Defender { get; set; }
    public SkillEffect TriggerEffect { get; set; }
    public Dictionary<string, object> CustomData { get; set; } = new Dictionary<string, object>();
    public bool IsEmptyCell { get; set; }
    public List<GridCell> Cells { get; set; }
    public GridCell TargetCell { get; set; }
}