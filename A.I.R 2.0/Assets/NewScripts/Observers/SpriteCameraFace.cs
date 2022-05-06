using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that controls where the sprite is facing, when the Sprite Camera Facing Event is triggered. 
/// </summary>
public class SpriteCameraFace : MonoBehaviour
{
    public Transform CameraRigTransform;
    private Transform SpriteTransform;

    // Start is called before the first frame update
    void Start()
    {
        SpriteCameraFacingEvent.current.onCameraAngleChangeFacing += FaceCamera;
        SpriteTransform = this.transform;
    }
    private void FaceCamera()
    {
        SpriteTransform.forward = CameraRigTransform.forward;
    }

    private void OnDestroy()
    {
        SpriteCameraFacingEvent.current.onCameraAngleChangeFacing -= FaceCamera;
    }
}
   