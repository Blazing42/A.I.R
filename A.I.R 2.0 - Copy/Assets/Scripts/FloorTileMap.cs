using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTileMap {

    public Grid<Tile> tileGrid;

    public void SetTileType(Vector3 position, Tile.TileType newtileType)
    {
        Tile tile = tileGrid.GetGridObject(position);
            if(tile != null)
            {
                tile.SetTileType(newtileType);
            }
    }
    
    public FloorTileMap(int width, int height, float cellsize, Vector3 originPosition)
    {
        tileGrid = new Grid<Tile>(width, height, cellsize, originPosition, (Grid<Tile> tileg, int x, int y) => new Tile(tileg, x, y));
    }

    public void SetTileMapVisual(TileMapVisuals tileMapVisual)
    {
        tileMapVisual.SetTileMap(this);
    }
}
