using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public int x_;
    public int y_;
    public GriddableObject object_;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SnapObject()
    {
        if (object_ == null) return;
        object_.transform.position = transform.position + Vector3.up * 0.45f;
    }
}
