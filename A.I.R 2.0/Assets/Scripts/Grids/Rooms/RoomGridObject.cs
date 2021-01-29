using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGridObject
{
    int x;
    int y;
    public Room room;
    Grid<RoomGridObject> roomGrid;

    public RoomGridObject(Grid<RoomGridObject> roomGrid, int x, int y)
    {
        this.x = x;
        this.y = y;
        this.roomGrid = roomGrid;
        room = null;
    }

    public void SetRoom(Room room )
    {
        this.room = room;
        roomGrid.TriggerGridObjectChanged(x, y);
    }
}
