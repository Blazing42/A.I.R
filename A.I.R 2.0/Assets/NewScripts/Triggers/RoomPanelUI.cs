using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script that takes the player input on the room panel and translates it into events and systems that effect the game
/// </summary>
public class RoomPanelUI : MonoBehaviour
{
    public Slider playerInTempSlider;
    public Slider actualTempSlider;
    public Text roomNameText;
    public Toggle lockRoomDoors;
    public Text temperatureText;
    bool doorsLocked;
    public RoomData roomData;
    public Text actualRoomTempText;

    //sets up the room panel each time a new room is clicked on by the player, accessed by the Room Panel controller
    public void SetUpRoomPanel(RoomData roomData)
    {
        this.roomData = roomData;
        roomNameText.text = roomData.name;
        playerInTempSlider.value = roomData.temperature;
        actualTempSlider.value = roomData.temperature;
        doorsLocked = roomData.doorsLocked;
        lockRoomDoors.isOn = roomData.doorsLocked;
        actualRoomTempText.text = roomData.temperature.ToString() + "'c";
        RoomAtmosEventSystem.current.onTemperatureTick += UpdateTemperatureData;
    }

   //methods used to trigger the events caused by the player interacting with the temeprature slider
    //to be used when the player tries to change the temperature in the room
    public void ChangeTemperature()
    {
        //if there is enough energy to change the temperature by this much
        if(EnergySystem.current.EnoughEnergyToChangeTemp(playerInTempSlider.value, actualTempSlider.value) == true)
        {
            RoomAtmosEventSystem.current.SlowlyTickUpOrDownActualTemp(playerInTempSlider.value, actualTempSlider.value, roomData);
            EnergySystem.current.SpendEnergy();
            //Debug.Log(playerInTempSlider.value + " " + actualTempSlider.value);
        }
        else
        {
            playerInTempSlider.value = actualTempSlider.value;
            int intTemp = (int)playerInTempSlider.value;
            temperatureText.text = intTemp.ToString() + "'c";
            //make any kind of not enought energy prompt etc
        }
        
    }

    //updates the temperature text as the player moves the slider up and down 
    public void ChangeTemperatureText(float value)
    {
        int intTemp = (int)value;
        temperatureText.text = intTemp.ToString() + "'c";
    }

    //
    public void UpdateTemperatureData(RoomData room)
    {
        //Debug.Log(room.temperature);
        if (room == roomData)
        {
            actualTempSlider.value = room.temperature;
            actualRoomTempText.text = room.temperature.ToString() + "'c";
        }
        
    }

    //methods used to trigger the events caused by the player interacting with the door trigger
    //to be used by the lock room doors trigger object in the scene
    public void LockRoomDoorTrigger()
    {
        if (!doorsLocked)
        {
            if(EnergySystem.current.EnoughEnergyToLockRoomDoors(roomData) == true)
            {
                DoorEventSystem.current.LockRoomDoors(roomData.doorsToRoomIds);
                EnergySystem.current.SpendEnergy();
                doorsLocked = true;
                UpdateDoorsLockedData(roomData);
            }
        }
        else
        {
            if (EnergySystem.current.EnoughEnergyToLockRoomDoors(roomData) == true)
            {
                DoorEventSystem.current.UnlockRoomDoors(roomData.doorsToRoomIds);
                EnergySystem.current.SpendEnergy();
                doorsLocked = false;
                UpdateDoorsLockedData(roomData);
            }
        }
    } 
    
    public void UpdateDoorsLockedData(RoomData room)
    {
        room.doorsLocked = lockRoomDoors.isOn;
    }

    void OnDestroy()
    {
        RoomAtmosEventSystem.current.onTemperatureTick -= UpdateTemperatureData;
    }
}
