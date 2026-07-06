using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleStone : ObstacleBase
{
    public override CharacterBase Init()
    {
        id = 0;
        hp = 20;
        def = 0;
        speed = 0;
        speed_dif = 0;
        dmg_k = 0f;
        moves = 0;
        energy = 0;
        name = "Обычный камень";
        description = "всё ещё обычный камень";
        ModelId = 0;

        return this;
    }
};

public class CharacterSpawn : ObstacleBase
{
    public override CharacterBase Init()
    {
        id = 1;
        hp = 1;
        def = 0;
        speed = 0;
        speed_dif = 0;
        dmg_k = 0f;
        moves = 0;
        energy = 0;
        name = "Спавнпоинт";
        description = "Здесь появляется персонаж игрока";
        ModelId = 1;

        return this;
    }
};