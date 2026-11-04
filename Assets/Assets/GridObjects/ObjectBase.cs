using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBase
{
    public GameObject object_;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public GridCharacter GetGridCharacer()
    {
        if (object_ != null && object_.GetComponent<GridCharacter>() != null)
        {
            return object_.GetComponent<GridCharacter>();
        }
        return null;
    }
}
