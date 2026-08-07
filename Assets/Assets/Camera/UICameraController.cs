using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICameraController : MonoBehaviour
{
    public Transform MainCamera;
    void Start()
    {
        
    }


    void Update()
    {
        transform.position = MainCamera.position;
        transform.rotation = MainCamera.rotation;
    }
}
