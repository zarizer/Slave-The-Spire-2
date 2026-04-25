using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GriddableObject : MonoBehaviour
{
    [SerializeField]
    protected GriddableObjectType GType_;

    public GridField Field_;

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
