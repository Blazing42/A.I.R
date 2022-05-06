using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Room
{
    //name of the room
    [SerializeField] string roomName;
    public string RoomName { get { return roomName; } set { roomName = value; } }

    //type of room, mainly used for the overlay system when creating new levels
    [System.Serializable]
    public enum RoomType { ENGINEERING, CREW, STORAGE, SCIENCE, PLAYER, COMMAND, NONE}
    public RoomType roomType = RoomType.NONE;

    //room id that is used to link all of the tiles in a room to the room itself
    //makes saved files smaller as a reference to the room isnt saved in every tile just an id to be used to look up the room in the roomGrid dictionary
    float roomID;
        public float RoomID
    {
        get { return roomID; }
        private set { roomID = value; }
    }

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

    //public event activated whenever a value is changed
    public event EventHandler onEnvironmentChange;

    public void ValueChangedEventCall()
    {
        //Debug.Log("eventCalled");
        onEnvironmentChange?.Invoke(this, EventArgs.Empty);
    }

    //constructor to create a default room with human surviable values
    public Room( string roomname, RoomType roomType)
    {
        //sets the name of the room
        roomName = roomname;
        //set the room type
        this.roomType = roomType;
        //create a new uniqueID
        roomID = UnityEngine.Random.Range(1, 1000) + Time.realtimeSinceStartup;
    }

    //constructor for the level editor for when the guys are creating new levels to set all of the custom rooms atmospheric values
    public Room(string roomname, RoomType roomType, float temp, float rehumidity, float lightlv, float oxyper, float nitper, float co2per, float pressure)
    {
        roomName = roomname;
        this.roomType = roomType;
        this.temp = temp;
        this.rehumidity = rehumidity;
        this.lightlv = lightlv;
        this.oxyper = oxyper;
        this.nitper = nitper;
        this.co2per = co2per;
        this.pressure = pressure;
        roomID = UnityEngine.Random.Range(1, 1000) + Time.realtimeSinceStartup;
    }

    //empty constructor used for saving and loading
    public Room()
    {}

    //constructor used for testing purposes
    public Room(string roomname, RoomType roomType, float temperature)
    {
        //sets the name of the room
        roomName = roomname;
        //set the room type
        this.roomType = roomType;
        //set the temperature
        temp = temperature;
        //create a new uniqueID
        roomID = UnityEngine.Random.Range(1, 1000) + Time.realtimeSinceStartup;
    }

    //add more functionality to this so that the total always is equal to 100%
    public void SetOxygenPer( float percentageChange)
    {
        oxyper += percentageChange;
    }

    //objects used for saving and loading the data in the tiles for the level editor and once the levels have been created
    [System.Serializable]
    public class SaveObject
    {
        public string roomName;
        public float roomID;
        public Room.RoomType roomType;
        public float temp;
        public float rehumidity;
        public float lightlv;
        public float oxyper;
        public float nitper;
        public float co2per;
        public float pressure;
    }

    public SaveObject Save()
    {
        return new SaveObject
        {
        roomName = roomName,
        roomID = roomID,
        roomType = roomType,
        temp = temp,
        rehumidity = rehumidity,
        lightlv = lightlv,
        oxyper = oxyper,
        nitper = nitper,
        co2per = co2per,
        pressure = pressure,
        };
    }

    public void Load(SaveObject saveObject)
    {
        roomName = saveObject.roomName;
        roomID = saveObject.roomID;
        roomType = saveObject.roomType;
        temp = saveObject.temp;
        rehumidity = saveObject.rehumidity;
        lightlv = saveObject.lightlv;
        oxyper = saveObject.oxyper;
        nitper = saveObject.nitper;
        co2per = saveObject.co2per;
        pressure = saveObject.pressure;
    }
}
