using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

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

public class PassiveSalty1 : Passive
{
    public override void Init()
    {
        base.Init();
        Name = "В поисках дозы";
        Description = $"Каждые {Value} ходов повышает свою скорость на 1";
    }

    public override void OnTurnStart(GridField field)
    {
        base.OnTurnStart(field);
        character.cur_moves += (field.battleMain.turn / (int)Value);
    }

    public override void UpdateAccourdingToLevel()
    {
        base.UpdateAccourdingToLevel();
        Value = 5 - level;
    }
};

public class PassiveSalty2 : Passive
{
    public override void Init()
    {
        base.Init();
        Name = "Ломка";
        Description = $"Каждый ход повышает коэффицент урона на {Value} и получает {Value*500} физического урона";
    }

    public override void OnTurnStart(GridField field)
    {
        base.OnTurnStart(field);
        character.cur_dmg_k += Value;
        character.GetDamage(new Damage((int)(Value * 500), Element.None, character));
    }

    public override void UpdateAccourdingToLevel()
    {
        base.UpdateAccourdingToLevel();
        Value = 0.02f + 0.01f * level;
    }
};

public class PassiveSalty3 : Passive
{
    public override void Init()
    {
        base.Init();
        Name = "Предвкушение дозы";
        Description = $"При убийстве врага восстанавливает {Value} энергии и здоровья";
    }

    public override void OnKill(GridField field_data, CharacterBase target)
    {
        character.Heal((int)Value, character);
        character.GetEnergy((int)Value);
    }

    public override void UpdateAccourdingToLevel()
    {
        base.UpdateAccourdingToLevel();
        Value = 20 + level * 5;
    }
};

public class PassiveSniper1 : Passive
{
    public override void Init()
    {
        base.Init();
        Name = "Пополнение запасов";
        Description = $"В начале хода получает {Value} энергии за каждого снюсоеда на поле";
    }

    public override void OnTurnStart(GridField field_data)
    {
        int k = 0;
        foreach (var enemy in field_data.GridEnemies)
        {
            if (enemy.GetCharacter().id == 5) { k++; }
        }
        character.GetEnergy((int)Value * k);
    }

    public override void UpdateAccourdingToLevel()
    {
        base.UpdateAccourdingToLevel();
        Value = 3 * level;
    }
};

public class PassiveSniper2 : Passive
{
    public override void Init()
    {
        base.Init();
        Name = "обмакнуть патроны";
        Description = $"При нанесении урона, тратит половину текущей энергии и за каждые 5 потраченной энергии наносит {Value} дополнительных гидро урона";
    }

    public override void OnDealDamage(GridField field, Damage dmg, CharacterBase target)
    {
        base.OnDealDamage(field, dmg, target);
        int k = character.cur_energy / 2;
        target.GetDamage(new Damage((int)Value, Element.water, character), true);
        character.cur_energy -= k;
    }


    public override void UpdateAccourdingToLevel()
    {
        base.UpdateAccourdingToLevel();
        Value = 3 * level;
    }
};

public class PassiveSnusoed1 : Passive
{
    public override void Init()
    {
        base.Init();
        Name = "Прикрытие товарища";
        Description = $"В начале хода получает {Value} скорости, если на поле есть снайпер";
    }

    public override void OnTurnStart(GridField field_data)
    {
        int k = 0;
        foreach (var enemy in field_data.GridEnemies)
        {
            if (enemy.GetCharacter().id == 4) { k=1; }
        }
        character.GetMoves((int)Value * k, character);
    }

    public override void UpdateAccourdingToLevel()
    {
        base.UpdateAccourdingToLevel();
        Value = level;
    }
};

public class PassiveSnusoed2 : Passive
{
    public override void Init()
    {
        base.Init();
        Name = "Раскопать шайбу";
        Description = $"Если у этого отморозка есть 20 или более энергии, в начале хода он восстанавливает {Value} здоровья, тратя эту энергию";
    }

    public override void OnTurnStart(GridField field_data)
    {
        if (character.cur_energy > 20)
        {
            character.cur_energy = 0;
            character.Heal((int)Value, character);
        }
    }

    public override void UpdateAccourdingToLevel()
    {
        base.UpdateAccourdingToLevel();
        Value = 20 * level;
    }
};

public class PassiveUnmovable : Passive
{
    public override void Init()
    {
        base.Init();
        Name = "Громадный";
        Description = $"На этого отморозка не влияют эффекты скорости или замедления";
    }



    public override void UpdateAccourdingToLevel()
    {
        base.UpdateAccourdingToLevel();
        Value = level;
    }
};

public class PassiveElephant2 : Passive
{
    public override void Init()
    {
        base.Init();
        Name = "Испуг";
        Description = $"При получении урона, по возможности отходит на соседнюю клетку";
    }

    public override void OnGetDamage(GridField field, Damage dmg)
    {
        base.OnGetDamage(field, dmg);
        List<GridCell> cells = new List<GridCell>();
        GridCell cur_cell = character.object_.GetComponent<GriddableObject>().cell_;
        cells.Add(field.GetGridCell(cur_cell.x_ + 1, cur_cell.y_));
        cells.Add(field.GetGridCell(cur_cell.x_ - 1, cur_cell.y_));
        cells.Add(field.GetGridCell(cur_cell.x_, cur_cell.y_ - 1));
        cells.Add(field.GetGridCell(cur_cell.x_, cur_cell.y_ + 1));
        foreach (var c in cells)
        {
            if (c != null)
            {
                if (c.object_ == null)
                {
                    character.object_.GetComponent<GriddableObject>().MoveToCell(c);
                }
            }
        }
    }

    public override void UpdateAccourdingToLevel()
    {
        base.UpdateAccourdingToLevel();
        Value = level;
    }
};

