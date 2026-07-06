using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainScript : MonoBehaviour
{
    public Vector3 InitialPos;
    void Start()
    {
        transform.position = InitialPos;
    }


    void Update()
    {
        
    }
}
