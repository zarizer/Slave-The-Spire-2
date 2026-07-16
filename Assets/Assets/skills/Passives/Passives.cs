using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class PassiveMain1 : Passive
{
    public override void Init()
    {
        base.Init();
        Name = "Лидерство";
        Description = $"Дружественные отморозки получают {(int)Value} уровней";
    }

    public override void OnBattleStart(GridField field)
    {
        base.OnBattleStart(field);
        foreach (var obj in field.GridCharacters)
        {
            obj.GetCharacter().level += (int)Value;
        }
    }

    public override void UpdateAccourdingToLevel()
    {
        base.UpdateAccourdingToLevel();
        Value = level * 2;
    }
};

public class PassiveMain2 : Passive
{
    public override void Init()
    {
        base.Init();
        Name = "Запал";
        Description = $"Этот отморозок с вероятностью {Value}% получает 1 скорость в начале хода";
    }

    public override void OnBattleStart(GridField field)
    {
        base.OnBattleStart(field);
        if (StaticFuncs.RandomRangeInclusive(1, 100) <= Value)
        {
            character.cur_moves++;
        }
    }

    public override void UpdateAccourdingToLevel()
    {
        base.UpdateAccourdingToLevel();
        Value = 20 + level * 10;
    }
};

public class PassiveMain3 : Passive
{
    public override void Init()
    {
        base.Init();
        Name = "Бравое дело";
        Description = $"Когда этот отморозок наносит урон светом, он наносит дополнительно {Value} истинного урона";
    }

    public override void OnDealDamage(GridField field, Damage dmg, CharacterBase target)
    {
        base.OnBattleStart(field);
        if (dmg.element == Element.light)
        {
            target.GetDamage(new Damage((int)Value, Element.True, character));
        }
    }

    public override void UpdateAccourdingToLevel()
    {
        base.UpdateAccourdingToLevel();
        Value = level * 4;
    }
};