using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// script that controls the temperature changes in each of the rooms in the level
/// </summary>
public class TemperatureController : MonoBehaviour
{
    public float temperatureTickWaitTime = 1f;
    RoomData roomData;
    RoomAtmosEventSystem atmosevents;
    

    // Start is called before the first frame update
    void Start()
    {
        roomData = this.gameObject.GetComponent<RoomData>();
        Debug.Log(roomData.name);
        RoomAtmosEventSystem.current.onTemperatureValueSuccessfullyChanged += SlowlyIncreaseOrDecreaseTemp;
        atmosevents = RoomAtmosEventSystem.current;
    }

    void SlowlyIncreaseOrDecreaseTemp(float newtemp, float oldTemp, RoomData room)
    {
        if(room == roomData)
        {
            StopAllCoroutines();
            StartCoroutine(TemperatureTick(temperatureTickWaitTime, newtemp, oldTemp, room));
        }
        
    }

    IEnumerator TemperatureTick(float waittime, float newtemp, float oldTemp, RoomData room)
    {
        int inNew = (int)newtemp;
        int inOld = (int)oldTemp;
        int tempDifference = inNew - inOld;

        Debug.Log("TemperatureTick started");

        if (inOld < inNew)
        {
            //audioSystem.PlaySoundEffect(tempRisingSFX);
            for (int i = 0; i < tempDifference; i++)
            { 
                room.temperature += 1;
                //trigger the temperature tick event, this event will trigger the GOAP AI world state system to change and the temperature slider UI to change
                yield return new WaitForSeconds(waittime);
                RoomAtmosEventSystem.current.TemperatureTick(room);
            }
        }
        else if (oldTemp > newtemp)
        {
            //audioSystem.PlaySoundEffect(tempFallingSFX);
            for (int i = 0; i > tempDifference; i--)
            {
                room.temperature -= 1;
                //trigger the temperature tick event, this event will trigger the GOAP AI world state system to change and the temperature slider UI to change
                yield return new WaitForSeconds(waittime);
                RoomAtmosEventSystem.current.TemperatureTick(room);
            }
        }
        //make sure to add interesting animations to anything that you click on so that if people are stuck for what to do they can change stuff
    }

    void OnDestroy()
    {
        RoomAtmosEventSystem.current.onTemperatureValueSuccessfullyChanged -= SlowlyIncreaseOrDecreaseTemp;
    }
}
