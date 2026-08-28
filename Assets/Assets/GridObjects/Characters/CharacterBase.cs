using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class CharacterBase
{
    public float init_time;
    public int id;
    public int hp;
    public int def;
    public int speed;
    public int speed_dif;
    public float dmg_k = 1;
    public int moves;
    public int start_hp = 0;

    public int energy;
    public string name;
    public string description;
    public string skills_description;
    public int level = 1;

    public int cur_hp;
    public int cur_def;
    public int cur_speed;
    public int cur_speed_dif;
    public float cur_dmg_k;
    public int cur_moves;
    public int cur_energy;

    public float fire_k = 1f;
    public float water_k = 1f;
    public float dendro_k = 1f;
    public float light_k = 1f;
    public float darkness_k = 1f;
    public float none_k = 1f;

    public float init_fire_k = 1f;
    public float init_water_k = 1f;
    public float init_dendro_k = 1f;
    public float init_light_k = 1f;
    public float init_darkness_k = 1f;
    public float init_none_k = 1f;

    public PlayerSkill Skill1;
    public PlayerSkill Skill2;
    public PlayerSkill Skill3;  
    public PlayerSkill Skill4;

    public int skill_id1;
    public int skill_id2;
    public int skill_id3;
    public int skill_id4;

    public List<BattleBuff> buffs = new List<BattleBuff>();
    public List<Passive> passives = new List<Passive>();
    public List<BattleEffect> effects = new List<BattleEffect>();
    public List<int> passive_ids = new List<int>();
    public List<int> passive_levels = new List<int>();
    public List<Roll> CurrentRolls = new List<Roll>();
    public GameObject object_;
    public bool is_custom_secondary_stats = false;

    public List<GridCell> TargetedCells;
    

    public virtual CharacterBase Init() 
    {
        init_time = Time.realtimeSinceStartup;
        init_fire_k = fire_k;
        init_water_k = water_k;
        init_dendro_k = dendro_k;
        init_light_k = light_k;
        init_darkness_k = darkness_k;
        init_none_k = none_k;
        InitSkills();
        InitCurStats();

        return this;
    }

    public PlayerSkill GetSkillByID(int id)
    {
        PlayerSkill ret_skill;
        if (DataDicts.PlayerSkillSet.ContainsKey(id))
        {
            ret_skill = new PlayerSkill(DataDicts.PlayerSkillSet[id]);
        }
        else
        {
            ret_skill = new PlayerSkill(DataDicts.PlayerSkillSet[0]);
        }
        ret_skill.character = this;
        ret_skill.Init();
        return ret_skill;
    }

    public void InitSkills()
    {
        Skill1 = GetSkillByID(skill_id1);
        Skill2 = GetSkillByID(skill_id2);
        Skill3 = GetSkillByID(skill_id3);
        Skill4 = GetSkillByID(skill_id4);
    }

    public void InitCurStats()
    {
        
        cur_hp = hp;
        cur_def = def;
        cur_speed = speed;
        cur_speed_dif = speed_dif;
        cur_moves = moves;
        cur_dmg_k = dmg_k;
        cur_energy = energy;
        CreateStatsAccourdingToLevel();
        start_hp = cur_hp;
    }

    public CharacterBase()
    {
        passive_levels.Add(1);
        passive_levels.Add(1);
        passive_levels.Add(1);
        passive_levels.Add(1);
        passive_levels.Add(1);
        passive_levels.Add(1);
        passive_levels.Add(1);
    }

    public CharacterBase(CharacterBase other)
    {
        id = other.id;
        hp = other.hp;
        def = other.def;
        speed = other.speed;
        speed_dif = other.speed_dif;
        dmg_k = other.dmg_k;
        moves = other.moves;
        energy = other.energy;
        if (!is_custom_secondary_stats) level = other.level;

        name = other.name;
        description = other.description;

        cur_hp = other.cur_hp;
        cur_def = other.cur_def;
        cur_speed = other.cur_speed;
        cur_speed_dif = other.cur_speed_dif;
        cur_dmg_k = other.cur_dmg_k;
        cur_moves = other.cur_moves;
        cur_energy = other.cur_energy;

        Skill1 = other.Skill1;
        Skill1.character = this;
        Skill2 = other.Skill2;
        Skill2.character = this;
        Skill3 = other.Skill3;
        Skill3.character = this;
        Skill4 = other.Skill4;
        Skill4.character = this;

        skill_id1 = other.skill_id1;
        skill_id2 = other.skill_id2;
        skill_id3 = other.skill_id3;
        skill_id4 = other.skill_id4;

        fire_k = other.fire_k;
        water_k = other.water_k;
        dendro_k = other.dendro_k;
        light_k = other.light_k;
        darkness_k = other.darkness_k;
        none_k = other.none_k;

        init_fire_k = other.init_fire_k;
        init_water_k = other.init_water_k;
        init_dendro_k = other.init_dendro_k;
        init_light_k = other.init_light_k;
        init_darkness_k = other.init_darkness_k;
        init_none_k = other.init_none_k;
    }

    public void WriteSecondaryData(CharacterBase obj)
    {
        obj.level = level;
        obj.skill_id1 = skill_id1;
        obj.skill_id2 = skill_id2;
        obj.skill_id3 = skill_id3;
        obj.skill_id4 = skill_id4;
        obj.passive_ids = new List<int>(passive_ids);
    }
    public void CreatePassives()
    { 
        passives.Clear();
        for (int i = 0; i < passive_ids.Count; i++)
        {
            var passive = Passive.GetPassiveInstance(DataDicts.PassiveTypes[passive_ids[i]]);
            passive.character = this;
            passive.level = passive_levels[i];
            passives.Add(passive);
           
        }
        CreateSkillDescription();
    }

    virtual public void CreateStatsAccourdingToLevel() 
    {
        start_hp = hp +(int)(((float)hp / 100) * 1.2f * level);
        cur_hp = hp +(int)(((float)hp / 100) * 1.2f * level);
        cur_dmg_k = (float)Math.Round(dmg_k + (0.05f*(level/4)), 2);
        cur_def = def + (int)(((float)def / 100) * 1.8f * level);
        float baff_k = (level / 25);
        fire_k = (float)Math.Round(init_fire_k - baff_k/10,2);
        water_k = (float)Math.Round(init_water_k - baff_k / 10, 2);
        dendro_k = (float)Math.Round(init_dendro_k - baff_k / 10, 2);
        light_k = (float)Math.Round(init_light_k - baff_k / 10, 2);
        darkness_k = (float)Math.Round(init_darkness_k - baff_k / 10, 2);
        none_k = (float)Math.Round(init_none_k - baff_k / 10, 2);
        cur_moves = moves + (level / 50);
        cur_energy = energy + level / 10;
    }

    public virtual CharacterBase Clone()
    {
        return new CharacterBase(this);
    }

    public void CreateSkillDescription()
    {
        skills_description = "";
        foreach (var passive in passives)
        {
            passive.UpdateAccourdingToLevel();
            passive.Init();
            skills_description += passive.Name + ":\n";
            skills_description += passive.Description + "\n\n";
        }
    }
    public virtual void GetDamage(Damage damage, bool silent = true)
    {
        if (object_ == null) return;
        Debug.Log(object_);
        int dmg = GetRealDamage(damage);
        damage.damage = dmg;

        if (damage.element != Element.True)
        {
            int cur_dmg = dmg - cur_def;
            if (cur_dmg > 0)
            {
                cur_hp -= cur_dmg;
                cur_def = 0;
                object_.GetComponent<GriddableObject>().CreateDamageText(damage, (float)cur_dmg / start_hp, false);
            }
            else
            {
                cur_def -= dmg;
                object_.GetComponent<GriddableObject>().CreateDamageText(damage, (float)dmg / start_hp, false, true);
            }
        }
        else
        {
            cur_hp -= dmg;
            object_.GetComponent<GriddableObject>().CreateDamageText(damage, (float)dmg / start_hp, false);
        }
        if (!silent) OnGetDamage(ResoursesDict.GetClass<BattleMain>().current_field, damage);
        if (cur_hp <= 0)
        {
            Death(damage);
        }
    } 

    public void GetEnergy(int value)
    {
        cur_energy += value;
        OnGetEnergy(ResoursesDict.GetClass<BattleMain>().current_field, value);
    }

    public void GetDamageK(int value)
    {
        cur_dmg_k += (float)value/10;
    }

    public int GetRealDamage(Damage damage)
    {
        int dmg = damage.damage;
        if (damage.from != null)
        {
            dmg = (int)(dmg * (damage.from.cur_dmg_k + ((float)(damage.from.level - level)) / 10));
        }
        if (damage.element == Element.fire) dmg = (int)(dmg * fire_k);
        if (damage.element == Element.water) dmg = (int)(dmg * water_k);
        if (damage.element == Element.dendro) dmg = (int)(dmg * dendro_k);
        if (damage.element == Element.light) dmg = (int)(dmg * light_k);
        if (damage.element == Element.darkness) dmg = (int)(dmg * darkness_k);
        if (damage.element == Element.None) dmg = (int)(dmg * none_k);
        return dmg;
    }

    public virtual void GetDefence(int value, CharacterBase source)
    {
        cur_def += value;
    }
    public virtual void GetMoves(int value, CharacterBase source)
    {
        cur_moves += value;
    }

    public virtual void UpdateStatsOnNewTurn()
    {
        Skill1.cur_use_count = 0;
        Skill2.cur_use_count = 0;
        Skill3.cur_use_count = 0;
        Skill4.cur_use_count = 0;
        GetMoves(moves, this);
    }

    public int GetSpecialValue()
    {
        return object_.GetComponent<GriddableObject>().specialValue;
    }
    
    public virtual void Death(Damage damage)
    {
        OnDeath(ResoursesDict.GetClass<BattleMain>().current_field, damage);
        //EffectManager.OneTimeBurst(ResoursesDict.ObjectSet["DeathEffect"], object_.transform);
        object_.GetComponent<GriddableObject>().Death();

    }
    public virtual void OnSpawn(GridField field_data)
    {
        if (object_ != null && object_.GetComponent<GriddableObject>().GType_ == GriddableObject.GriddableObjectType.Character)
        {
            object_.GetComponent<GridCharacter>().Texture.texture = IconManager.PlayerIcons[id].texture;
        }
    }
    

    public virtual void OnRemove(GridField field_data) { }

    public virtual void OnGetEnergy(GridField field_data, int value) 
    {
        foreach (var passive in passives)
        {
            passive.OnGetEnergy(field_data, value);
        }
    }

    public virtual void OnDeath(GridField field_data, Damage dmg) 
    {
        foreach (var passive in passives)
        {
            passive.OnDeath(field_data, dmg);
            if (dmg.from != null)
            {
                dmg.from.OnKill(field_data, this);
            }
        }
        foreach (var effect in effects)
        {
            effect.OnDeath(field_data, dmg);
            if (dmg.from != null)
            {
                effect.OnKill(field_data, this);
            }
        }

    }

    public virtual void OnKill(GridField field_data, CharacterBase target)
    {
        foreach (var passive in passives)
        {
            passive.OnKill(field_data, target);
        }
        foreach (var effect in effects)
        {
            effect.OnKill(field_data, target);
        }

    }

    public virtual void OnGetDamage(GridField field_data, Damage dmg) 
    {
        foreach (var passive in passives)
        {
            passive.OnGetDamage(field_data, dmg);
        }
        foreach (var effect in effects)
        {
            effect.OnGetDamage(field_data, dmg);
        }
    }

    public virtual void OnAttack(GridField field_data, CharacterBase target, Damage dmg) 
    {
        foreach (var passive in passives)
        {
            passive.OnAttack(field_data, target, dmg);
        }
        foreach (var effect in effects)
        {
            effect.OnAttack(field_data, target, dmg);
        }
    }
    public virtual void OnTurnStart(GridField field_data)
    {
        foreach (var passive in passives)
        {
            passive.OnTurnStart(field_data);
        }
        for (int i = effects.Count - 1; i >= 0; i--) 
        {
            effects[i].OnTurnStart(field_data);
        }
    }

    public virtual void OnTurnEnd(GridField field_data)
    {
        foreach (var passive in passives)
        {
            passive.OnTurnEnd(field_data);
        }
        foreach (var effect in effects)
        {
            effect.OnTurnEnd(field_data);
        }
    }

    public virtual void OnBattleStart(GridField field_data)
    {
        foreach (var passive in passives)
        {
            passive.OnBattleStart(field_data);
        }
    }

    public virtual void OnCameraTarget(GridField field_data) { }

    public virtual void OnLevelStart(GridField field_data) { }

    public virtual void Heal(int amount, CharacterBase sourse)
    {
        cur_hp += amount;
        if (cur_hp > start_hp) cur_hp = start_hp;
        if (object_.GetComponent<GriddableObject>().GType_ == GriddableObject.GriddableObjectType.Enemy ||
            object_.GetComponent<GriddableObject>().GType_ == GriddableObject.GriddableObjectType.Character)
        {
            object_.GetComponent<GriddableObject>().CreateDamageText(new Damage(amount, Element.None, sourse), (float)amount / start_hp, true);
        }
    }

    public virtual int GetXpTicketAmountToUpgrade()
    {
        return level / 20 + 1;
    }
}

