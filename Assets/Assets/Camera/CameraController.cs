using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Camera;
    public Transform Center;
    public Transform CameraDestination;
    public Transform Target;
    public Transform CameraBack;
    public bool IsTargeted;
    public float XMovement;
    public float ZMovement;
    public float CameraSpeed = 0.1f;
    public float CameraSensativity;
    public float CameraY;
    public float MaxDist;
    public float MinDist;
    void Start()
    {
        
    }

    void Update()
    {
        XMovement = Input.GetAxis("Horizontal") * CameraSensativity;
        ZMovement = Input.GetAxis("Mouse ScrollWheel") * CameraSensativity;
        
        CameraReposition();
    }

    void CameraReposition()
    {
        transform.position = Vector3.Lerp(transform.position, Target.position, CameraSpeed);
        DestinationReposition();
        DestinationRepositionZ();
        Camera.position = Vector3.Lerp(Camera.position, CameraDestination.position, CameraSpeed);
        Camera.rotation = Quaternion.Lerp(Camera.rotation, CameraDestination.rotation, CameraSpeed);
    }

    void DestinationReposition()
    {
        CameraDestination.RotateAround(Center.position, Vector3.up, XMovement);
        CameraBack.RotateAround(Center.position, Vector3.up, XMovement);
        //CameraDestination.position = new Vector3(CameraDestination.position.x, CameraY, CameraDestination.position.z);
        CameraDestination.LookAt(Center);
    }

    void DestinationRepositionZ()
    {
        if (ZMovement < 0 && Vector3.Distance(CameraDestination.position, Center.position) > MinDist)
        {
            CameraDestination.position = Vector3.Lerp(CameraDestination.position, Center.position, CameraSpeed*0.1f);
            Debug.Log("in");
        }
        if (ZMovement > 0)
        {
            CameraDestination.position = Vector3.Lerp(CameraDestination.position, CameraBack.position, CameraSpeed*0.1f);
            Debug.Log("out");
        }
    }
}
