using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class that controls the camera movement by taking player input and translating it into camera movement
/// It also pings the sprite camera facing Event System to trigger the sprite changing to face the camera
/// </summary>
public class CameraController : MonoBehaviour
{
    //variables that control the camera movement
    public float normalSpeed;
    public float cameraMoveTime;
    public Vector2 panCameraLimit;
    public Vector2 zoomCameraLimit;
    public float cameraRotationSpeed;
    public float zoomSpeed;

    Vector3 newPosition;
    Quaternion newRotation;
    float cameraMoveSpd;
    Transform cameraTransform;
    Vector3 newZoom;
    bool cameraRotationChanged;

    // Start is called before the first frame update
    void Start()
    {
        newPosition = transform.position;
        cameraMoveSpd = normalSpeed;
        newRotation = transform.rotation;
        cameraTransform = Camera.main.transform;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        HandleCameraPan();
        HandleCameraZoom();
        HandleCameraAngle();
    }

    void HandleCameraPan()
    {
        //moves the camera based on the player keyinputs
        float v = Input.GetAxis("Vertical") * cameraMoveSpd;
        float h = Input.GetAxis("Horizontal") * cameraMoveSpd;
        newPosition += transform.right * h;
        newPosition += transform.forward * v;
        newPosition.x = Mathf.Clamp(newPosition.x, -panCameraLimit.x, panCameraLimit.x);
        newPosition.z = Mathf.Clamp(newPosition.z, -panCameraLimit.y, panCameraLimit.y);
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * cameraMoveTime);
    }

    //change this to a different input based on the console that is being used to play the game
    void HandleCameraZoom()
    {
        float zoom = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        newZoom.y -= zoom;
        newZoom.z += zoom;
        newZoom.y = Mathf.Clamp(newZoom.y, zoomCameraLimit.y, zoomCameraLimit.x);
        newZoom.z = Mathf.Clamp(newZoom.z, -zoomCameraLimit.x, -zoomCameraLimit.y);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * cameraMoveTime);
    }

    //change this to a different input based on the console that is being used to play the game
    void HandleCameraAngle()
    {
        if(Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * cameraRotationSpeed);
            cameraRotationChanged = true;
        }
        if(Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -cameraRotationSpeed);
            cameraRotationChanged = true;
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * cameraMoveTime);
    }

    void LateUpdate()
    {
        if(cameraRotationChanged == true)
        {
            //invoke the script that changes the sprite angle
            SpriteCameraFacingEvent.current.CameraAngleChangeFacing();
        }
        cameraRotationChanged = false;
    }
}
