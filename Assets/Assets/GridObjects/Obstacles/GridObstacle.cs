using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObstacle : GriddableObject
{
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

    public override void ReplaceObject(int ID)
    {
        // —ƒ≈À¿“‹ –≈¿À»«¿÷»ﬁ
    }

    public override Roll GetFirstRoll()
    {
        return null;
    }
}
