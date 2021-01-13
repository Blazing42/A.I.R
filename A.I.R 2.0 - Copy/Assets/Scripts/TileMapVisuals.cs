﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileMapVisuals : MonoBehaviour
{
    //still working on this class to join the tilevisuals and the tiledata together
    //reference to the tilemap that the tielmap visuals will use as reference
    FloorTileMap floorTileMap;
    Mesh mesh;
    //reference to the grid of tiles underneath the visuals
    Grid<Tile> grid;

    GameObject[,] tileSprites;

    void Awake()
    {
        //creating a new empty mesh to display the tilemap visuals
        mesh = new Mesh();
        //adding the mesh to the mesh filter on this object so it can display the material correctly 
        GetComponent<MeshFilter>().mesh = mesh;
    }



    public void SetTileMap(FloorTileMap tileMap)
    {
        floorTileMap = tileMap;
        grid = floorTileMap.tileGrid;
        UpdateTileMapVisual();

        //subscribed to the event that is triggered when a grid tile changes
        grid.OnGridValueChanged += TileGrid_OnGridValueChanged;
    }

    //when the event is triggered update all of the map visuals
    private void TileGrid_OnGridValueChanged(object sender, Grid<Tile>.OnGridValueChangedEvent e)
    {
        Debug.Log("Tilemap VIsuals Reset");
        UpdateTileMapVisual();
    }

    public void UpdateTileMapVisual()
    {
        CreateEmptyMeshArray(grid.width * grid.height, out Vector3[] vertices, out Vector2[] uv, out int[] triangles);
        for (int x = 0; x < grid.width; x++)
        {
            //this for loop done backwards to ensure that the tiles with a higher y value get rendered first behind the ones in front to simulate depth in isometric view
            //remember to make sure that the wall objects are in a sorting layer that if in front of the creatures and the floor and space are behind the creatures so they appear correctly
            for (int y = grid.height - 1; y > -1; y--)
            {
                //asigns an index value to each of the grid squares
                int index = x * grid.height + (grid.height - (y+1));
                //Debug.Log(index);
                Vector3 quadsize = new Vector3(2, 2) * grid.cellsize;
                //gets the tile that is on the x,y of the grid and asigns it to a variable
                Tile currentTile = grid.GetGridObject(x,y);
                //Debug.Log(currentTile.ToString());
                //gets the tileType enum from the current tile and assignes it to a variable
                Tile.TileType tileType = currentTile.GetTileType();
                Vector2 gridUV;
                if(tileType == Tile.TileType.Space)
                {
                    gridUV = Vector2.zero;
                }
                else
                {
                    gridUV = Vector2.one;
                 }

                AddToMeshArrays(vertices, uv, triangles, index,  new Vector3( grid.GetWorldPosition(x, y).x + quadsize.x* 0.5f, grid.GetWorldPosition(x, y).y - quadsize.y*0.1f), 0f, quadsize, Vector2.zero, gridUV);
            }
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    //creates an empty custom mesh with quadNo of quads put this method into a mesh seperate mesh call later on
    public void CreateEmptyMeshArray(int quadNo, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles)
    {
        vertices = new Vector3[4* quadNo];
        uvs = new Vector2[4* quadNo];
        triangles = new int[6* quadNo];
    }

    //put this method into a mesh seperate mesh call later on, adds a material to each of the quads created earlier
    public void AddToMeshArrays(Vector3[] vertices, Vector2[] uvs, int[] triangles, int index, Vector3 pos, float rotation, Vector3 baseSize, Vector2 uv00, Vector2 uv11)
    {
        //relocates the vertices
        int vertexIndex = index * 4;
        int vIndex0 = vertexIndex;
        int vIndex1 = vertexIndex + 1;
        int vIndex2 = vertexIndex + 2;
        int vIndex3 = vertexIndex + 3;

        //relocates the uvs
        uvs[vIndex0] = new Vector2(uv00.x, uv11.y);
        uvs[vIndex1] = new Vector2(uv00.x, uv00.y);
        uvs[vIndex2] = new Vector2(uv11.x, uv00.y);
        uvs[vIndex3] = new Vector2(uv11.x, uv11.y);

        //Create triangles
        int tIndex = index * 6;

        triangles[tIndex + 0] = vIndex0;
        triangles[tIndex + 1] = vIndex3;
        triangles[tIndex + 2] = vIndex1;

        triangles[tIndex + 3] = vIndex1;
        triangles[tIndex + 4] = vIndex3;
        triangles[tIndex + 5] = vIndex2;

        baseSize *= 0.5f;
        bool skewed = baseSize.x != baseSize.y;
        if(skewed == true)
        {
            vertices[vIndex0] = pos + GetQuaternionEuler(rotation) * new Vector3(-baseSize.x, baseSize.y);
            vertices[vIndex1] = pos + GetQuaternionEuler(rotation) * new Vector3(-baseSize.x, -baseSize.y);
            vertices[vIndex2] = pos + GetQuaternionEuler(rotation) * new Vector3(baseSize.x, -baseSize.y);
            vertices[vIndex3] = pos + GetQuaternionEuler(rotation) * baseSize;
        }
        else
        {
            vertices[vIndex0] = pos + GetQuaternionEuler(rotation-270) * baseSize;
            vertices[vIndex1] = pos + GetQuaternionEuler(rotation -180) * baseSize;
            vertices[vIndex2] = pos + GetQuaternionEuler(rotation-90) * baseSize;
            vertices[vIndex3] = pos + GetQuaternionEuler(rotation) * baseSize;
        }

    }

    //put this method into a mesh seperate mesh call later on
    Quaternion[] cashedQuaternionEulerArr;
    public void CasheQuaternionEuler()
    {
        if(cashedQuaternionEulerArr != null)
        {
            return;
        }
        else
        {
            cashedQuaternionEulerArr = new Quaternion[360];
            for (int i = 0; i < 360; i++)
            {
                cashedQuaternionEulerArr[i] = Quaternion.Euler(0, 0, i);
            }
        }
    }

    //put this method into a mesh seperate mesh call later on
    Quaternion GetQuaternionEuler(float rotation)
    {
        int rot = Mathf.RoundToInt(rotation);
        rot = rot % 360;
        if(rot < 0)
        {
            rot += 360;
        }
        if(cashedQuaternionEulerArr == null)
        {
            CasheQuaternionEuler();
        }
        return cashedQuaternionEulerArr[rot];
    }

}