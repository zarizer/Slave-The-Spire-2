using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBase : CharacterBase
{
    List<PlayerSkill> SkillQueue;

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
}
