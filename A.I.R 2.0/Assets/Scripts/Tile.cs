using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile 
{
    //variable to store the tiletype, to determine what graphic is loaded
    //create an enumerator to store the different tiletypes
    public enum TileType{Space, Floor, WallExternal, WallInternal, ExternalCorner, InternalCorner, InternalEnd, Door /*, StairsUp, StairsDown */};

    //set the default to space to help with tilemap creation in the editor
    TileType tileType = TileType.Space;

    //co-ordinates of the tile so it knows where it is on the tilemap
    public int x;
    public int y;
    
    //variables to keep track of how damaged the wall is, and whether it is walkable
    //referenced by the pathfinding system, if hp below 20 the creatures can walk over the tile if its a wall
    //referenced by the graphics system as the tile looses hp, the graphic will change to a more damaged version of the tile graphic
    //set the default value to be 100% hp and not walkable
    public int hp = 100;
    public bool walkable = false;

    //reference to the tilegrid that it is attached to, which floor/level
    private Grid<Tile> tileGrid;

    //method that changes the tileType maybe it can be used as an editing tool to allow designers to create new cool levels and a level editor down the line
    public void SetTileType(TileType newType)
    {
        this.tileType = newType;
        tileGrid.TriggerGridObjectChanged(x, y);
        SetHPandWalkability();
    }

    public TileType GetTileType()
    {
        return tileType;
    }

    public override string ToString()
    {
        return tileType.ToString();
    }

    //constructor for the tiles
    public Tile(Grid<Tile> tileGrid, int x, int y)
    {
        this.tileGrid = tileGrid;
        this.x = x;
        this.y = y;
        SetHPandWalkability();
    }

    //sets the tiles hp and walkability based on the tiletype of the tile for initial set up
    public void SetHPandWalkability()
    {
        if (tileType == TileType.Floor || tileType == TileType.Door)
        {
            this.walkable = true;
        }

        if (tileType == TileType.Space)
        {
            this.hp = 0;
        }
        else
        {
            this.hp = 100;
        }
    }
}
