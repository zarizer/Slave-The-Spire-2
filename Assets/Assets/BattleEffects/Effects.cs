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
        character.GetDamage(new Damage((int)(((float)character.cur_hp) / 100 * power), Element.dendro, source));
    }

};
