using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterBase
{
    public int id;
    public int hp;
    public int def;
    public int speed;
    public int speed_dif;
    public float base_dmg;
    public int moves;
    public int energy;
    public string name;
    public string description;

    public int cur_hp;
    public int cur_def;
    public int cur_speed;
    public int cur_speed_dif;
    public float cur_base_dmg;
    public int cur_moves;
    public int cur_energy;

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

        if (DataDicts.SkillSet.ContainsKey(id))
        {
            ret_skill = new PlayerSkill(DataDicts.SkillSet[id]);
        }
        else
        {
            ret_skill = new TestSkill1();
        }

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
        cur_base_dmg = base_dmg;
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
        base_dmg = other.base_dmg;
        moves = other.moves;
        energy = other.energy;
        name = other.name;
        description = other.description;

        cur_hp = other.cur_hp;
        cur_def = other.cur_def;
        cur_speed = other.cur_speed;
        cur_speed_dif = other.cur_speed_dif;
        cur_base_dmg = other.cur_base_dmg;
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
    }

    public virtual CharacterBase Clone()
    {
        return new CharacterBase(this);
    }

    public virtual void GetDamage(int damage)
    {
        damage -= cur_def;
        if (damage > 0)
        {
            cur_hp -= damage;
        }
    } 
}

