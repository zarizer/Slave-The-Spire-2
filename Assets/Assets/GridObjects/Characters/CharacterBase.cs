using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterBase
{
    public int hp;
    public int def;
    public int speed;
    public int speed_dif;
    public float base_dmg;
    public int moves;

    public PlayerSkill Skill1;
    public PlayerSkill Skill2;
    public PlayerSkill Skill3;  
    public PlayerSkill Skill4;

    public int skill_id1;
    public int skill_id2;
    public int skill_id3;
    public int skill_id4;

    public virtual void Init() { }

    public PlayerSkill GetSkillByID(int id)
    {
        PlayerSkill ret_skill;

        ret_skill = DataDicts.SkillSet[id];
        if (ret_skill == null ) ret_skill = new TestSkill1();

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
}

