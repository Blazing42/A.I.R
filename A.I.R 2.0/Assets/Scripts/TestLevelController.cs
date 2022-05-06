using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLevelController : MonoBehaviour
{
    //variables used to control the initial set up of the level
    //so that the correct level can be loaded for the start of the game level
    [SerializeField] string loadfilename;
    //TileMap
    public FloorTileMap floorTileMap;
    [SerializeField] private TileMapVisuals visuals;
    //Pathfinding
    public Pathfinding pathfindingGrid;
    //Audio
    AudioSystem audioSystem;
    [SerializeField] AudioClip backgroundMusic;

    bool setup = false;


    // Start is called before the first frame update
    void Start()
    {

        //set up the Tilemap
        floorTileMap = new FloorTileMap(60, 60, 4, new Vector3(0, 0, 0));
        //load up the tilemap from the savefile
        floorTileMap.LoadTileMap(loadfilename);
        //set the tilemap visuals
        floorTileMap.SetTileMapVisual(visuals);

        //set up the pathfinder grid
        pathfindingGrid = new Pathfinding(120, 120, 2);
        //initially set up the walkability
        pathfindingGrid.SetWalkability(floorTileMap.tileGrid);

        //set up the background music
        //audioSystem = AudioSystem.Instance;
        //audioSystem.PlayBackgroundMusic(backgroundMusic);
        GOAPGameWorld.WorldInstance.GetWorld().ModifyStateValue("doorClosed", 1);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
