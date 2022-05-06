using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpriteCameraFacingEvent : MonoBehaviour
{
    public static SpriteCameraFacingEvent current;

    private void Awake()
    {
        current = this;
    }

    public event Action onCameraAngleChangeFacing;

    public void CameraAngleChangeFacing()
    {
        if(onCameraAngleChangeFacing != null)
        {
            onCameraAngleChangeFacing();
        }
    }
}
