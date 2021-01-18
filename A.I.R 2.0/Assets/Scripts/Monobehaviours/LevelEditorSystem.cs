using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelEditorSystem: MonoBehaviour
{
    //allows designer to set the size of the grid they want to work with when creating the levels in the editor
    [SerializeField] int gridSize;
    //create grid variable to store the tilemap 
    //eventually we will create an array of grids to store all of the floor tilemaps for this game level
    public FloorTileMap floorTileMap;

    //testing out pathfinding
    public Pathfinding pathfindingGrid;
    Vector3 startPos;
    Vector3 endPos;
    bool isfirstclick = true;

    //reference to the tiletype that is stored to change to a new type when the tile object is clicked on, initially set to floor
    Tile.TileType tileTypesprite = Tile.TileType.Floor;

    //reference to the object that will be updating the tilemap visuals
    [SerializeField] private TileMapVisuals visuals;

    // Start is called before the first frame update
    void Start()
    {
        //instantiate the tilemap
        floorTileMap = new FloorTileMap(gridSize, gridSize, 4, new Vector3(0, 0, 0));
        //set up the visuals to display correctly
        floorTileMap.SetTileMapVisual(visuals);

        //instatiate the pathfinding grid
        pathfindingGrid = new Pathfinding(gridSize*2, gridSize*2, 2);
        //have the pathfinding grid detect what tile is on top of it and determine if the point is walkable or not filling out the grid with pathfinding data. 
        
    }

    // Update is called once per frame
    void Update()
    {
        //testing to see if the coordinate conversion worked and that the value saved in the int array can be changed
        if(Input.GetMouseButton(0))
        {
            //makes sure the mouse isnt over the ui elements that i have created
            if(!EventSystem.current.IsPointerOverGameObject())
            {
                Vector3 position = ScreenToWorldPoint(Input.mousePosition, Camera.main);
                floorTileMap.SetTileType(position, tileTypesprite);
            }
            
        }

        //testing to see if the pathfinding algorithm works correctly
        /*if(Input.GetMouseButtonDown(1))
        {
            pathfindingGrid.SetWalkability(floorTileMap.tileGrid);
            Vector3 position = ScreenToWorldPoint(Input.mousePosition, Camera.main);
            pathfindingGrid.PathfindingGrid.GetXYCoord(position, out int x, out int y);
            List<PathfindingNode> pathfindingNodes =  pathfindingGrid.FindPath(0, 0, x, y);
            if(pathfindingNodes != null)
            {
                for (int i = 0; i < pathfindingNodes.Count - 1 ; i++)
                {
                    Vector3 nodeWorldPos = pathfindingGrid.PathfindingGrid.GetWorldPosition(pathfindingNodes[i].X, pathfindingNodes[i].Y);
                    Vector3 nextnodeWorldPos = pathfindingGrid.PathfindingGrid.GetWorldPosition(pathfindingNodes[i+1].X, pathfindingNodes[i+1].Y);
                    Debug.DrawLine(new Vector3(nodeWorldPos.x + 2, nodeWorldPos.y) , new Vector3(nextnodeWorldPos.x + 2, nextnodeWorldPos.y), Color.green, 10f);
                }
            }  
        }*/

        if(Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetMouseButtonDown(1) && isfirstclick == true)
            {
                pathfindingGrid.SetWalkability(floorTileMap.tileGrid);
                startPos = ScreenToWorldPoint(Input.mousePosition, Camera.main);
                isfirstclick = false;
            }
            else if (Input.GetMouseButtonDown(1) && isfirstclick == false)
            {
                pathfindingGrid.SetWalkability(floorTileMap.tileGrid);
                endPos = ScreenToWorldPoint(Input.mousePosition, Camera.main);
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
            }
        }
        

        //methods used to create levels in the game e.g. map editor, mainly to be used as a tool to speed up the games development
        //they change the tiletype that the player is placing down in the editor when the player clicks on a number key, by default the tiles are set to space and when clicked it becomes floor
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            tileTypesprite = Tile.TileType.Floor;
            Debug.Log(tileTypesprite);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            tileTypesprite = Tile.TileType.Space;
            Debug.Log(tileTypesprite);
        }
        /*if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            tileTypesprite = Tile.TileType.WallInternal;
            Debug.Log(tileTypesprite);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            tileTypesprite = Tile.TileType.Door;
            Debug.Log(tileTypesprite);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            tileTypesprite = Tile.TileType.ExternalCorner;
            Debug.Log(tileTypesprite);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            tileTypesprite = Tile.TileType.InternalCorner;
            Debug.Log(tileTypesprite);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            tileTypesprite = Tile.TileType.InternalEnd;
            Debug.Log(tileTypesprite);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            tileTypesprite = Tile.TileType.WallExternal;
            Debug.Log(tileTypesprite);
        }*/

        //method that quits the application if the escape key is pressed
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }


    //converts the mouse input position on the screen to the unity world position
    public static Vector3 ScreenToWorldPoint(Vector3 screenPos, Camera camera)
    {
        Vector3 worldPosition = camera.ScreenToWorldPoint(screenPos);
        return worldPosition;
    }

}
