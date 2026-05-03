using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSkill1 : PlayerSkill
{
    public override PlayerSkill Init()
    {
        id = -1;
        name = "TestSkill1";
        rollDist = RollDist.Radius;
        dist = 2;
        rolls.Add(new Roll(
        4,
        8,
        RollType.Atk,
        RollRadius.Single,
        1,
        Element.fire));
        energy = -10;
        MakeAttackPositions();
        base.Init();
        return this;
    }
}

public class TestSkill2 : PlayerSkill
{
    public override PlayerSkill Init()
    {
        id = -2;
        name = "TestSkill2";
        dist = 3;
        rollDist = RollDist.DgLine;
        rolls.Add(new Roll(
        5,
        10,
        RollType.Atk,
        RollRadius.DgLine,
        5,
        Element.None));
        energy = -10;
        MakeAttackPositions();
        base.Init();
        return this;
    }
}
