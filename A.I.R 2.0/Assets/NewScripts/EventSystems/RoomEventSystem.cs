using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoomEventSystem : MonoBehaviour
{
    //singleton pattern to ensure that this event system is easy to access from other classes
    public static RoomEventSystem current;
    void Awake()
    {
        current = this;
    }

    public event Action<RoomData, Vector3> onRoomClickedTrigger;
    public void RoomClickedTrigger(RoomData roomData, Vector3 panelPos)
    {
        if (onRoomClickedTrigger != null)
        {
            onRoomClickedTrigger(roomData, panelPos);
        }
    }

    public event Action onRoomNotClickedTrigger;
    public void RoomNotClickedTrigger()
    {
        if (onRoomNotClickedTrigger != null)
        {
            onRoomNotClickedTrigger();
        }
    }
}
