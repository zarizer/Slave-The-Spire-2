using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class BattleBuff
{
    public string Name;
    public string Description;
    public float Value;
    public bool OneTime = false;

    public BattleBuff()
    {
        Name = "deffault";
        Description = "deffault";
        Value = 0;
        OneTime = false;

    }
    public virtual void MakeBuff(CharacterBase character) { }

    public virtual void Init() { }

    public static BattleBuff GetBaffInstance(Type type)
    {
        var baff = (BattleBuff)Activator.CreateInstance(type);
        baff.Init();
        return baff;
    }
}

public class HPBuff : BattleBuff
{
    public override void Init()
    {
        base.Init();
        Value = 1.1f;
        Name = "Здоровье";
        Description = "увеличивает здоровье отморозков на 10%";
    }
    public override void MakeBuff(CharacterBase character) 
    {
        character.cur_hp = (int)(character.cur_hp * Value);
        character.hp = (int)(character.hp * Value);
    }
}

public class DefBuff : BattleBuff
{
    public override void Init()
    {
        base.Init();
        Value = 15;
        Name = "Защита";
        Description = "Отморозки получают 15 защиты";
    }
    public override void MakeBuff(CharacterBase character)
    {
        character.cur_def = (int)(character.cur_def + Value);
    }
}

public class StrBuff : BattleBuff
{
    public override void Init()
    {
        base.Init();
        Value = 0.1f;
        Name = "Сила";
        Description = "Увеличивание коэффицент урона отморозков на 0.1";
    }
    public override void MakeBuff(CharacterBase character)
    {
        character.cur_dmg_k = (float)(character.cur_dmg_k + Value);
    }
}

public class ElementDefBuff : BattleBuff
{
    public override void Init()
    {
        base.Init();
        //OneTime = true;
        Value = 0.1f;
        Name = "Сопротивление";
        Description = "Увеличивание сопротивление отморозков случайному виду урона на 0.1";
    }
    public override void MakeBuff(CharacterBase character)
    {
        int rand = UnityEngine.Random.Range(0, 7);
        switch (rand)
        {
            case 0:
                character.none_k = character.none_k - Value;
                break;
            case 1:
                character.fire_k = character.fire_k - Value;
                break;
            case 2:
                character.water_k = character.water_k - Value;
                break;
            case 3:
                character.dendro_k = character.dendro_k - Value;
                break;
            case 4:
                character.light_k = character.light_k - Value;
                break;
            case 5:
                character.darkness_k = character.darkness_k - Value;
                break;
            default:
                break;
        }
    }
}

public class SpeedBuff : BattleBuff
{
    public override void Init()
    {
        base.Init();
        Value = 1f;
        Name = "Скорость";
        Description = "С вероятностью 40% увеличивает скорость отморозка на 1";
    }
    public override void MakeBuff(CharacterBase character)
    {
        if (UnityEngine.Random.Range(0,10) < 4) character.cur_moves = (int)(character.cur_moves + Value);
    }
}