public class PassiveDisignCutie1 : Passive
{
    public override void Init()
    {
        base.Init();
        Name = "я поделюсь!";
        Description = $"Когда этот отморозок получает энергию, все остальные отморозки получают {Value} энергии";
    }

    public override void OnGetEnergy(GridField field, int value)
    {
        base.OnGetEnergy(field, value);

        foreach (var c in field.GridCharacters) 
        {
            c.GetCharacter().GetEnergy(value, true);
        }
    }

    public override void UpdateAccourdingToLevel()
    {
        base.UpdateAccourdingToLevel();
        Value = level;
    }
};

public class PassiveDisignCutie2 : Passive
{
    public override void Init()
    {
        base.Init();
        Name = "ну не надо...";
        Description = $"Когда этому отморозку наносит урон противник, нанёсший получает тупой эффект тупой клинок силы {Value} длительности 1";
    }

    public override void OnGetDamage(GridField field, Damage dmg)
    {
        base.OnGetDamage(field, dmg);
        if (dmg.from != null && dmg.from.object_.GetComponent<GriddableObject>().GType_ != GriddableObject.GriddableObjectType.Character)
        {
            if (dmg.damage > 0)
            {
                GridCharacter.ApplyBattleEffect(DataDicts.EffectTypes[1004], (int)Value, 1, dmg.from, character);
            }
        }
    }

    public override void UpdateAccourdingToLevel()
    {
        base.UpdateAccourdingToLevel();
        Value = level;
    }
};

public class PassiveDisignCutie3 : Passive
{
    public override void Init()
    {
        base.Init();
        Name = "Что мы наделали...";
        Description = $"Если этот отморозок погибает, ВСЕ на поле получают медлительность силы {Value}, слабость силы {Value} длительностью 2, топой клинок силы {Value} длительностью 2, а также 20 урона тьмой";
    }

    public override void OnDeath(GridField field, Damage dmg)
    {
        base.OnDeath(field, dmg);
        foreach (var g in field.GetAllObjects())
        {
            
            GridCharacter.ApplyBattleEffect(DataDicts.EffectTypes[1004], (int)Value, 2, dmg.from, character);
            GridCharacter.ApplyBattleEffect(DataDicts.EffectTypes[1002], (int)Value, 2, dmg.from, character);
            GridCharacter.ApplyBattleEffect(DataDicts.EffectTypes[1006], (int)Value, 0, dmg.from, character);
            g.GetCharacter().GetDamage(new Damage(20, Element.darkness, character));
        }
    }

    public override void UpdateAccourdingToLevel()
    {
        base.UpdateAccourdingToLevel();
        Value = level;
    }
};

public class PassiveDrevo1 : Passive
{
    public override void Init()
    {
        base.Init();
        Name = "Тактические указания";
        Description = $"В начале хода все дружественные отморозки получают 1 скорость";
    }

    public override void OnTurnStart(GridField field)
    {
        if (character.object_.GetComponent<GriddableObject>().player_)
        {
            foreach (var c in field.GridCharacters)
            {
                c.GetCharacter().cur_moves++;
            }
        }
        else
        {
            foreach (var c in field.GridEnemies)
            {
                c.GetCharacter().cur_moves++;
            }
        }
    }

    public override void UpdateAccourdingToLevel()
    {
        base.UpdateAccourdingToLevel();
        Value = level;
    }
};

public class PassiveVenomous : Passive
{
    public override void Init()
    {
        base.Init();
        Name = "Ядовитый";
        Description = $"При нанесении урона, накладывает яд силы {Value} длительности 1";
    }

    public override void OnDealDamage(GridField field, Damage dmg, CharacterBase target)
    {
        base.OnDealDamage(field, dmg, target);
        if (dmg.damage > 0)
        {
            GridCharacter.ApplyBattleEffect(DataDicts.EffectTypes[0], (int)Value, 1, target, character);
        }
    }
    public override void UpdateAccourdingToLevel()
    {
        base.UpdateAccourdingToLevel();
        Value = level;
    }
};

public class PassivePython1 : Passive
{
    public override void Init()
    {
        base.Init();
        Name = "Использовать запасы";
        Description = $"Если у этого отморозка в начале хода 75+ энергии, он получает специальный дополнительный сильный кубик атаки и сбрасывает всю энергию";
    }

    public override void OnTurnStart(GridField field)
    {
        base.OnTurnStart(field);
        if (character.cur_energy >= 75) 
        {
            character.cur_energy = 0;
            character.GetExtraRoll(true, 27);
        }
    }
    public override void UpdateAccourdingToLevel()
    {
        base.UpdateAccourdingToLevel();
        Value = level;
    }
};

public class PassivePunishingBird1 : Passive
{
    public bool agressive;
    public override void Init()
    {
        base.Init();
        Name = "Кара";
        Description = $"Запись недоступна";
    }

    public override void OnBattleStart(GridField field)
    {
        base.OnBattleStart(field);
        character.hp = 666;
        character.dmg_k = 6.66f;
        character.init_none_k = 0.66f;
        character.init_darkness_k = 0.66f;
        character.init_light_k = 0.66f;
        character.init_water_k = 0.66f;
        character.init_fire_k = 0.66f;
        character.init_dendro_k = 0.66f;
    }
    public override void OnGetDamage(GridField field, Damage dmg)
    {
        base.OnGetDamage(field, dmg);
        if (dmg.damage > 0) { agressive = true; }
    }

    public override void OnTurnStart(GridField field)
    {
        base.OnTurnStart(field);
        if (agressive)
        {
            character.GetExtraRoll(true, 30);
        }
    }

    public override void UpdateAccourdingToLevel()
    {
        base.UpdateAccourdingToLevel();
        Value = level;
    }
};
