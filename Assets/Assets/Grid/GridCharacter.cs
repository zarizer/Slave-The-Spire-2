using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCharacter : GriddableObject
{
    public bool player_;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnEnable()
    {
        GType_ = GriddableObjectType.Character;
    }
}
