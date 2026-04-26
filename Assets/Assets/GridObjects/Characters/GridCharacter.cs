using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCharacter : GriddableObject
{
    public bool player_;

    public GameObject TexturePlane;
    void Start()
    {
        
    }

    void Update()
    {
        LookAtCamera();
    }

    private void OnEnable()
    {
        GType_ = GriddableObjectType.Character;
    }

    void LookAtCamera()
    {
        TexturePlane.transform.LookAt(field_.Camera.Camera);
        TexturePlane.transform.localEulerAngles = new Vector3(0,
                                                              TexturePlane.transform.localEulerAngles.y,
                                                              TexturePlane.transform.localEulerAngles.z);
    }
}
