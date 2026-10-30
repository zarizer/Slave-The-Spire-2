using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ControlData : MonoBehaviour
{
    public float FMovement_speed;
    public float SMovement_speed;
    public float MXMovement_speed;
    public float MYMovement_speed;
    public float CameraSpeed;
    public float CameraSensativity;
    public float CameraSensativityY;
    public float CameraSensativityYLerp;
    public float CameraY;
    public float MaxDist;
    public float MinDist;
    public float CameraXSave;

    public static ControlData instance;

    public void Awake()
    {
        instance = this;
    }
    public static CameraController.Controls GetCameraData()
    {
        CameraController.Controls data = new CameraController.Controls();
        data.FMovement_speed = instance.FMovement_speed;
        data.SMovement_speed = instance.SMovement_speed;
        data.MYMovement_speed = instance.MYMovement_speed;
        data.MXMovement_speed = instance.MXMovement_speed;
        data.CameraSpeed = instance.CameraSpeed;
        data.CameraSensativity = instance.CameraSensativity;
        data.CameraSensativityY = instance.CameraSensativityY;
        data.CameraSensativityYLerp = instance.CameraSensativityYLerp;
        data.CameraY = instance.CameraY;
        data.MaxDist = instance.MaxDist;
        data.MinDist = instance.MinDist;
        data.CameraXSave = instance.CameraXSave;
        return data;
    }
}
