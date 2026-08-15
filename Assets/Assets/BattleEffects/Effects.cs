using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPoison : BattleEffect
{
    public override void Init()
    {
        image_name = "poison_effect.png"; //важно чтобы было раньше чем base.Init()
        base.Init();
        name = "Отравление";
        description = $"в конце хода отморозок получает дендро урон в размере {power}% от своего текущего здоровья";
    }

    public override void OnTurnEnd(GridField field)
    {
        base.OnTurnEnd(field);
        float one = (float)character.cur_hp / 100;
        float two = one * power;
        int three = (int)two;
        var damage = new Damage(three, Element.dendro, source);
        character.GetDamage(damage);
    }

};

public class EffectBurn : BattleEffect
{
    public override void Init()
    {
        image_name = "fire_effect.png"; //важно чтобы было раньше чем base.Init()
        base.Init();
        name = "Горение";
        description = $"в конце хода отморозок получает пиро урон в размере {power}";
    }

    public override void OnTurnEnd(GridField field)
    {
        base.OnTurnEnd(field);
        var damage = new Damage(power, Element.fire, source);
        character.GetDamage(damage);
    }

};

public class EffectHealProcentBySource : BattleEffect
{
    public override void Init()
    {
        image_name = "HealProcentBySource_effect.png"; //важно чтобы было раньше чем base.Init()
        base.Init();
        name = "Медицинская помощь";
        description = $"В конце хода отморозок восстанавливает {power}% здоровья от максимального здоровья, наложившего эффект";
    }

    public override void OnTurnEnd(GridField field)
    {
        Debug.Log(source);
        base.OnTurnEnd(field);
        float percent = ((float)source.start_hp / 100);
        int amount = (int)(percent * power);
        character.Heal(amount, source);
    }

};

public class EffectPowerDown : BattleEffect
{
    public override void Init()
    {
        image_name = "powe_down_effect.png"; //важно чтобы было раньше чем base.Init()
        base.Init();
        name = "Слабость";
        description = $"Значения роллов понижено на {power}";
    }

};

public class EffectPowerUp : BattleEffect
{
    public override void Init()
    {
        image_name = "powe_up_effect.png"; //важно чтобы было раньше чем base.Init()
        base.Init();
        name = "Сила";
        description = $"Значения роллов повышено на {power}";
    }

};

public class EffectAtkDown : BattleEffect
{
    public override void Init()
    {
        image_name = "atk_down_effect.png"; //важно чтобы было раньше чем base.Init()
        base.Init();
        name = "Тупой клинок";
        description = $"коэффицент урона этого отморозка понижен на {power / 10f}";
    }

    public override void OnApply(GridField field, BattleEffect effect, int ex_power, int ex_duration, bool is_continue)
    {
        base.OnApply(field, effect, ex_power, ex_duration, is_continue);
        character.cur_dmg_k -= ex_power / 10f;

    }

    public override void OnEffectEnd(GridField field, BattleEffect effect)
    {
        base.OnEffectEnd(field, effect);
        character.cur_dmg_k += power/10f;
    }

};

public class EffectAtkUp : BattleEffect
{
    public override void Init()
    {
        image_name = "atk_up_effect.png"; //важно чтобы было раньше чем base.Init()
        base.Init();
        name = "Заточенный клинок";
        description = $"коэффицент урона этого отморозка повышен на {power/10f}";
    }

    public override void OnApply(GridField field, BattleEffect effect, int ex_power, int ex_duration, bool is_continue)
    {
        base.OnApply(field, effect, ex_power, ex_duration, is_continue);
        character.cur_dmg_k += ex_power / 10f;

    }

    public override void OnEffectEnd(GridField field, BattleEffect effect)
    {
        base.OnEffectEnd(field, effect);
        character.cur_dmg_k -= power / 10f;
    }
};

public class EffectSpeedUp : BattleEffect
{
    public override void Init()
    {
        image_name = "speed_up_effect.png"; //важно чтобы было раньше чем base.Init()
        base.Init();
        name = "скорость";
        description = $"В начале хода этот оморозок получает {power} скорости";
    }

    public override void OnTurnStart(GridField field)
    {
        base.OnTurnStart(field);
        character.cur_moves += power;
    }
};

public class EffectSpeedSown : BattleEffect
{
    public override void Init()
    {
        image_name = "speed_down_effect.png"; //важно чтобы было раньше чем base.Init()
        base.Init();
        name = "медлительность";
        description = $"В начале хода отморозок теряет {power} скорости";
        character.cur_moves -= power;
    }

};
