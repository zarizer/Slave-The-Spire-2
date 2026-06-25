using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerSkill
{
    public int id;
    public string name;
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
        element = other.element;
        skill = other.skill;
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

