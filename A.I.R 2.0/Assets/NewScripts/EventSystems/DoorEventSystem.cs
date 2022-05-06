using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Singleton class that contains the event to open and close the doors, interacts with the door controller script and the door trigger script. 
/// </summary>
public class DoorEventSystem : MonoBehaviour
{
    //singleton pattern to ensure that this event system is easy to access from other classes
    public static DoorEventSystem current;
    void Awake()
    {
        current = this;
    }

    //event that triggers when the doorway trigger is entered, id is assigned in the inspector and is used to ensure only one door moves at once
    //the door controller scripts on the door objects are observing and listening for these events
    public event Action<int> onDoorwayTriggerEnter;
    public void DoorwayTriggerEnter(int id)
    {
        if(onDoorwayTriggerEnter != null)
        {
            onDoorwayTriggerEnter(id);
        }
    }

    //event that triggers when the doorway trigger is exited, id is assigned in the inspector and is used to ensure only one door moves at once
    //the door controller scripts on the door objects are observing and listening for these events
    public event Action<int> onDoorwayTriggerExit;
    public void DoorwayTriggerExit(int id)
    {
        if (onDoorwayTriggerExit != null)
        {
            onDoorwayTriggerExit(id);
        }
    }

    //event triggered when the player clicks the master lock all doors button
    //the door controller scripts on the door objects are observing and listening for these events
    public event Action onMasterLockPressed;
    public void LockAllDoors()
    {
        if(onMasterLockPressed != null)
        {
            onMasterLockPressed();
        }
    }

    //event triggered when the player clicks the master lock all doors button
    //the door controller scripts on the door objects are observing and listening for these events
    public event Action onMasterLockPressedWhenLocked;
    public void UnlockAllDoors()
    {
        if (onMasterLockPressedWhenLocked != null)
        {
            onMasterLockPressedWhenLocked();
        }
    }

    //event triggered when the player clicks the room lock doors button
    //the door controller scripts on the door objects are observing and listening for these events
    public event Action<List<int>> onRoomDoorLockPressed;
    public void LockRoomDoors(List<int> doorIds)
    {
        if(onRoomDoorLockPressed != null)
        {
            onRoomDoorLockPressed(doorIds);
        }
    }

    //event triggered when the player clicks the room lock doors button
    //the door controller scripts on the door objects are observing and listening for these events
    public event Action<List<int>> onRoomDoorUnLockPressed;
    public void UnlockRoomDoors(List<int> doorIds)
    {
        if (onRoomDoorUnLockPressed != null)
        {
            onRoomDoorUnLockPressed(doorIds);
        }
    }
}
