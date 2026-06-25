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
        6,
        12,
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
        rollDist = RollDist.Any;
        rolls.Add(new Roll(
        5,
        10,
        RollType.Atk,
        RollRadius.Single,
        5,
        Element.None));
        energy = -10;
        MakeAttackPositions();
        base.Init();
        return this;
    }
}



public class TestEnemySkill1 : PlayerSkill
{
    public override PlayerSkill Init()
    {
        id = -3;
        name = "TestEnemySkill1";
        dist = 3;
        rollDist = RollDist.Any;
        rolls.Add(new Roll(
        5,
        10,
        RollType.Atk,
        RollRadius.Single,
        5,
        Element.None));
        rolls.Add(new Roll(
        5,
        14,
        RollType.Atk,
        RollRadius.PlayerRadius,
        20,
        Element.dendro));
        energy = -10;
        MakeAttackPositions();
        base.Init();
        return this;

    }
}

public class TestEnemySkill2 : PlayerSkill
{
    public override PlayerSkill Init()
    {
        id = -4;
        name = "TestEnemySkill2";
        dist = 3;
        rollDist = RollDist.StLine;
        rolls.Add(new Roll(
        6,
        12,
        RollType.Atk,
        RollRadius.Single,
        5,
        Element.water));
        energy = -10;
        MakeAttackPositions();
        base.Init();
        return this;
    }
}
public class TestEnemySkill3 : PlayerSkill
{
    public override PlayerSkill Init()
    {
        id = -5;
        name = "TestEnemySkill3";
        dist = 3;
        rollDist = RollDist.StLine;
        rolls.Add(new Roll(
        2,
        4,
        RollType.Def,
        RollRadius.Single,
        5,
        Element.None));
        energy = -10;
        MakeAttackPositions();
        base.Init();
        return this;
    }
}