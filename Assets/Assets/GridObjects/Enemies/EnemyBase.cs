using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBase : CharacterBase
{
    public List<PlayerSkill> SkillQueue = new List<PlayerSkill>();
    public List<List<PlayerSkill>> SkillsPerTurn; 
    public Transform RollsUIMenu;

    public int DebugTurn;

    public override CharacterBase Init()
    {
        InitCurStats();
        CreateSkills(0);
        CreateStatsAccourdingToLevel();
        
        return this;
    }


    public virtual void SetNextSkills()
    {
    
    }

    public EnemyBase() : base()
    {

    }
    public EnemyBase(EnemyBase other) : base(other)
    {
        SkillQueue = other.SkillQueue;
        if (other.SkillsPerTurn != null)
        {
            SkillsPerTurn = new List<List<PlayerSkill>>();
            foreach (var turnSkills in other.SkillsPerTurn)
            {
                SkillsPerTurn.Add(new List<PlayerSkill>(turnSkills));
            }
        }
    }

    public virtual new CharacterBase Clone()
    {
        return new EnemyBase(this);
    }

    public override void UpdateStatsOnNewTurn()
    {
        cur_moves = moves;
        if (level > 40) cur_moves++;
        if (level > 85) cur_moves++;
    }
    public void CreateSkills(int cur_turn)
    {
        int turn = cur_turn;

        SkillQueue.Clear();
        CurrentRolls.Clear();
        //Debug.Log(SkillsPerTurn);
        if (SkillsPerTurn.Count <= 0) return;
        foreach (PlayerSkill skill in SkillsPerTurn[turn % SkillsPerTurn.Count])
        {
            SkillQueue.Add(skill);
        }
    }


    public bool CreateNextRolls()
    {
        if (SkillQueue.Count == 0) { Debug.Log("No more enemy skills on:" + name); return false; }
        foreach (Roll roll in SkillQueue[0].rolls) {
            var cur_roll = new Roll(roll);
            cur_roll.skill.character = this;
            CurrentRolls.Add(cur_roll);
        }
        SkillQueue.RemoveAt(0);
        return true;
    }

    public override void CreateStatsAccourdingToLevel()
    {
        base.CreateStatsAccourdingToLevel();
        //Debug.Log("Level:" + level.ToString() + "  k:" + (1 + ((float)level) / 12).ToString());
        cur_hp = (int)(cur_hp * (1 + ((float)level) / 12));
        cur_def = (int)(cur_def * (1 + ((float)level) / 12));
        float baff_k = (1 - (float)level / 500);
        cur_dmg_k += (float)Math.Round((level / 4) / 10f, 2);
        fire_k = (float)Math.Round(fire_k * baff_k,2);
        water_k = (float)Math.Round(water_k * baff_k, 2);
        dendro_k = (float)Math.Round(dendro_k * baff_k, 2);
        light_k = (float)Math.Round(light_k * baff_k, 2);
        darkness_k = (float)Math.Round(darkness_k * baff_k, 2);
        none_k = (float)Math.Round(none_k * baff_k, 2);



        for( int i = 0; i< passive_levels.Count; i++)
        {
            passive_levels[i] = 1 + level / 40;
        }


    }

}
