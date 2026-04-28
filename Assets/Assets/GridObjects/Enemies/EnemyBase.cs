using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : CharacterBase
{
    List<PlayerSkill> SkillQueue;

    public override void Init()
    {
        InitCurStats();
    }

    public virtual void SetNextSkills()
    {
    
    }
}
