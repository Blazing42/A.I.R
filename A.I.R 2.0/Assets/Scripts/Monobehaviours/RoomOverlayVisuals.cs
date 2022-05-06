using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoomOverlayVisuals : MonoBehaviour
{
    //working on this so that it will eventually display the various overlays of the rooms e.g. temp overlay, oxygen overlay etc
    Grid<Tile> tileGrid;
    RoomGrid roomDict;
    bool updateOverlayMesh;
    Mesh mesh;

    //struct used to assign a tiletype to a set of pixels on a spritesheet so the correct visuals are displayed
    [Serializable]
    public struct RoomOverlayUVs
    {
        public Room.RoomType roomType;
        public Vector2Int uv00pixels;
        public Vector2Int uv11pixels;
    }

    //struct used to convert pixel values of an image to their normalized between o and one values on a uv texture map
    public struct UVCoords
    {
        public Vector2 uv00;
        public Vector2 uv11;
    }

    //stores a set of tile map sprite UVs that can be easily manipulated in the inspector
    [SerializeField] RoomOverlayUVs[] roomOverlaySpriteUVs;
    List<Room> rooms;
    private Dictionary<Room.RoomType, UVCoords> uvCoordsDictionary;

    // Start is called before the first frame update
    void Awake()
    {
        //creating a new empty mesh to display the tilemap visuals
        mesh = new Mesh();
        //adding the mesh to the mesh filter on this object so it can display the material correctly 
        GetComponent<MeshFilter>().mesh = mesh;
        //

        //go into our mesh renderer material and access the texture so we can get a width and height 
        Texture roomOverlaySpriteSheet = GetComponent<MeshRenderer>().material.mainTexture;
        float textureH = roomOverlaySpriteSheet.height;
        float textureW = roomOverlaySpriteSheet.width;

        //create and fill out a dictionary of room to uv values by using the pixels added values in the editor/overall texture size
        rooms = new List<Room>();
        uvCoordsDictionary = new Dictionary<Room.RoomType, UVCoords>();
        foreach (RoomOverlayUVs roomOverlayUV in roomOverlaySpriteUVs)
        {
            uvCoordsDictionary[roomOverlayUV.roomType] = new UVCoords
            {
                uv00 = new Vector2(roomOverlayUV.uv00pixels.x / textureW, roomOverlayUV.uv00pixels.y / textureH),
                uv11 = new Vector2(roomOverlayUV.uv11pixels.x / textureW, roomOverlayUV.uv11pixels.y / textureH),
            };
            //Debug.Log(uvCoordsDictionary[roomOverlayUV.roomType].uv00);
            //Debug.Log(uvCoordsDictionary[roomOverlayUV.roomType].uv11);
        }

    }

    void LateUpdate()
    {
        if (updateOverlayMesh == true)
        {
            updateOverlayMesh = false;
            //update the room overlay
            UpdateRoomOverlayVisual();
        }
    }

    public void SetRoomOverlay(RoomGrid lvRoomGrid, Grid<Tile> tileGrid)
    { 
        updateOverlayMesh = true;
        roomDict = lvRoomGrid;
        this.tileGrid = tileGrid;
        //for when the rooms saving and loading get implemented
        //lvRoomGrid.OnLoaded += TileMap_OnLoaded;

        //subscribed to the event that is triggered when a grid tile changes
        tileGrid.OnGridValueChanged += TileGrid_OnGridValueChanged;

        //subscribed to the event that is triggered when a new room is created
        lvRoomGrid.OnRoomCreated += TileMap_OnRoomCreated;
        roomDict.OnLoaded += TileMap_OnLoaded;
    }

    public void UpdateRoomOverlayVisual()
    {
        //creates a new empty mesh
        MeshUtilities.CreateEmptyMeshArray(tileGrid.width * tileGrid.height, out Vector3[] vertices, out Vector2[] uv, out int[] triangles);
        for (int x = 0; x < tileGrid.width; x++)
        {
            //this for loop done backwards to ensure that the tiles with a higher y value get rendered first behind the ones in front to simulate depth in isometric view
            //remember to make sure that the wall objects are in a sorting layer that if in front of the creatures and the floor and space are behind the creatures so they appear correctly
            for (int y = tileGrid.height - 1; y > -1; y--)
            {
                //asigns an index value to each of the grid squares
                int index = x * tileGrid.height + (tileGrid.height - (y + 1));
                //Debug.Log(index);
                Vector3 quadsize = new Vector3(2, 2) * tileGrid.cellsize;
                //gets the tile that is on the x,y of the grid and asigns it to a variable
               Tile currentRoomTileGridObj = tileGrid.GetGridObject(x, y);
                //Debug.Log(currentRoomGridObj.ToString());
                //gets the tileType enum from the current tile and assignes it to a variable
                Room room = roomDict.GetRoom( currentRoomTileGridObj.GetRoomId());
                //assign vector 2 values to the the bottom left and top right hand corners of the uvs
                //use these to define pixel values for the opposite corners of the various tile sprites within the whole spritesheet material 
                Vector2 gridUV00, gridUV11;
                if (room != null)
                {
                    Room.RoomType roomType = room.roomType;
                    
                    if(roomType == Room.RoomType.NONE)
                    {   
                        gridUV00 = Vector2.zero;
                        gridUV11 = Vector2.zero;
                        quadsize = Vector3.zero;
                    }
                    else
                    {
                        UVCoords uvCoords = uvCoordsDictionary[roomType];
                        gridUV00 = uvCoords.uv00;
                        //Debug.Log(uvCoords.uv00.ToString());
                        gridUV11 = uvCoords.uv11;
                        //Debug.Log(uvCoords.uv11.ToString());
                    }
                }
                else
                {
                    gridUV00 = Vector2.zero;
                    gridUV11 = Vector2.zero;
                    quadsize = Vector3.zero;  
                }
                MeshUtilities.AddToMeshArrays(vertices, uv, triangles, index, new Vector3(tileGrid.GetWorldPosition(x, y).x + quadsize.x * 0.5f, tileGrid.GetWorldPosition(x, y).y), 0f, quadsize, gridUV00, gridUV11);
            }
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    private void TileMap_OnLoaded(object sender, System.EventArgs e)
    {
        UpdateRoomOverlayVisual();
        //Debug.Log("Room Visuals Reset");
        updateOverlayMesh = true;
    }

    //when the event is triggered update all of the map visuals
    private void TileGrid_OnGridValueChanged(object sender, Grid<Tile>.OnGridValueChangedEvent e)
    {
        //Debug.Log("Room Visuals Reset");
        updateOverlayMesh = true;
    }

    private void TileMap_OnRoomCreated(object sender, RoomGrid.OnRoomCreatedEvent e)
    {
        Debug.Log("Room Visuals Reset");
        rooms.Add(e.newRoom);
        updateOverlayMesh = true;
    }

}
