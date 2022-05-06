using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateNewRoomButtonScript : MonoBehaviour
{
    int roomTypeDropdownValue;
    [SerializeField] InputField roomNameInput;
    [SerializeField] Slider tempSlider;
    [SerializeField] Text tempslidervalue;

    void Start()
    {
        roomTypeDropdownValue = 0;
    }

    public void CreateButton()
    {
        //find the level editor system script
        LevelEditorSystem test = GameObject.FindObjectOfType<LevelEditorSystem>();
        //create a new room using the name input by the designer and the type from the dropdown
        test.roomGrid.CreateRoom(roomNameInput.text, (Room.RoomType)roomTypeDropdownValue, tempSlider.value);
        //debug to make sure it works 
        //Debug.Log("new room created " + roomNameInput.text);
        LevelEditorButtonBehaviours buttonBehaviours = GameObject.FindObjectOfType<LevelEditorButtonBehaviours>();
        buttonBehaviours.NewRoomCreationPanel.SetActive(false);
    }

    public void ChangeRoomType(int dropdownValue)
    {
        roomTypeDropdownValue = dropdownValue;
        Debug.Log(dropdownValue.ToString());
        Debug.Log((Room.RoomType)dropdownValue);
    }

    public void TempSliderValue(float temp)
    {
        tempslidervalue.text = temp.ToString();
    }
        
}
