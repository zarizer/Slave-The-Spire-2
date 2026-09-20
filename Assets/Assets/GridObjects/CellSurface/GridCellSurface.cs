using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridCellSurface : GriddableObject
{

    public CellSurface surface_;
    public int SurfaceId_ = -1;
    public Transform Model;
    void Start()
    {
        
    }

    void Update()
    {
        MoveToDestination();
    }

    public override CharacterBase GetCharacter()
    {
        return surface_;
    }

    public static CellSurface GetSurfaceByID(int id)
    {
        Type type = DataDicts.SurafaceTypes[id];
        CellSurface result = (CellSurface)Activator.CreateInstance(type);

        result.Init();
        return result;
    }

    public override void ReplaceObject(int id)
    {
        surface_ = GetSurfaceByID(id);
        var obj = Instantiate(ResoursesDict.SurfaceSet[surface_.ModelId], Model);
        obj.transform.parent = Model;
    }

    void MoveToDestination()
    {
        transform.position = Vector3.Lerp(transform.position, DestinationPosition, 0.1f);
    }
}
