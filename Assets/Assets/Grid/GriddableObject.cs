using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GriddableObject : MonoBehaviour
{
    public bool player_;
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
        Enemy,
        Character,
        Obstacle,
        Breakable
    }
}
