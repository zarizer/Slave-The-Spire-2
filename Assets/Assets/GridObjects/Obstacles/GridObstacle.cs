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
        ObstacleBase ret_obstacle = null;


        ret_obstacle = DataDicts.ObstacleSet[id].Clone();

        Debug.Log("Got Obstacle: " + ret_obstacle.name + " id: " + id);

        if (ret_obstacle == null) ret_obstacle = new SimpleStone();

        ret_obstacle.Init();
        return ret_obstacle;
    }

    public override Roll GetFirstRoll()
    {
        return null;
    }

    public override CharacterBase GetCharacter() { return obstacle_; }
}
