using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerSkill
{
    public int id;
    public string name;
    public int max_use_count = 1;
    public int cur_use_count = 0;
    public List<Roll> rolls = new List<Roll>();
    public int dist;
    public RollDist rollDist;
    public List<(int, int)> AttackPositions = new List<(int, int)>();

    public CharacterBase character;
    public int energy;



    public PlayerSkill()
    {

    }

    public PlayerSkill(PlayerSkill other)
    {
        id = other.id;
        name = other.name;
        rollDist = other.rollDist;
        character = other.character;
        energy = other.energy;
        dist = other.dist;
        max_use_count = other.max_use_count;
        rolls = new List<Roll>();
        for (int i = 0; i < other.rolls.Count; i++)
        {
            rolls.Add(new Roll(other.rolls[i]));
        }
        MakeAttackPositions();
    }

    public virtual PlayerSkill Init() 
    {
        MakeAttackPositions();
        foreach (Roll roll in rolls)
        {
            roll.skill = this;
        }
        return this; 
    }

    public virtual bool CheckSkillRestrictions() { return true; }
    public virtual bool CheckSkillResourses()
    {
        if (cur_use_count < max_use_count &&
            energy < character.cur_energy &&
            CheckSkillRestrictions())
        {
            return true;
        }
        ResoursesDict.GetClass<SoundMain>().PlaySound("RestrictSound");
        return false;
    }


    public void MakeAttackPositions()
    {
        if (rollDist != RollDist.Other) AttackPositions.Clear();

        if (rollDist == RollDist.Any) AttackPositions.Add((-999, -999));
        if (rollDist == RollDist.StLine)
        {
            for (int i = 0; i<dist; i++)
            {
                AttackPositions.Add((0, i));
                AttackPositions.Add((0, -i));
                AttackPositions.Add((i, 0));
                AttackPositions.Add((-i, 0));

            }
        }
        if (rollDist == RollDist.DgLine)
        {
            for (int i = 0; i < dist; i++)
            {
                AttackPositions.Add((i, i));
                AttackPositions.Add((-i, -i));
                AttackPositions.Add((i, -i));
                AttackPositions.Add((-i, i));

            }
        }
        if (rollDist == RollDist.Radius)
        {
            for (int i = -dist; i<= dist; i++)
            {
                for (int j = -dist; j <= dist; j++)
                {
                    if (Mathf.Abs(i) + Mathf.Abs(j) <= dist)
                    {
                        AttackPositions.Add((i, j));
                    }
                }
            }
        }
    }

    public virtual List<Roll> GetRolls()
    {
        var ret_list = new List<Roll>();
        foreach (Roll roll in rolls)
        {
            ret_list.Add(new Roll(roll));
        }
        cur_use_count++;
        character.cur_energy -= energy;
        ResoursesDict.GetClass<CharacterTabController>().RequestedUpdate(true);
        return ret_list;
    }
}

public class Roll
{
    public PlayerSkill skill;
    public int radius;
    public int minRoll;
    public int maxRoll;
    public RollType rollType;
    public RollRadius rollRadius;
    public List<(int, int)> DamagePositions = new List<(int, int)>();
    public Element element;

    public int GetRoll()
    {
        int min_plus = 0;
        int max_plus = 0;

        /*
        «ƒ≈—‹ —ƒ≈À¿“‹ œ–Œ¬≈– ” Õ¿ ¡¿‘‘€ »√–Œ ¿ 
        */

        int power = Random.Range(GetMinRoll() + min_plus, GetMaxRoll() + max_plus);

        return power;
    }

    public int GetMaxRoll()
    {
        return maxRoll;
    }

    public int GetMinRoll()
    {
        return minRoll;
    }

    public int GetDamage()
    {
        int min_plus = 0;
        int max_plus = 0;

        /*
        «ƒ≈—‹ —ƒ≈À¿“‹ œ–Œ¬≈– ” Õ¿ ¡¿‘‘€ »√–Œ ¿ 
        */
        int power = Random.Range(GetMinRoll() + min_plus, GetMaxRoll() + max_plus);

        return power;
    }

    public Roll(Roll other)
    {
        minRoll = other.minRoll;
        maxRoll = other.maxRoll;
        rollType = other.rollType;
        rollRadius = other.rollRadius;
        radius = other.radius;
        element = other.element;
        skill = other.skill;
        MakeDamagePositions();
    }

    public Roll()
    {
        minRoll = 1;
        maxRoll = 3;
        rollType = RollType.Atk;
        rollRadius = RollRadius.Single;
        radius = 1;
        element = Element.None;
        skill = null;
        MakeDamagePositions();
    }

    public Roll(int min_roll, int max_roll, RollType type, RollRadius radius_type, int rad, Element element, PlayerSkill skill_ = null)
    {
        minRoll = min_roll;
        maxRoll = max_roll;
        rollType = type;
        rollRadius = radius_type;
        radius = rad;
        this.element = element;
        skill = skill_;
        MakeDamagePositions();
    }

    public void MakeDamagePositions()
    {
        if (rollRadius != RollRadius.Other) DamagePositions.Clear();

        if (rollRadius == RollRadius.Field) DamagePositions.Add((-999, -999));
        if (rollRadius == RollRadius.StLine)
        {
            for (int i = 0; i < radius; i++)
            {
                DamagePositions.Add((0, i));
                DamagePositions.Add((0, -i));
                DamagePositions.Add((i, 0));
                DamagePositions.Add((-i, 0));

            }
        }
        if (rollRadius == RollRadius.DgLine)
        {
            for (int i = 0; i < radius; i++)
            {
                DamagePositions.Add((i, i));
                DamagePositions.Add((-i, -i));
                DamagePositions.Add((i, -i));
                DamagePositions.Add((-i, i));

            }
        }
        if (rollRadius == RollRadius.PlayerRadius || rollRadius == RollRadius.TargetRadius)
        {
            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    if (Mathf.Abs(i) + Mathf.Abs(j) <= radius)
                    {
                        DamagePositions.Add((i, j));
                    }
                }
            }
        }
    }
}

public class Damage
{
    public int damage;
    public Element element;
    public CharacterBase from;

    public Damage(int damage, Element element, CharacterBase from)
    {
        this.damage = damage;
        this.element = element;
        this.from = from;
    }
};
