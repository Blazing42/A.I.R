using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FloorTileMap {

    public event EventHandler OnLoaded;

    public Grid<Tile> tileGrid;

    //method to create a new tilemap using the grid class 
    public FloorTileMap(int width, int height, float cellsize, Vector3 originPosition)
    {
        tileGrid = new Grid<Tile>(width, height, cellsize, originPosition, true, (Grid<Tile> tileg, int x, int y) => new Tile(tileg, x, y));
    }

    //method used to set the tilemap visuals once the grid has been created
    public void SetTileMapVisual(TileMapVisuals tileMapVisual)
    {
        tileMapVisual.SetTileMap(this);
    }
    
    //method used when changing the tiletype of a tile in a specific position
    public void SetTileType(Vector3 position, Tile.TileType newtileType)
    {
        //get the tile in the grid position based on the world position of the players mouse
        Tile tile = tileGrid.GetGridObject(position);
            if(tile != null)
            {
            //if the object is found change it to the new tiletype specfied by the method
                tile.SetTileType(newtileType);
            }
    }

    //method used to save the tilemap 
    public void SaveTileMap(string filename)
    {
        //create a list to store the tile save objects
        List<Tile.SaveObject> tileSaveList = new List<Tile.SaveObject>();
        //cycle through all of the tiles in the tilemap grid
        for (int x = 0; x < tileGrid.width; x++)
        {
            for (int y = 0; y < tileGrid.height; y++)
            {
                //get the tile in each grid position
                Tile tile = tileGrid.GetGridObject(x, y);
                //create a saveobject from each of those tiles
                Tile.SaveObject saveobject = tile.Save();
                //add them to the tile save list
                tileSaveList.Add(saveobject);
            }
        }
        //create a new tilemap save object and add the list ot tile saveobjects to the array
        SaveObject saveObject = new SaveObject
        {
            tilemapSaveObjectwithTileArray = tileSaveList.ToArray(),
        };
        SaveSystem.SaveObject(saveObject, filename, true);
    }

    //method used to reset the tilemap to its original all space format
    public void ResetTileMap()
    {
        for (int x = 0; x < tileGrid.width; x++)
        {
            for (int y = 0; y < tileGrid.height; y++)
            {
                //get the tile in each grid position
                Tile tile = tileGrid.GetGridObject(x, y);
                tile.SetTileType(Tile.TileType.Space);
            }
        }
    }

    //method used to load the most recently worked on tileMap
    public void LoadMostRecentTileMap()
    {
        SaveObject saveObject = SaveSystem.LoadMostRecentObject<SaveObject>();
        foreach(Tile.SaveObject savedTile in saveObject.tilemapSaveObjectwithTileArray)
        {
            Tile tile = tileGrid.GetGridObject(savedTile.x, savedTile.y);
            tile.Load(savedTile);
        }
        OnLoaded?.Invoke(this, EventArgs.Empty);
    }

    //method used to load a tilemap
    public void LoadTileMap(string filename)
    {
        SaveObject saveObject = SaveSystem.LoadObject<SaveObject>(filename);
        foreach (Tile.SaveObject savedTile in saveObject.tilemapSaveObjectwithTileArray)
        {
            Tile tile = tileGrid.GetGridObject(savedTile.x, savedTile.y);
            tile.Load(savedTile);
        }
        OnLoaded?.Invoke(this, EventArgs.Empty);
    }

    //tilemap save object it is an array because json utilities doesnt work directly with lists
    public class SaveObject
    {
        public Tile.SaveObject[] tilemapSaveObjectwithTileArray;
    }
}
