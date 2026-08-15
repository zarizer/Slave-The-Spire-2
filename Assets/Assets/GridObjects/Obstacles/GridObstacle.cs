using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GridObstacle : GriddableObject
{
    ObstacleBase obstacle_;
    public Transform Model;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnEnable()
    {
        GType_ = GriddableObjectType.Obstacle;
    }

    public override void ReplaceObject(int id)
    {
        obstacle_ = GetObstacleByID(id);
        var obj = Instantiate(ResoursesDict.ModelSet[obstacle_.ModelId], Model);
        obj.transform.parent = Model;
    }
    ObstacleBase GetObstacleByID(int id)
    {
        Type type = DataDicts.ObstacleTypes[id];
        ObstacleBase result = (ObstacleBase)Activator.CreateInstance(type);
        result.Init();
        return result;
    }

    public override Roll GetFirstRoll()
    {
        return null;
    }

    public override CharacterBase GetCharacter() { return obstacle_; }
    public override void GetDamage(Damage damage)
    {
        obstacle_.GetDamage(damage);
    }

}
