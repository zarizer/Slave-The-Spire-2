using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSkill1 : PlayerSkill
{
    public override PlayerSkill Init()
    {
        id = -1;
        name = "TestSkill1";
        minRoll = 3;
        maxRoll = 8;
        rollType = RollType.Atk;
        rollDist = RollDist.StLine;
        rollRadius = RollRadius.Single;
        energy = 10;
        return this;
    }
}

public class TestSkill2 : PlayerSkill
{
    public override PlayerSkill Init()
    {
        id = -2;
        name = "TestSkill2";
        minRoll = 5;
        maxRoll = 10;
        rollType = RollType.Atk;
        rollDist = RollDist.Raridus;
        rollRadius = RollRadius.TargetRadius;
        energy = -10;
        return this;
    }
}
