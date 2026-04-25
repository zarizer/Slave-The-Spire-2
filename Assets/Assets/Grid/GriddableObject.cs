using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GriddableObject : MonoBehaviour
{
    [SerializeField]
    public GriddableObjectType GType_;

    public GridField field_;
    public GridCell cell_;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public enum GriddableObjectType
    {
        Character,
        Obstacle,
        Breakable
    }
}
