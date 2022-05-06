using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class LevelEditorSystem: MonoBehaviour
{
    //allows designer to set the size of the grid they want to work with when creating the levels in the editor
    [SerializeField] int gridSize;
    [SerializeField] int cellSize;

    //create grid variable to store the tilemap 
    //eventually we will create an array of grids to store all of the floor tilemaps for this game level
    public FloorTileMap floorTileMap;
    //reference to the object that will be updating the tilemap visuals
    [SerializeField] private TileMapVisuals visuals;

    //testing out pathfinding
    public Pathfinding pathfindingGrid;
    //Vector3 startPos;
    Vector3 endPos;
    public GameObject testcreatureprefab;
    GameObject testcreature;

    //testing out room system
    public RoomGrid roomGrid;
    [SerializeField] private RoomOverlayVisuals roomOverlay;
    float editingRoomID = 0.1f;
    public bool roomPicker = false;
    [SerializeField] GameObject roomEditorPanelPrefab;
    GameObject roomEditorPanel;

    AudioSystem audioSystem;
    [SerializeField] AudioClip backgroundMusic;

    //reference to the tiletype that is stored to change to a new type when the tile object is clicked on, initially set to floor
    Tile.TileType tileTypesprite = Tile.TileType.Floor;

    public enum EditorState { TILE_EDITOR, ROOM_EDITOR, TEST}
    public EditorState editorState;

    // Start is called before the first frame update
    void Awake()
    {
        //instantiate the tilemap
        floorTileMap = new FloorTileMap(gridSize, gridSize, cellSize, new Vector3(0, 0, 0));
        //set up the visuals to display correctly
        floorTileMap.SetTileMapVisual(visuals);

        //instatiate the pathfinding grid
        pathfindingGrid = new Pathfinding(gridSize*2, gridSize*2, cellSize/2);
        //initially set up the walkability
        pathfindingGrid.SetWalkability(floorTileMap.tileGrid);

        //instantiate the room dictionary
        roomGrid = new RoomGrid();
        //initially set up the room overlay visuals to display correctly
        roomOverlay.SetRoomOverlay(roomGrid, floorTileMap.tileGrid);

        //set up the editor state so that it starts in the tile editor mode
        editorState = EditorState.TILE_EDITOR;

       
        
    }
    void Start()
    { 
        //set up the background music
        audioSystem = AudioSystem.Instance;
        audioSystem.PlayBackgroundMusic(backgroundMusic);
    }

    // Update is called once per frame
    void Update()
    {
        //makes sure the mouse isnt over the ui elements that i have created
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //if the level editor is in the tile editor state it allows the designers/players to edit the tiles 
            if (editorState == EditorState.TILE_EDITOR)
            {
                //testing to see if the coordinate conversion worked and that the value saved in the int array can be changed
                if (Input.GetMouseButton(0))
                {
                    Vector3 position = InputUtilities.ScreenToWorldPoint(Input.mousePosition, Camera.main);
                    //can only do this in the lv editor, comment out the set tile method when game has been completed
                    floorTileMap.SetTileType(position, tileTypesprite);

                }
                //methods used to create levels in the game e.g. map editor, mainly to be used as a tool to speed up the games development
                //they change the tiletype that the player is placing down in the editor when the player clicks on a number key, by default the tiles are set to space and when clicked it becomes floor
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    tileTypesprite = Tile.TileType.Floor;
                    Debug.Log(tileTypesprite);
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    tileTypesprite = Tile.TileType.WallBottomLeft;
                    Debug.Log(tileTypesprite);
                }
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    tileTypesprite = Tile.TileType.WallBottomRight;
                    Debug.Log(tileTypesprite);
                }
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    tileTypesprite = Tile.TileType.WallTopLeft;
                    Debug.Log(tileTypesprite);
                }
                if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    tileTypesprite = Tile.TileType.WallTopRight;
                    Debug.Log(tileTypesprite);
                }
                if(Input.GetKeyDown(KeyCode.Alpha6))
                {
                    tileTypesprite = Tile.TileType.WallCornerBottom;
                    Debug.Log(tileTypesprite);
                }
                if (Input.GetKeyDown(KeyCode.Alpha7))
                {
                    tileTypesprite = Tile.TileType.WallCornerTop;
                    Debug.Log(tileTypesprite);
                }
                if (Input.GetKeyDown(KeyCode.Alpha8))
                {
                    tileTypesprite = Tile.TileType.WallCornerLeft;
                    Debug.Log(tileTypesprite);
                }
                if (Input.GetKeyDown(KeyCode.Alpha9))
                {
                    tileTypesprite = Tile.TileType.WallCornerRight;
                    Debug.Log(tileTypesprite);
                }
                if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    tileTypesprite = Tile.TileType.CorridorLeft;
                    Debug.Log(tileTypesprite);
                }
                if(Input.GetKeyDown(KeyCode.P))
                {
                    tileTypesprite = Tile.TileType.CorridorRight;
                    Debug.Log(tileTypesprite);
                }
                if (Input.GetKeyDown(KeyCode.T))
                {
                    tileTypesprite = Tile.TileType.EndLeftTop;
                    Debug.Log(tileTypesprite);
                }
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    tileTypesprite = Tile.TileType.EndRightTop;
                    Debug.Log(tileTypesprite);
                }
                if (Input.GetKeyDown(KeyCode.U))
                {
                    tileTypesprite = Tile.TileType.EndLeftBottom;
                    Debug.Log(tileTypesprite);
                }
                if (Input.GetKeyDown(KeyCode.I))
                {
                    tileTypesprite = Tile.TileType.EndRightBottom;
                    Debug.Log(tileTypesprite);
                }


                if (Input.GetMouseButton(1))
                {
                    Vector3 position = InputUtilities.ScreenToWorldPoint(Input.mousePosition, Camera.main);
                    //can only do this in the lv editor, comment out the set tile method when game has been completed
                    floorTileMap.SetTileType(position, Tile.TileType.Space);
                }
            }
            //if the level editor is in the room editor state it allows the designers/players to edit the rooms and their values 
            else if (editorState == EditorState.ROOM_EDITOR)
            {
                //if the designer is using the roompicker tool, change the room they are editing based on where they click
                if(roomPicker == true)
                {
                    if(Input.GetMouseButtonDown(0))
                    {
                        //get the world position of where the mouse was clicked
                        Vector3 position = InputUtilities.ScreenToWorldPoint(Input.mousePosition, Camera.main);
                        //get the tile that the mouse is over
                        Tile currentTile = floorTileMap.tileGrid.GetGridObject(position);
                        if (currentTile.GetTileType() == Tile.TileType.Floor)
                        {
                            //if the tile is in a room
                            if (currentTile.GetRoomId() >= 0.1f)
                            {
                                //make the room the current room that is being edited
                                editingRoomID = currentTile.GetRoomId();
                                //make the room clicker false
                            }
                            //if the current tile isnt in a room and rooms have already been created in the lv
                            else
                            {
                                //make the current editing room the last room that was added
                                editingRoomID = roomGrid.roomsInLv.Keys.Last();
                                //make the room clicker false
                            }
                        }
                    }
                    //for testing purposes remove later when working
                    /*if(Input.GetMouseButtonDown(1))
                    {
                        //get the world position of where the mouse was clicked
                        Vector3 position = InputUtilities.ScreenToWorldPoint(Input.mousePosition, Camera.main);
                        //get the tile that the mouse is over
                        Tile currentTile = floorTileMap.tileGrid.GetGridObject(position);
                        Debug.Log(currentTile);
                        float roomID = currentTile.GetRoomId();
                        Debug.Log(roomID);
                        Room room = roomGrid.roomsInLv[roomID];
                        Debug.Log(room.roomType);
                        Debug.Log(room.RoomName);
                    }*/
                }
                else
                {
                    //testing to see if the room grid works correctly
                    //when the mouse button is held down the tile you are over will be added to current room
                    if (Input.GetMouseButton(0) && roomGrid.roomsInLv.Values.First() != null)
                    {
                        Vector3 position = InputUtilities.ScreenToWorldPoint(Input.mousePosition, Camera.main);
                        //can only do this in the lv editor, comment out the set tile method when game has been completed
                        Tile currentTile = floorTileMap.tileGrid.GetGridObject(position);
                        //add the tile to the current room
                        if (currentTile.GetTileType() == Tile.TileType.Floor)
                        {
                            if (editingRoomID != 0 && roomPicker == true)
                            {
                                currentTile.SetRoomID(editingRoomID);
                            }

                            else
                            {
                                editingRoomID = roomGrid.roomsInLv.Keys.Last();
                                currentTile.SetRoomID(editingRoomID);
                            }
                        }                        

                    }
                    //once you have a list of tiles to add to a new room right click to create the room with the command type as default
                    if (Input.GetMouseButton(1))
                    {
                        Vector3 position = InputUtilities.ScreenToWorldPoint(Input.mousePosition, Camera.main);
                        //can only do this in the lv editor, comment out the set tile method when game has been completed
                        Tile currentTile = floorTileMap.tileGrid.GetGridObject(position);
                        currentTile.SetRoomID(0.1f);
                    }
                }
                
            }
            //if the level editor is in the test pathfinding state it allows designers and programmers to test out the pathfinding
            else if (editorState == EditorState.TEST)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    if(testcreature != null)
                    {
                        endPos = InputUtilities.ScreenToWorldPoint(Input.mousePosition, Camera.main);
                        //testcreature.GetComponent<Actor>().SetTarget();
                        pathfindingGrid.PathfindingGrid.GetXYCoord(testcreature.transform.position, out int startx, out int starty);
                        pathfindingGrid.PathfindingGrid.GetXYCoord(endPos, out int endx, out int endy);
                        List<PathfindingNode> pathfindingNodes = pathfindingGrid.FindPath(startx, starty, endx, endy);
                        if (pathfindingNodes != null)
                        {
                            for (int i = 0; i < pathfindingNodes.Count - 1; i++)
                            {
                                Vector3 nodeWorldPos = pathfindingGrid.PathfindingGrid.GetWorldPosition(pathfindingNodes[i].X, pathfindingNodes[i].Y);
                                Vector3 nextnodeWorldPos = pathfindingGrid.PathfindingGrid.GetWorldPosition(pathfindingNodes[i + 1].X, pathfindingNodes[i + 1].Y);
                               // Debug.DrawLine(new Vector3(nodeWorldPos.x + 2, nodeWorldPos.y), new Vector3(nextnodeWorldPos.x + 2, nextnodeWorldPos.y), Color.green, 10f);
                            }
                        }
                    }
                    else
                    {
                        Vector3 mousePosition = InputUtilities.ScreenToWorldPoint(Input.mousePosition, Camera.main);
                        if (floorTileMap.tileGrid.GetGridObject(mousePosition).GetTileType() != Tile.TileType.Space)
                        {
                            testcreature = Instantiate(testcreatureprefab, new Vector3(mousePosition.x, mousePosition.y, 0) , Quaternion.identity);
                            SpriteLayerSystem.Instance.AddSpriteToArray(testcreature.GetComponent<SpriteRenderer>());
                        }
                        else
                        {
                            Debug.Log("you must place the test creature on an existing tile");
                        }
                    }

                }

                /*else if(Input.GetMouseButtonDown(0) && testcreature.transform == null)
                {
                    //get a position to spawn the panel
                    var panelPosition = InputUtilities.ScreenToWorldPoint(Input.mousePosition, Camera.main);
                    //get the tile that was clicked on
                    Tile currentTile = floorTileMap.tileGrid.GetGridObject(panelPosition);
                    //instantiate the panel in that position
                    GameObject panel = Instantiate(roomEditorPanel, panelPosition, Quaternion.identity);
                    //set up the panel so that it shows the values of the room that was clicked on
                    panel.GetComponent<RoomControlPanelScript>().SetupPanel(currentTile.GetRoomId());
                }*/

                if(Input.GetMouseButtonDown(1))
                {
                    //get the canvas to use for the ui panel
                    var canvas = GameObject.Find("Canvas");
                    //get a position to spawn the panel
                    var panelPosition = Input.mousePosition;
                    //get the tile that was clicked on
                    Tile currentTile = floorTileMap.tileGrid.GetGridObject(InputUtilities.ScreenToWorldPoint(panelPosition, Camera.main));
                    //instantiate the panel in that position if it doesnt already exist
                    if(roomEditorPanel == null)
                    {
                        roomEditorPanel = Instantiate(roomEditorPanelPrefab, new Vector3(panelPosition.x + 120f, panelPosition.y + 30f), Quaternion.identity, canvas.transform);
                    }
                    //if it does already exist move it to the new clicked position instead
                    else
                    {
                        //create a new one in the new place
                        roomEditorPanel.transform.position = new Vector3(panelPosition.x + 120f, panelPosition.y + 30f);
                    }
                    //set up the panel so that it shows the values of the room that was clicked on
                    roomEditorPanel.GetComponent<RoomControlPanelScript>().SetupPanel(currentTile.GetRoomId());

                }
                //the first time the player clicks will be the start of the pathfinding path
                /*if (Input.GetMouseButtonDown(0) && isfirstclick == true)
                {
                    pathfindingGrid.SetWalkability(floorTileMap.tileGrid);
                    startPos = InputUtilities.ScreenToWorldPoint(Input.mousePosition, Camera.main);
                    isfirstclick = false;
                }
                //the second time the player clicks will be the end of the pathfinding path
                else if (Input.GetMouseButtonDown(0) && isfirstclick == false)
                {
                    pathfindingGrid.SetWalkability(floorTileMap.tileGrid);
                    endPos = InputUtilities.ScreenToWorldPoint(Input.mousePosition, Camera.main);
                    pathfindingGrid.PathfindingGrid.GetXYCoord(startPos, out int startx, out int starty);
                    pathfindingGrid.PathfindingGrid.GetXYCoord(endPos, out int endx, out int endy);
                    List<PathfindingNode> pathfindingNodes = pathfindingGrid.FindPath(startx, starty, endx, endy);
                    if (pathfindingNodes != null)
                    {
                        for (int i = 0; i < pathfindingNodes.Count - 1; i++)
                        {
                            Vector3 nodeWorldPos = pathfindingGrid.PathfindingGrid.GetWorldPosition(pathfindingNodes[i].X, pathfindingNodes[i].Y);
                            Vector3 nextnodeWorldPos = pathfindingGrid.PathfindingGrid.GetWorldPosition(pathfindingNodes[i + 1].X, pathfindingNodes[i + 1].Y);
                            Debug.DrawLine(new Vector3(nodeWorldPos.x + 2, nodeWorldPos.y), new Vector3(nextnodeWorldPos.x + 2, nextnodeWorldPos.y), Color.green, 10f);
                        }
                    }
                    isfirstclick = true;
                }*/

                //if the player right clicks it cancels the pathfinding path so that they can start again
                /*if (Input.GetMouseButtonDown(1))
                {
                    isfirstclick = true;
                    Debug.Log("reset pathfinding");
                }*/

            }
        }
    }
}
