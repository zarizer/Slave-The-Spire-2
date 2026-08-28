using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

public class PassiveVosh1 : Passive
{
    public override void Init()
    {
        base.Init();
        Name = "Мерзость";
        Description = $"Если этому отморозку нанесли урон, нанёсший теряет {Value} скорости";
    }

    public override void OnGetDamage(GridField field, Damage dmg)
    {
        base.OnBattleStart(field);
        if (dmg.from == null) return;
        dmg.from.cur_moves -= (int)Value;
    }

    public override void UpdateAccourdingToLevel()
    {
        base.UpdateAccourdingToLevel();
        Value = level;
    }
};

public class PassiveVosh2 : Passive
{
    public override void Init()
    {
        base.Init();
        Name = "отвратительные брызги";
        Description = $"При смерти этого отморозка, нанёсший последнюю атаку получает {Value} гидро урона";
    }

    public override void OnDeath(GridField field, Damage dmg)
    {
        base.OnBattleStart(field);
        if (dmg.from == null) return;
        dmg.from.GetDamage(new Damage((int)Value, Element.water, character));
    }

    public override void UpdateAccourdingToLevel()
    {
        base.UpdateAccourdingToLevel();
        Value = 3 * level;
    }
};

public class PassiveWitch1 : Passive
{
    public override void Init()
    {
        base.Init();
        Name = "Защита леса";
        Description = $"За каждые 20 энергии на себе, при получении урона, этот отморозок наносит по 3 дендро урона агрессору";
    }

    public override void OnGetDamage(GridField field, Damage dmg)
    {
        base.OnGetDamage(field, dmg);
        if (dmg.from != null)
        {
            Damage damage = new Damage(3 * character.energy, Element.dendro, character);
            dmg.from.GetDamage(damage);
        }
    }

};

public class PassiveWitch2 : Passive
{
    public override void Init()
    {
        base.Init();
        Name = "запас зелий";
        Description = $"в конце хода этот отморозок восстанавливает {Value} здоровья";
    }

    public override void OnTurnEnd(GridField field)
    {
        character.Heal((int)Value, character);
    }

    public override void UpdateAccourdingToLevel()
    {
        base.UpdateAccourdingToLevel();
        Value = 8 * level;
    }
};

public class PassiveCultist1 : Passive
{
    public override void Init()
    {
        base.Init();
        Name = "Связь с культом";
        Description = $"В начале хода, этот отморозок получает {Value} остроты, за каждый(в том числе и вражеский) тотем на поле";
    }

    public override void OnTurnStart(GridField field)
    {
        int p = 0;
        foreach(GridObstacle o in field.GridObstacles)
        {
            if (o.GetCharacter().name == "Оккультный тотем")
            {
                p++;
            }
        }

        if (p == 0) return;

        GridCharacter.ApplyBattleEffect(DataDicts.EffectTypes[1003], p * (int)Value, 0, character, character);
    }

    public override void UpdateAccourdingToLevel()
    {
        base.UpdateAccourdingToLevel();
        Value = 2 * level;
    }
};

public class PassiveCultist2 : Passive
{
    public override void Init()
    {
        base.Init();
        Name = "настоящая порча на понос";
        Description = $"В начале хода случайный вражеский отморозок получает {Value}, за каждые 20 энергии у этого отморозка";
    }

    public override void OnTurnStart(GridField field)
    {
        if (character.object_.GetComponent<GriddableObject>().GType_ == GriddableObject.GriddableObjectType.Enemy)
        {
            for (int i = 0; i< character.energy/20; i++)
            {
                int k = ResoursesDict.GetClass<CameraController>().field_.GridCharacters.Count;
                int j = StaticFuncs.RandomRangeInclusive(0, k - 1);
                GridCharacter.ApplyBattleEffect(DataDicts.EffectTypes[1004], (int)Value, 0,
                    ResoursesDict.GetClass<CameraController>().field_.GridCharacters[j].GetCharacter(), character, true);
            }
        }
        else
        {
            for (int i = 0; i < character.energy / 20; i++)
            {
                int k = ResoursesDict.GetClass<CameraController>().field_.GridEnemies.Count;
                int j = StaticFuncs.RandomRangeInclusive(0, k - 1);
                GridCharacter.ApplyBattleEffect(DataDicts.EffectTypes[1004], (int)Value, 1,
                    ResoursesDict.GetClass<CameraController>().field_.GridEnemies[j].GetCharacter(), character, true);
            }
        }
    }

    public override void UpdateAccourdingToLevel()
    {
        base.UpdateAccourdingToLevel();
        Value = 2 * level;
    }
};

public class PassiveShaman2 : Passive
{
    public override void Init()
    {
        base.Init();
        Name = "Подзакинуться пивом";
        Description = $"При получении урона у этого отморозка есть {Value}% вероятность восстановить 3% здоровья";
    }

    public override void OnGetDamage(GridField field, Damage dmg)
    {
        base.OnGetDamage(field, dmg);

        if (StaticFuncs.RandomRangeInclusive(1, 100) < Value)
        {
            character.Heal(((int)((float)character.start_hp/100)*3), character);
        }
    }

    public override void UpdateAccourdingToLevel()
    { 
        base.UpdateAccourdingToLevel();
        Value = 19 + level * 6;
    }
};

public class PassiveShaman3 : Passive
{
    public override void Init()
    {
        base.Init();
        Name = "Тёмные пивные искусства";
        Description = $"Когда этот отморозок получает энергию, он получает дополнительно ещё 3 энергии и восстанавливает {Value}% здоровья";
    }

    public override void OnGetEnergy(GridField field, int value)
    {
        base.OnGetEnergy(field, value);


        character.Heal(((int)((float)character.start_hp / 100) * value), character);
        character.cur_energy += 3;

    }

    public override void UpdateAccourdingToLevel()
    {
        base.UpdateAccourdingToLevel();
        Value = 0 + level;
    }
};