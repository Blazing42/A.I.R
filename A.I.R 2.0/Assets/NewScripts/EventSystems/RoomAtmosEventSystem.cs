using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoomAtmosEventSystem : MonoBehaviour
{
    //singleton pattern to ensure that this event system is easy to access from other classes
    public static RoomAtmosEventSystem current;
    void Awake()
    {
        current = this;
    }

    //events that are triggeres when the player changes the temperature
    public event Action<float, float, RoomData> onTemperatureValueSuccessfullyChanged;
    public void SlowlyTickUpOrDownActualTemp(float newTemp, float oldTemp, RoomData roomData)
    {
        if (onTemperatureValueSuccessfullyChanged != null)
        {
            onTemperatureValueSuccessfullyChanged(newTemp, oldTemp, roomData);
        }
    }

    //event that is triggered every time the temperature slowly ticks up or down in each room
    public event Action<RoomData> onTemperatureTick;
    public void TemperatureTick(RoomData roomData)
    {
        if(onTemperatureTick != null)
        {
            onTemperatureTick(roomData);
        }
    }

    //event that is triggered when the player changes the room pressure
    public event Action<float, float, RoomData> onPressureValueSuccessfullyChanged;
    public void SlowlyTickUpOrDownActualPressure(float newPressure, float oldPressure, RoomData roomData)
    {
        if (onPressureValueSuccessfullyChanged != null)
        {
            onPressureValueSuccessfullyChanged(newPressure, oldPressure, roomData);
        }
    }

    //event that is triggered every time the temperature slowly ticks up or down in each room
    public event Action<RoomData> onPressureTick;
    public void PressureTick(RoomData roomData)
    {
        if (onPressureTick != null)
        {
            onPressureTick(roomData);
        }
    }
}
