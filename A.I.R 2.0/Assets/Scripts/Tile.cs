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
    
    //referenced by the graphics system as the tile looses hp, the graphic will change to a more damaged version of the tile graphic
    //set the default value to be 100% hp and not walkable
    int hp = 100;

    //reference to the tilegrid that it is attached to, which floor/level
    private Grid<Tile> tileGrid;

    //method that changes the tileType maybe it can be used as an editing tool to allow designers to create new cool levels and a level editor down the line
    public void SetTileType(TileType newType)
    {
        this.tileType = newType;
        tileGrid.TriggerGridObjectChanged(x, y);
        SetHP();
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
        SetHP();
    }

    //sets the tiles hp and walkability based on the tiletype of the tile for initial set up
    public void SetHP()
    {
        if (tileType == TileType.Space)
        {
            this.hp = 0;
        }
        else
        {
            this.hp = 100;
        }
    }

    //method that triggers when a tile takes damage from an outside sorce 
    //used for if the aliens are going to have the ability to break through walls and doors if the sitution requires it
    //it sets the hp and walkability of the tiles appropriately
    public void TakeDamage(int damage)
    {
        this.hp -= damage;
        //link to an event that will change the tile visual when the tile reaches specific damage thresholds
        //tileGrid.TriggerGridObjectChanged(x, y);
    }


    //objects used for saving and loading the data in the tiles for the level editor and once the levels have been created
    [System.Serializable]
    public class SaveObject
    {
        public TileType tileType;
        public int x;
        public int y;
        public int hp;
    }

    public SaveObject Save()
    {
        return new SaveObject
        {
            tileType = tileType,
            x = x,
            y = y,
            hp = hp,
        };
    }

    public void Load(SaveObject saveObject)
    {
        tileType = saveObject.tileType;
        hp = saveObject.hp;
    }
}
