using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBase : CharacterBase
{
    public bool Destroyable = false;
    public bool Interactable = false;
    public int ModelId = 0;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override CharacterBase Init()
    {
        base.Init();
        
        return this;
    }

    public ObstacleBase()
    {

    }
    public ObstacleBase(ObstacleBase other) : base(other)
    {
        this.Destroyable = other.Destroyable;
        this.Interactable = other.Interactable;
        this.ModelId = other.ModelId;
    }

    public virtual new ObstacleBase Clone()
    {
        return new ObstacleBase(this);
    }


}
