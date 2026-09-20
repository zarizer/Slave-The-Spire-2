using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSurface : CharacterBase
{

    public int speed_cost = 1;
    public int ModelId = -1;
    void Start()
    {
        
    }

    void Update()
    {
        
    }


    public virtual void OnSet(GridField field_data, ObjectBase source)
    {

    }

    public virtual void OnRemove(GridField field_data, ObjectBase source) 
    { 
      
    } 

    public virtual void OnDealDamage(GridField field_data)
    {

    }
}
