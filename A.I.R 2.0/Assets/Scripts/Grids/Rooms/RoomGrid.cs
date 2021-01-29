using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoomGrid 
{
    public Grid<RoomGridObject> roomGrid;
    public List<Room> rooms;
    public event EventHandler OnLoaded;

    public event EventHandler<OnRoomCreatedEvent> OnRoomCreated;
    public class OnRoomCreatedEvent : EventArgs
    {
        public Room newRoom;
    }

    //constructor used to create a new grid full of room grid objects
    public RoomGrid(int width, int height, float cellsize, Vector3 originPosition)
    {
        roomGrid = new Grid<RoomGridObject>(width, height, cellsize, originPosition, true, (Grid<RoomGridObject> roomObjg, int x, int y) => new RoomGridObject(roomObjg, x, y));
        rooms = new List<Room>();
    }



    //create a new room and add it to the list of rooms in this room grid
    public void CreateRoom(List<RoomGridObject> roomGridObjects, string roomName, Room.RoomType roomType)
    {
        //set up the room objects and room so they have references to each other for later player interaction
        Room room = new Room(roomGridObjects, roomName, roomType);
        foreach(RoomGridObject roomObject in roomGridObjects)
        {
            roomObject.SetRoom(room);
        }
        //add the new room to the list
        rooms.Add(room);
        Debug.Log(roomName + " room created");
        OnRoomCreated?.Invoke(this, new OnRoomCreatedEvent {newRoom = room});
    }

    public void ResetRooms()
    {

    }

}
