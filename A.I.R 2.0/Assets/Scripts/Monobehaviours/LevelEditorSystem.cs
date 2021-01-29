using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    bool isfirstclick = true;
    public GameObject testcreature;

    //testing out room system
    public RoomGrid roomGrid;
    List<RoomGridObject> newRoomGridObjects;
    [SerializeField] private RoomOverlayVisuals roomOverlay;

    //reference to the tiletype that is stored to change to a new type when the tile object is clicked on, initially set to floor
    Tile.TileType tileTypesprite = Tile.TileType.Floor;

    public enum EditorState { TILE_EDITOR, ROOM_EDITOR, TEST}
    public EditorState editorState;

    // Start is called before the first frame update
    void Start()
    {
        //instantiate the tilemap
        floorTileMap = new FloorTileMap(gridSize, gridSize, cellSize, new Vector3(0, 0, 0));
        //set up the visuals to display correctly
        floorTileMap.SetTileMapVisual(visuals);

        //instatiate the pathfinding grid
        pathfindingGrid = new Pathfinding(gridSize*2, gridSize*2, cellSize/2);
        //initially set up the walkability
        pathfindingGrid.SetWalkability(floorTileMap.tileGrid);

        //instantiate the room grid
        roomGrid = new RoomGrid(gridSize, gridSize, cellSize, new Vector3(0, 0, 0));
        //initially set up the room overlay visuals to display correctly
        roomOverlay.SetRoomOverlay(roomGrid);
        newRoomGridObjects = new List<RoomGridObject>();

        //set up the editor state so that it starts in the tile editor mode
        editorState = EditorState.TILE_EDITOR;

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
                    tileTypesprite = Tile.TileType.Space;
                    Debug.Log(tileTypesprite);
                }
            }
            //if the level editor is in the room editor state it allows the designers/players to edit the rooms and their values 
            else if (editorState == EditorState.ROOM_EDITOR)
            {
                //testing to see if the room grid works correctly
                //when the mouse button is heald down the tile you are over will be added to the list of tiles that will be added to the new room
                if (Input.GetMouseButton(0))
                {
                    Vector3 position = InputUtilities.ScreenToWorldPoint(Input.mousePosition, Camera.main);
                    RoomGridObject roomObject = roomGrid.roomGrid.GetGridObject(position);
                    //just to make sure that if the tile was already added in the previous frame dont add it again
                    if (!newRoomGridObjects.Contains(roomObject))
                    {
                        newRoomGridObjects.Add(roomObject);
                        Debug.Log("tile added to new room");
                    }

                }
                //once you have a list of tiles to add to a new room right click to create the room with the command type as default
                if (Input.GetMouseButtonDown(1))
                {
                    if (newRoomGridObjects.Count > 0)
                    {
                        roomGrid.CreateRoom(newRoomGridObjects, "new room", Room.RoomType.COMMAND);
                        newRoomGridObjects.Clear();
                    }
                }
            }
            //if the level editor is in the test pathfinding state it allows designers and programmers to test out the pathfinding
            else if (editorState == EditorState.TEST)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    endPos = InputUtilities.ScreenToWorldPoint(Input.mousePosition, Camera.main);
                    testcreature.GetComponent<Actor>().SetTarget(endPos);
                    pathfindingGrid.PathfindingGrid.GetXYCoord(testcreature.transform.position, out int startx, out int starty);
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
                if (Input.GetMouseButtonDown(1))
                {
                    isfirstclick = true;
                    Debug.Log("reset pathfinding");
                }

            }
        }

        //method that quits the application if the escape key is pressed
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
