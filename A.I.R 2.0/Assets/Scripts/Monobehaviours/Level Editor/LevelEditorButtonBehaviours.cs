using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorButtonBehaviours : MonoBehaviour
{
    [SerializeField] GameObject loadButtonPrefab;
    [SerializeField] GameObject loadButtonPanel;
    public GameObject LoadButtonPanel { get { return loadButtonPanel; } }
    [SerializeField] GameObject saveButtonPanel;
    [SerializeField] LevelEditorSystem levelEditorSystem;
    [SerializeField] Text levelEditorStateText;
    [SerializeField] GameObject editRoomsPanel;
    [SerializeField] GameObject newRoomCreationPanel;
    public GameObject NewRoomCreationPanel { get { return newRoomCreationPanel; } }

    void Start()
    {
        levelEditorStateText.text = "Level Editor : Edit Tiles";
    }

    //methods to control the buttons in this scene
    public void SaveButtonPressed()
    {
        saveButtonPanel.SetActive(!saveButtonPanel.activeSelf);
    }

    public void LoadPreviousButtonPressed()
    {
        levelEditorSystem.floorTileMap.LoadMostRecentTileMap();
        Debug.Log("Tilemap Loaded");
    }

    public void LoadButtonPressed()
    {
        InstantiateLoadFileButtons();
        loadButtonPanel.SetActive(!loadButtonPanel.activeSelf);
    }

    public void EditTilemapButtonPressed()
    {
        levelEditorSystem.editorState = LevelEditorSystem.EditorState.TILE_EDITOR;
        levelEditorStateText.text = "Level Editor : Edit Tiles";
        editRoomsPanel.SetActive(false);
    }

    public void EditRoomsButtonPressed()
    {
        levelEditorSystem.editorState = LevelEditorSystem.EditorState.ROOM_EDITOR;
        levelEditorStateText.text = "Level Editor : Edit Rooms";
        editRoomsPanel.SetActive(true);
    }

    public void TestButtonPressed()
    {
        levelEditorSystem.editorState = LevelEditorSystem.EditorState.TEST;
        levelEditorSystem.pathfindingGrid.SetWalkability(levelEditorSystem.floorTileMap.tileGrid);
        levelEditorStateText.text = "Level Editor : Testing";
        editRoomsPanel.SetActive(false);
    }

    public void SaveButton()
    {
        //sort this out when you get the chance
        Transform transform = saveButtonPanel.transform.Find("InputField").transform.Find("Input Text").transform;
        string savefilename = transform.GetComponent<Text>().text;
        levelEditorSystem.floorTileMap.SaveTileMap(savefilename);
        levelEditorSystem.roomGrid.SaveRooms(savefilename);
        saveButtonPanel.SetActive(!saveButtonPanel.activeSelf);
        Debug.Log("Tilemap Saved");
    }
    public void CreateNewRoomButton()
    {
        newRoomCreationPanel.SetActive(!newRoomCreationPanel.activeSelf);
    }

    public void RoomPickerToolButton()
    {
        levelEditorSystem.roomPicker = !levelEditorSystem.roomPicker;
        Debug.Log(levelEditorSystem.roomPicker.ToString());
    }

    public void ResetRoomsButton()
    {
        levelEditorSystem.roomGrid.ResetRooms(levelEditorSystem.floorTileMap.tileGrid);
    }

    public void ResetButton()
    {
        levelEditorSystem.floorTileMap.ResetTileMap();
        levelEditorSystem.roomGrid.ResetRooms(levelEditorSystem.floorTileMap.tileGrid);
    }

    //clean this method up when you have the chance
    void InstantiateLoadFileButtons()
    {
        //a list of filenames is creates from the files in the save folder
        List<string> loadfilenames = SaveSystem.ListFilesToLoad();
        //the transform of the button parent is stored
        Transform transform = loadButtonPanel.transform.Find("Contents").transform;
        //a list of text objects is obtained from the existing contents of the load button panel
        Text[] existingtextobjects = transform.GetComponentsInChildren<Text>();
        List<string> existingfilenames = new List<string>();
        foreach (Text text in existingtextobjects)
        {
            existingfilenames.Add(text.text);
        }

        foreach (string filename in loadfilenames)
        {
            if (existingfilenames.Contains(filename) == false)
            {
                var newLoadButton = Instantiate(loadButtonPrefab, transform);
                newLoadButton.GetComponentInChildren<Text>().text = filename;
                //have a script on it that loads the correct tile based on its name
            }
        }
        //remember to unasign the memory used in this method for the lists and arrays
    }
}
