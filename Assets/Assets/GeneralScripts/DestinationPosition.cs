using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestinationPosition : MonoBehaviour
{
    public bool is_2D = true;
    public bool is_locked = true;
    public bool is_local = false;
    public float speed = 0.2f;
    public Vector3 DestinationPos;

    void Start()
    {
    }

    void Update()
    {
        if (is_locked)
        {
            Vector3 move_pos = DestinationPos;
            if (is_2D) move_pos.z = transform.position.z;
            if (is_local) transform.localPosition = Vector3.Lerp(transform.position, move_pos, speed);
            else transform.position = Vector3.Lerp(transform.position, move_pos, speed);
        }
    }

    public void SetDestinationPosition(Vector3 destination)
    {
        DestinationPos = destination;
    }

    public void SetDestinationPosition(float x, float y, float z)
    {
        DestinationPos = new Vector3(x, y, z);
    }

    public float GetCurX()
    {
        return DestinationPos.x;
    }
    public float GetCurY()
    {
        return DestinationPos.y;
    }
    public float GetCurZ()
    {
        return DestinationPos.z;
    }
}
