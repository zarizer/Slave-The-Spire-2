using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTheCamera : MonoBehaviour
{
    public bool enable = true;

    void FixedUpdate()
    {
        if (!enable) return;
        transform.LookAt(Camera.main.transform.position);
    }
}
