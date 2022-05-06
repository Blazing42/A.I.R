using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that controls the spawning and despawning of the room panels when the player clicks on various areas of the level
/// </summary>
public class RoomPanelController : MonoBehaviour
{
    public GameObject roomEditorPanelPrefab;
    public Canvas canvas;
    bool roomPanelForRoomExists;
    GameObject roomEditorPanel;
    List<GameObject> roomPanels;

    // Start is called before the first frame update
    void Start()
    {
        RoomEventSystem.current.onRoomClickedTrigger += SpawnRoomPanel;
        RoomEventSystem.current.onRoomNotClickedTrigger += DestroyRoomPanel;
        roomPanels = new List<GameObject>();
    }

    void SpawnRoomPanel(RoomData room, Vector3 panelPos)
    {
        if (roomPanels.Count < 1)
        {
            roomEditorPanel = Instantiate(roomEditorPanelPrefab, new Vector3(panelPos.x + 120f, panelPos.y + 30f), Quaternion.identity, canvas.transform);
            //set the value to match the room that was clicked on
            roomEditorPanel.GetComponent<RoomPanelUI>().SetUpRoomPanel(room);
            roomEditorPanel.GetComponent<RoomPanelVisibilityController>().SetUpVisability();
            roomPanels.Add(roomEditorPanel);
        }
        else
        {
            foreach (GameObject roomEditorPanel in roomPanels)
            {
                roomEditorPanel.GetComponent<RoomPanelVisibilityController>().MakeInvisible();
            }
            roomPanelForRoomExists = false;

            foreach (GameObject roomEditorPanel in roomPanels)
            {
                var roomIDCheck = roomEditorPanel.GetComponent<RoomPanelUI>().roomData;
                if (room == roomIDCheck)
                {
                    //if there is move its position
                    roomEditorPanel.transform.position = new Vector3(panelPos.x + 120f, panelPos.y + 30f);
                    //roomEditorPanel.GetComponent<RoomControlPanelScript>().SetupPanel(currentTile.GetRoomId());
                    //then set it active in the scene
                    roomEditorPanel.GetComponent<RoomPanelVisibilityController>().MakeVisable();
                    roomPanelForRoomExists = true;
                }
            }
            if (roomPanelForRoomExists == false)
            {
                roomEditorPanel = Instantiate(roomEditorPanelPrefab, new Vector3(panelPos.x + 120f, panelPos.y + 30f), Quaternion.identity, canvas.transform);
                //set the value to match the room that was clicked on
                roomEditorPanel.GetComponent<RoomPanelUI>().SetUpRoomPanel(room);
                roomEditorPanel.GetComponent<RoomPanelVisibilityController>().SetUpVisability();
                roomPanels.Add(roomEditorPanel);
            }
        }
        /*Debug.Log("spawnRoomPanel");
        if (!roomPanelActive)
        {
            roomEditorPanel = Instantiate(roomEditorPanelPrefab, new Vector3(panelPos.x + 120f, panelPos.y + 30f), Quaternion.identity, canvas.transform);
            //set the value to match the room that was clicked on
            roomEditorPanel.GetComponent<RoomPanelUI>().SetUpRoomPanel(roomData);
            roomPanelActive = true;
        }
        else
        {
            roomEditorPanel.transform.position = new Vector3(panelPos.x + 120f, panelPos.y + 30f);
            //set the value to match the room that was clicked on
            roomEditorPanel.GetComponent<RoomPanelUI>().SetUpRoomPanel(roomData);
            
        }*/
    }

    void DestroyRoomPanel()
    {
        foreach (GameObject roomEditorPanel in roomPanels)
        {
            roomEditorPanel.GetComponent<RoomPanelVisibilityController>().MakeInvisible();
        }
    }
}
