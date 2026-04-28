using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill
{
    public int id;
    public string name;
    public int minRoll;
    public int maxRoll;
    public RollType rollType;
    public RollDist rollDist;
    public RollRadius rollRadius;
    public CharacterBase character;
    public int energy;

    public int GetRoll()
    {
        int min_plus = 0;
        int max_plus = 0;

        /*
        «ƒ≈—‹ —ƒ≈À¿“‹ œ–Œ¬≈– ” Õ¿ ¡¿‘‘€ »√–Œ ¿ 
        */

        return Random.Range(minRoll + min_plus, maxRoll + max_plus); 
    }

    public PlayerSkill()
    {

    }

    public PlayerSkill(PlayerSkill other)
    {
        id = other.id;
        name = other.name;
        minRoll = other.minRoll;
        maxRoll = other.maxRoll;
        rollType = other.rollType;
        rollDist = other.rollDist;
        rollRadius = other.rollRadius;
        character = other.character;
        energy = other.energy;
    }

    public virtual PlayerSkill Init() { return this; }
}

public enum RollType
{
    Def,
    Atk,
    Evade,
    Effect,
    Other
}

public enum RollDist
{
    StLine,
    DgLine,
    Raridus,
    Any,
    Other
}

public enum RollRadius
{
    Single,
    StLine,
    DgLine,
    PlayerRaridus,
    TargetRadius,
    Field,
    Other
}