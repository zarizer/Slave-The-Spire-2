using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBase : CharacterBase
{
    public List<PlayerSkill> SkillQueue;
    public List<List<PlayerSkill>> SkillsPerTurn;

    public int DebugTurn;

    public override CharacterBase Init()
    {
        InitCurStats();
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
        
    }

    public virtual new EnemyBase Clone()
    {
        return new EnemyBase(this);
    }

    [ContextMenu("CreateRolls")]
    public void CreateRolls()
    {
        int turn = DebugTurn;

        SkillQueue.Clear();
        foreach(PlayerSkill skill in SkillsPerTurn[turn % SkillsPerTurn.Count])
        {
            SkillQueue.Add(skill);
        }
    }

}
