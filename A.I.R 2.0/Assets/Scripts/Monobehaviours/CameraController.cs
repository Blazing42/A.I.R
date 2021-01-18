using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraController : MonoBehaviour
{
    //variable to control the camera position
    [SerializeField] float camMovSpeed;
    [SerializeField] float camMovTime;
    [SerializeField] float maxXCamPos;
    [SerializeField] float minXCamPos;
    [SerializeField] float maxYCamPos;
    [SerializeField] float minYCamPos;
    Vector3 newPos;

    //variables to control the camera zoom
    [SerializeField] float zoomSpeedMultiplier;
    [SerializeField] float minCamZoom;
    [SerializeField] float maxCamZoom;
    Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        newPos = transform.position;
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleCameraMovImput();
        HandleCameraZoomInput();
    }

    void HandleCameraMovImput()
    {
        //if statements that handle any horizontal and vertical input from controllers to navigate the scene left right up and down
        if (Input.GetAxis("Horizontal") > 0)
        {
            //right
            newPos += (transform.right * camMovSpeed);
        }
        if(Input.GetAxis("Horizontal") < 0)
        {
            //left
            newPos += (transform.right * - camMovSpeed);
        }
        if(Input.GetAxis("Vertical") > 0)
        {
            //up
            newPos += (transform.up * camMovSpeed);
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            //down
            newPos += (transform.up * -camMovSpeed);
        }
        //to make the camera scrolling less jarring this causes the cameras transform to happen over a certain amount of time
        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * camMovTime);

        //clamping the x and y camera positions so the player can loose the tilemap and scroll somewhere off in the distance
        newPos.x = Mathf.Clamp(newPos.x, minXCamPos, maxXCamPos);
        newPos.y = Mathf.Clamp(newPos.y, minYCamPos, maxYCamPos);
    }

    void HandleCameraZoomInput()
    {
        //if statements to handle the camera zoom
        if (Input.mouseScrollDelta.y > 0)
        {
            //zoom out
            camera.orthographicSize -= (Input.GetAxis("Mouse ScrollWheel") * zoomSpeedMultiplier);
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            //zoom in
            camera.orthographicSize -= (Input.GetAxis("Mouse ScrollWheel") * zoomSpeedMultiplier);
        }
        //clamping the zoom value so the player cant zoom in or out to far
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, maxCamZoom, minCamZoom);
    }
}
