using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashManager : MonoBehaviour
{

    void Start()
    {
        
    }

    void FixedUpdate()
    {
       foreach (Transform t in transform)
       {
           Destroy(t.gameObject);
       } 
    }
}
