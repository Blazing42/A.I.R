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
    public TileMapVisuals visuals;

    // Start is called before the first frame update
    void Start()
    {
        //instantiate the tilemap
        floorTileMap = new FloorTileMap(12, 12, 4, new Vector3(0, 0, 0));
        //fill it with the tile info obtained from this specific level tilemap data

        //instatiate the pathfinding grid
        //pathfindingGrid = new Grid(24, 24, 2, new Vector3(0, 0, 0));
        //have the pathfinding grid detect what tile is on top of it and determine if the point is walkable or not filling out the grid with pathfinding data. 
        //testing out the visuals
        visuals.CreateVisualsfromTilemap(floorTileMap);
        
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

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            tileTypesprite = Tile.TileType.Floor;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            tileTypesprite = Tile.TileType.WallExternal;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            tileTypesprite = Tile.TileType.Space;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Tile value = floorTileMap.tileGrid.GetGridObject(ScreenToWorldPoint(Input.mousePosition, Camera.main));
            Debug.Log(value.GetTileType());
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
