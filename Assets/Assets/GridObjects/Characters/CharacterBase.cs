using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class CharacterBase
{
    public int id;
    public int hp;
    public int def;
    public int speed;
    public int speed_dif;
    public float dmg_k = 1;
    public int moves;
    public int energy;
    public string name;
    public string description;
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

    public PlayerSkill Skill1;
    public PlayerSkill Skill2;
    public PlayerSkill Skill3;  
    public PlayerSkill Skill4;

    public int skill_id1;
    public int skill_id2;
    public int skill_id3;
    public int skill_id4;



    public virtual CharacterBase Init() 
    {
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

    protected void InitSkills()
    {
        Skill1 = GetSkillByID(skill_id1);
        Skill2 = GetSkillByID(skill_id2);
        Skill3 = GetSkillByID(skill_id3);
        Skill4 = GetSkillByID(skill_id4);
    }

    protected void InitCurStats()
    {
        cur_hp = hp;
        cur_def = def;
        cur_speed = speed;
        cur_speed_dif = speed_dif;
        cur_moves = moves;
        cur_dmg_k = dmg_k;
        cur_energy = energy;
    }

    public CharacterBase()
    {

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
        level = other.level;
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
        Skill2 = other.Skill2;
        Skill3 = other.Skill3;
        Skill4 = other.Skill4;

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
    }

    public virtual CharacterBase Clone()
    {
        return new CharacterBase(this);
    }

    public virtual void GetDamage(Damage damage)
    {

        int dmg = GetRealDamage(damage);
        int cur_dmg = dmg - cur_def;
        if (cur_dmg > 0)
        {
            cur_hp -= cur_dmg;
            cur_def = 0;
        }
        else
        {
            cur_def -= dmg;
        }
    } 

    int GetRealDamage(Damage damage)
    {
        int dmg = damage.damage;
        dmg = (int)(dmg * damage.from.cur_dmg_k);
        if (damage.element == Element.fire) dmg = (int)(dmg * fire_k);
        if (damage.element == Element.water) dmg = (int)(dmg * water_k);
        if (damage.element == Element.dendro) dmg = (int)(dmg * dendro_k);
        if (damage.element == Element.light) dmg = (int)(dmg * light_k);
        if (damage.element == Element.darkness) dmg = (int)(dmg * darkness_k);
        if (damage.element == Element.None) dmg = (int)(dmg * none_k);
        return dmg;
    }

    public virtual void UpdateStatsOnNewTurn()
    {
        Skill1.cur_use_count = 0;
        Skill2.cur_use_count = 0;
        Skill3.cur_use_count = 0;
        Skill4.cur_use_count = 0;
        cur_moves += moves;
    }
}

