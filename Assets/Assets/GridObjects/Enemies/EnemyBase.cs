using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBase : CharacterBase
{
    public List<PlayerSkill> SkillQueue = new List<PlayerSkill>();
    public List<List<PlayerSkill>> SkillsPerTurn; 
    public List<Roll> CurrentRolls = new List<Roll>();
    public Transform RollsUIMenu;

    public int DebugTurn;

    public override CharacterBase Init()
    {
        InitCurStats();
        CreateSkills();
        
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

    public virtual new EnemyBase Clone()
    {
        return new EnemyBase(this);
    }

   
    public void CreateSkills()
    {
        int turn = DebugTurn;

        SkillQueue.Clear();
        Debug.Log("skill count:" + SkillsPerTurn.Count);
        foreach (PlayerSkill skill in SkillsPerTurn[turn % SkillsPerTurn.Count])
        {
            SkillQueue.Add(skill);
        }
    }


    public void CreateNextRolls()
    {
        if (SkillQueue.Count == 0) { Debug.Log("No more enemy skills on:" + name); return; }
        foreach (Roll roll in SkillQueue[0].rolls) {
            CurrentRolls.Add(new Roll(roll));
        }
        SkillQueue.RemoveAt(0);
    }


}
