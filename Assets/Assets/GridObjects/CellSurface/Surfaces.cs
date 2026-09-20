using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurfaceBasic : CellSurface
{
    public override CharacterBase Init()
    {
        id = 0;
        name = "пусто";
        description = "Ѕез приколов";
        base.Init();
        return this;
    }
}
public class SurfaceSwamp : CellSurface
{
    public override CharacterBase Init()
    {
        id = 1;
        ModelId = 0;
        name = "болото";
        description = "передвижение по этой клетке тратит 2 скорости вместо 1";
        speed_cost = 2;
        base.Init();
        return this;
    }

    public override void OnSpawn(GridField field_data)
    {
        base.OnSpawn(field_data);
        Debug.Log(object_);
        Debug.Log(object_.transform.Find("Model").GetChild(0).Find("Canvas"));
        object_.transform.Find("Model").GetChild(0).Find("Canvas").GetChild(0).GetComponent<RawImage>().texture =
            StaticFuncs.GenerateTextureByNoise(0, 100, 100, (Time.time* StaticFuncs.RandomRangeInclusive(0, 99999)) % 98344, Time.time % 824378, 6);

    }
}
