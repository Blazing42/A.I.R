using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Room
{
    //the list of tiles that make up the room
    List<RoomGridObject> roomGridObjects;
    //name of the room
    [SerializeField] string roomName;
    public string RoomName { get { return roomName; } set { roomName = value; } }

    public enum RoomType { ENGINEERING, CREW, STORAGE, SCIENCE, PLAYER, COMMAND, NONE}
    public RoomType roomType = RoomType.NONE;

    //all of the room atmosphere variables to be altered by the player as the main mechanic of the game
    //the rooms temperature in degrees celcius, will need to do a conversion to farenheit for people used to the scale in the UI
    float temp = 20;
    public float Temp
    {get { return temp; } set { temp = value; }}
    //the room percentage/relative humidity
    float rehumidity = 50;
    public float ReHumidity
    { get { return rehumidity; } set { rehumidity = Mathf.Clamp(value,0,100); } }
    //the rooms light level
    float lightlv = 70;
    public float Lightlv
    { get { return lightlv; } set { lightlv = value; } }
    //the percentage of oxygen in the air
    float oxyper = 25;
    public float Oxyper
    { get { return oxyper; } set { oxyper = Mathf.Clamp(value, 0, 100); } }
    //the percentage of nitrogen in the air
    float nitper = 70;
    public float Nitper
    { get { return nitper; } set { nitper = Mathf.Clamp(value, 0, 100); } }
    //the percentage of carbon dioxide in the air
    float co2per = 5;
    public float Co2per
    { get { return co2per; } set { co2per = Mathf.Clamp(value, 0, 100); } }
    //the air pressure in bar/atmospheres
    float pressure = 1;
    public float Pressure
    { get { return pressure; } set { pressure = Mathf.Clamp(value, -1, 5); } }


    //constructor to create a default room
    public Room(List<RoomGridObject> roomObjects, string roomname, RoomType roomType)
    {
        //sets the name of the room
        roomName = roomname;
        //sets the tiles that are in the room
        roomGridObjects = roomObjects;
        //makes sure that the tiles have a reference to which room they are in for pathfinding
        this.roomType = roomType;
    }

    //constructor for the level editor for when the guys are creating new levels to set all of the rooms atmospheric values
    public Room(List<RoomGridObject> roomObjects, string roomname, float temp, float rehumidity, float lightlv, float oxyper, float nitper, float co2per, float pressure)
    {
        roomGridObjects = roomObjects;
        roomName = roomname;
        this.temp = temp;
        this.rehumidity = rehumidity;
        this.lightlv = lightlv;
        this.oxyper = oxyper;
        this.nitper = nitper;
        this.co2per = co2per;
        this.pressure = pressure;

    }

    //add more functionality to this so that the total always is equal to 100%
    public void SetOxygenPer( float percentageChange)
    {
        oxyper += percentageChange;
    }
}
