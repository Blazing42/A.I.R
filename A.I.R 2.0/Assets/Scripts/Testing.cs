using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    //create grid variable to store the tilemap 
    //eventually we will create an array of grids to store all of the floor tilemaps for this game level
    FloorTileMap floorTileMap;
    //create another grid to store the pathfinding nodes
    //Grid pathfindingGrid;

    //reference to the visual that you want to add to the specific tile
    Tile.TileType tileTypesprite = Tile.TileType.Floor;

    //testing
    [SerializeField] private TileMapVisuals visuals;

    // Start is called before the first frame update
    void Start()
    {
        //instantiate the tilemap
        floorTileMap = new FloorTileMap(10, 10, 4, new Vector3(0, 0, 0));
        //fill it with the tile info obtained from this specific level tilemap data

        //instatiate the pathfinding grid
        //pathfindingGrid = new Grid(24, 24, 2, new Vector3(0, 0, 0));
        //have the pathfinding grid detect what tile is on top of it and determine if the point is walkable or not filling out the grid with pathfinding data. 
        //testing out the visuals
        visuals.SetTileMap(floorTileMap);
        
    }

    // Update is called once per frame
    void Update()
    {
        //testing to see if the coordinate conversion worked and that the value saved in the int array can be changed
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 position = ScreenToWorldPoint(Input.mousePosition, Camera.main);
            floorTileMap.SetTileType(position, tileTypesprite);
        }

        //methods used to create levels in the game e.g. map editor, mainly to be used as a tool to speed up the games development
        //they change the tiletype that the player is placing down in the editor when the player clicks on a number key, by default the tiles are set to space
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            tileTypesprite = Tile.TileType.Floor;
            Debug.Log(tileTypesprite);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            tileTypesprite = Tile.TileType.WallExternal;
            Debug.Log(tileTypesprite);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
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
            tileTypesprite = Tile.TileType.Space;
            Debug.Log(tileTypesprite);
        }
        

        //if the player left clicks on a tile it gives them information on whether the tile is walkable etc
        if (Input.GetMouseButtonDown(1))
        {
            Tile value = floorTileMap.tileGrid.GetGridObject(ScreenToWorldPoint(Input.mousePosition, Camera.main));
            Debug.Log(value.hp.ToString() + " , " + value.walkable.ToString());
        }
    }

    //converts the mouse input position on the screen to the unity world position
    public static Vector3 ScreenToWorldPoint(Vector3 screenPos, Camera camera)
    {
        Vector3 worldPosition = camera.ScreenToWorldPoint(screenPos);
        return worldPosition;
    }

}
