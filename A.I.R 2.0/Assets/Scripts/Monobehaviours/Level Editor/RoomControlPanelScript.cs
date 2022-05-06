using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RoomControlPanelScript : MonoBehaviour
{
    [SerializeField] Slider temperatureSlider;
    [SerializeField] Slider actualTemperaturSlider;
    [SerializeField] Text temperatureText;
    [SerializeField] Text actualTemperatureText;
    [SerializeField] Slider pressureSlider;
    [SerializeField] Slider relativeHumiditySlider;
    [SerializeField] Text roomNameText;
    LevelSystem levelSystem;
    RoomGrid roomGrid;
    Room selectedRoom;
    bool crRunninginRoom = false;
    public float roomID;
    List<GameObject> children;
    AudioSystem audioSystem;
    [SerializeField] AudioClip tempRisingSFX;
    [SerializeField] AudioClip tempFallingSFX;

    //method to set up the panel when a new room is clicked on
    public void SetupPanel(float roomID)
    {
        audioSystem = AudioSystem.Instance;
        children = new List<GameObject>();
        AddDescendants(this.gameObject.transform, children);
        foreach(GameObject child in children)
        {
            Debug.Log(child.name);
        }
        //get a reference to the current level system
        levelSystem = LevelSystem.Instance;
        //use this reference to get the roomDictionary attached to this level
        roomGrid = levelSystem.roomDict;
        //use the roomID to get the room that was selected
        this.roomID = roomID;
        selectedRoom = roomGrid.GetRoom(roomID);
        //set the room name text box to display the name of the room that has been selected
        roomNameText.text = selectedRoom.RoomName;
        //set up the temperature slider so that it displays the current room temperature
        temperatureSlider.value = selectedRoom.Temp;
        actualTemperaturSlider.value = selectedRoom.Temp;
        int intTemp = (int)selectedRoom.Temp;
        temperatureText.text = intTemp.ToString();
        actualTemperatureText.text = "Temperature : " + (int)selectedRoom.Temp;
        //set up the pressure slider so that it displays the current room pressure
        pressureSlider.value = selectedRoom.Pressure;
        //set up the relative humidity slider so that it displaye the current room relative humidity
        relativeHumiditySlider.value = selectedRoom.ReHumidity;
        Debug.Log(selectedRoom.RoomName);
        //ChangeTemperature();
    }

    public void ChangeTemperature()
    {
        if(crRunninginRoom == true)
        {
            StopAllCoroutines();
        }
        //Debug.Log("TemperatureChanged");
        StartCoroutine(TemperatureTick(0.2f, selectedRoom));
    }

    public void ChangeTemperatureText(float value)
    {
        int intTemp = (int)value;
        temperatureText.text = intTemp.ToString();
    }

    public void MakeInvisible()
    {
        GetComponent<Image>().enabled = false;
        foreach(GameObject child in children)
        {
            child.SetActive(false);
        }
        
    }

    public void MakeVisable()
    {
        GetComponent<Image>().enabled = true;
        foreach (GameObject child in children)
        {
            child.SetActive(true);
        }
    }

    private void AddDescendants(Transform parent, List<GameObject> list)
    {
        foreach (Transform child in parent)
        {
            list.Add(child.gameObject);
            AddDescendants(child, list);
        }
    }


    IEnumerator TemperatureTick(float waittime, Room room)
    {
       crRunninginRoom = true;
       float targetTemperature = temperatureSlider.value; 
       float tempDifference =  targetTemperature - room.Temp;
        //Debug.Log(tempDifference);
        if (room.Temp < targetTemperature)
        {
            audioSystem.PlaySoundEffect(tempRisingSFX);
            for (int i = 0; i < tempDifference; i++)
            {

                //trigger the environment changed event
                room.Temp += 1;
                actualTemperaturSlider.value = room.Temp;
                actualTemperatureText.text = "Temperature : " + (int)room.Temp;
                room.ValueChangedEventCall();
                //event will cause aliens to react
                yield return new WaitForSeconds(waittime);

            }
        }
        else if (room.Temp > targetTemperature)
        {
            audioSystem.PlaySoundEffect(tempFallingSFX);
            for (int i = 0; i > tempDifference; i--)
            {
                
                room.Temp -= 1;
                actualTemperaturSlider.value = room.Temp;
                actualTemperatureText.text = "Temperature : " + (int)room.Temp;
                room.ValueChangedEventCall();
                
                //event will cause aliens to react
                yield return new WaitForSeconds(waittime);
            }
        }
        crRunninginRoom = false;
            //make sure to add interesting animations to anything that you click on so that if people are stuck for what to do they can change stuff
    }
}

    //in a seperate script probably a UI controller script create a method that instantiates this panel near to the place that was clicked, and sets up the value
    //if one of these panels already exists have it change transform to the new position and have the valuse change to those of the new room that was clicked on, so as to not clutter the screen and to increase frame rate.
