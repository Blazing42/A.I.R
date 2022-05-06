using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class PlayerInputHandler : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //methods that spawn the room editor UI
            if (Input.GetMouseButtonDown(0))
            {
                //var canvas = GameObject.Find("Canvas");
                //get a position to spawn the panel
                Vector3 panelPosition = Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000f))
                {
                    if (hit.collider.tag == "room")
                    {
                        RoomData clickedRoomData = hit.transform.gameObject.GetComponent<RoomData>();
                        RoomEventSystem.current.RoomClickedTrigger(clickedRoomData, panelPosition);
                    }

                }
                else
                {
                    RoomEventSystem.current.RoomNotClickedTrigger();
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                RoomEventSystem.current.RoomNotClickedTrigger();
            }
        }
    }
}
