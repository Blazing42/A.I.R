using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class RoomGrid 
{
    public event EventHandler OnLoaded;

    //a dictionary containing all of the rooms using roomID as a reference
    public Dictionary<float, Room> roomsInLv;

    //public event EventHandler OnLoaded;

    //setting up an event that triggers when a new room is created
    public event EventHandler<OnRoomCreatedEvent> OnRoomCreated;
    public class OnRoomCreatedEvent : EventArgs
    {
        public Room newRoom;
    }

    //constructor used to create a new empty dictionary of rooms for level editor
    public RoomGrid()
    {
        roomsInLv = new Dictionary<float, Room>();
    }

    //constructor used to create a room dict from a savefile for testing and play
    public RoomGrid( Dictionary<float, Room> savedDict)
    {
        roomsInLv = savedDict;
    }

    //create a new room and add it to the list of rooms in this room grid/dictionary
    public void CreateRoom(string roomName, Room.RoomType roomType, float temp)
    {
        //create the new room Object
        Room room = new Room(roomName, roomType, temp);
        Debug.Log(roomName + " room created");
        roomsInLv.Add(room.RoomID, room);
        //OnRoomCreated?.Invoke(this, new OnRoomCreatedEvent {newRoom = room});
    }

    //method used to add a tile to the room, used in level editor
    public void AddTileToRoom(Tile tile, float currentRoomID)
    {
        //set the roomID in the tile to be the same as the current roomID
        tile.SetRoomID(currentRoomID);
        
    }

    //method used to remove a tile from the room, used in level editor
    public void RemoveTileFromRoom(Tile tile)
    {
        //set the roomID in the tile to be 0
        tile.SetRoomID(0.1f);
    }

    //method used to reset all of the rooms, used in level editor
    public void ResetRooms(Grid<Tile> tilemapToReset)
    {
        for (int x = 0; x < tilemapToReset.width; x++)
        {
            for (int y = 0; y < tilemapToReset.height; y++)
            {
                Tile currentTile = tilemapToReset.GetGridObject(x, y);
                currentTile.SetRoomID(0.1f);
            }
        }
    }

    //used to access the room, and its  using the roomIDs stored in the tile
    public Room GetRoom(float roomID)
    {
        if (!roomsInLv.TryGetValue(roomID, out Room room))
        {
            return null;
        }
        else
        {
            return room;
        }
            
    }
    //method for saving the rooms to a list
    public void SaveRooms(string filename)
    {
        //create a new empty list of room saveobjects
        List<Room.SaveObject> roomsToSave = new List<Room.SaveObject>();
        //create a list of rooms from the dictionary
        List<Room> roomList = roomsInLv.Values.ToList();
        //convert the rooms in the room list into room saveobjects and add to the saveobject list
        foreach(Room room in roomList)
        {
            Room.SaveObject saveRoom = room.Save();
            roomsToSave.Add(saveRoom);
        }
        //create a new tilemap save object and add the list ot tile saveobjects to the array
        SaveObject saveObject = new SaveObject
        {
            roomGridSaveObjectWithRoomArray = roomsToSave.ToArray(),
        };
        SaveSystem.SaveObject(saveObject, filename + "_rooms", true);
    }
    public void LoadRooms(string filename)
    {
        //get the array of rooms from the save file
        SaveObject saveObject = SaveSystem.LoadObject<SaveObject>(filename + "_rooms");
        //clear the already existing dictionary
        roomsInLv.Clear();
        //add the saved rooms back into the dictionary
        foreach (Room.SaveObject savedRoom in saveObject.roomGridSaveObjectWithRoomArray)
        {
            //set the key to the roomID in the savefile
            float roomKey = savedRoom.roomID;
            //create a new empty room
            Room room = new Room();
            //assigned the saved values to the empty room
            room.Load(savedRoom);
            //add it to the dictionary
            roomsInLv.Add(roomKey, room);
            
        }
        OnLoaded?.Invoke(this, EventArgs.Empty);
    }


    //tilemap save object it is an array because json utilities doesnt work directly with lists
    public class SaveObject
    {
        public Room.SaveObject[] roomGridSaveObjectWithRoomArray ;
    }
}
