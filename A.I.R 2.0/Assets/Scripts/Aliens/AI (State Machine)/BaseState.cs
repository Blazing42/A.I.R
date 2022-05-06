using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    protected GameObject gameObject;
    protected Transform transform;
    protected Animator animator;
    protected Actor actorScript;
    protected Grid<Tile> currentLv;
    protected float currentRoomID;
    protected bool environmentChanged;
    protected Room currentRoom;

    public BaseState(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.transform = gameObject.transform;
        this.currentLv = LevelSystem.Instance.floorTileMap.tileGrid;
        animator = gameObject.GetComponentInChildren<Animator>();
        actorScript = gameObject.GetComponent<Actor>();
        environmentChanged = false;
        ChangeCurrentRoom();
    }

    public abstract Type Tick();

    public bool RoomChangeCheck()
    {
        //continuously check to see if the tile the creature is on is in the same room as periously
        float nextRoomId = currentLv.GetGridObject(this.transform.position).GetRoomId();
        //if not
        if (currentRoomID != nextRoomId)
        {
            //change the current roomId to the new value before starting the cycle again
            currentRoomID = nextRoomId;
            ChangeCurrentRoom();
            //Debug.Log("Room Changed");
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ChangeCurrentRoom()
    {
        //set the current roomID to the room that the creature is currently standing on if the room hasnt been set yet 
        if (currentRoomID <= 1f)
        {
            currentRoomID = currentLv.GetGridObject(this.transform.position).GetRoomId();
        }
        else
        {
            currentRoom.onEnvironmentChange -= CurrentRoom_onEnvironmentChange;
        }
        currentRoom = actorScript.CurrentRoomDict.GetRoom(currentRoomID);
        currentRoom.onEnvironmentChange += CurrentRoom_onEnvironmentChange;
    }

    private void CurrentRoom_onEnvironmentChange(object sender, EventArgs e)
    {
        environmentChanged = true;
        //Debug.Log(environmentChanged.ToString());
    }

    public AtmosphereUtilities.TempValue RoomTemp()
    {
        //gets a reference to the room that the creature is currently in
        Room newRoom = actorScript.CurrentRoomDict.GetRoom(currentRoomID);
        //check the temperature of the new room
        AtmosphereUtilities.TempValue tempValue = AtmosphereUtilities.CheckTemperature(newRoom.Temp, actorScript.MaxComfTemp, actorScript.MinComfTemp);
        return tempValue;
    }
}
