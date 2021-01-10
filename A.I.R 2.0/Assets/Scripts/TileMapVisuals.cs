using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileMapVisuals : MonoBehaviour
{
    //still working on this class to join the tilevisuals and the tiledata together
    FloorTileMap floorTileMap;
    //variable to hold the sprites that will be used to create the visuals for the tilemap
    [SerializeField] GameObject floorSprite;
    //[SerializeField] GameObject wallSprite;
    GameObject[,] tileSprites;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateVisualsfromTilemap (FloorTileMap tileMap)
    {
        //creates a new array to hold the tile visual gameobjects
        tileSprites = new GameObject[tileMap.tileGrid.width, tileMap.tileGrid.height];
        for (int x = 0; x < tileMap.tileGrid.width; x++)
        {
            //this for loop done backwards to ensure that the tiles with a higher y value get rendered first behind the ones in front to simulate depth
            for (int y = tileMap.tileGrid.height - 1; y >= 0; y--)
            {
                GameObject newTileSprite = Instantiate(floorSprite, new Vector3(tileMap.tileGrid.GetWorldPosition(x,y).x + 4, tileMap.tileGrid.GetWorldPosition(x, y).y), Quaternion.identity);
                tileSprites[x, y] = newTileSprite;
                Debug.Log(x + " , " + y);
            }
        }
    }
}
