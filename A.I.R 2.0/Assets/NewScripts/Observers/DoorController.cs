using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An observer class subscribed to the DoorEventSystem triggered event, it controls the movement of the doors 
/// It also contains methods that interact with the GOAP AI system and the Player Room Change System in the form of locking and unlocking the door 
/// </summary>
public class DoorController : MonoBehaviour
{
    public int doorId;
    bool isOpen = false;
    bool isLocked = false;
    GameObject leftdoor;
    GameObject rightdoor;

    // Start is called before the first frame update
    void Start()
    {
        DoorEventSystem.current.onDoorwayTriggerEnter += OpenDoorway;
        DoorEventSystem.current.onDoorwayTriggerExit += CloseDoorway;
        DoorEventSystem.current.onMasterLockPressed += LockDoor;
        DoorEventSystem.current.onMasterLockPressedWhenLocked += UnLockDoor;
        DoorEventSystem.current.onRoomDoorLockPressed += LockIfInRoom;
        DoorEventSystem.current.onRoomDoorUnLockPressed += UnLockIfInRoom;

        leftdoor = this.transform.GetChild(0).gameObject;
        rightdoor = this.transform.GetChild(1).gameObject;
    }

    private void OpenDoorway(int id)
    {
        if(id == doorId)
        {
            if (!isLocked)
            {
                //open the door
                LeanTween.moveLocalZ(leftdoor, -6.5f, .6f).setEaseInQuad();
                LeanTween.moveLocalZ(rightdoor, 4.5f, .6f).setEaseInQuad();
                isOpen = true;
            }
        }
        
    }

    private void CloseDoorway(int id)
    {
        if (id == doorId)
        {
            if (isOpen)
            {
                //close the door
                LeanTween.moveLocalZ(leftdoor, -3f, 0.6f).setEaseOutQuad();
                LeanTween.moveLocalZ(rightdoor, 1f, 0.6f).setEaseOutQuad();
                isOpen = false;
            }
        }
    }

    private void LockIfInRoom(List<int> doorids)
    {
        foreach (int doorId in doorids)
        {
            if(this.doorId == doorId)
            {
                isLocked = true;
            }
        }
    }

    private void UnLockIfInRoom(List<int> doorids)
    {
        foreach (int doorId in doorids)
        {
            if (this.doorId == doorId)
            {
                isLocked = false;
            }
        }
    }

    //to be used with the AI system and the emergency system
    public void LockDoor()
    {
        if (isOpen)
            {
                CloseDoorway(doorId);
            }
        isLocked = true;
    }
    public void UnLockDoor()
    {
        isLocked = false;
    }

    //use if hacked/ broken in to by the aliens
    public void UnlockandOpenDoor()
    {
        isLocked = false;
        if (!isOpen)
        {
            OpenDoorway(doorId);
        }
    }

    void onDestroy()
    {
        DoorEventSystem.current.onDoorwayTriggerEnter -= OpenDoorway;
        DoorEventSystem.current.onDoorwayTriggerExit -= CloseDoorway;
        DoorEventSystem.current.onMasterLockPressed -= LockDoor;
        DoorEventSystem.current.onMasterLockPressedWhenLocked -= UnLockDoor;
        DoorEventSystem.current.onRoomDoorLockPressed -= LockIfInRoom;
        DoorEventSystem.current.onRoomDoorUnLockPressed -= UnLockIfInRoom;
    }
}
