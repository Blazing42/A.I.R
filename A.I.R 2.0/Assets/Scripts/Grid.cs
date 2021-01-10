using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Grid<TGridObject>
{
    public int width;
    public int height;
    private Vector3 originPos;
    private float cellsize;
    private TGridObject[,] gridArray;
    private TextMesh[,] testTextArray;

    //setting up an event to trigger every time a value on the grid changes
    //will eventually use this with the pathfinding system to recalculate if tiles are walkable or not
    public event EventHandler<OnGridValueChangedEvent> OnGridValueChanged;
    public class OnGridValueChangedEvent : EventArgs
    {
        public int x;
        public int y;
    }

    //constructor where you add a width, hight and cellsize to determine the size of the tiles to be added
    public Grid(int width, int height, float cellsize, Vector3 originPos, Func<Grid<TGridObject>,int, int, TGridObject>gridObject)
    {
        this.width = width;
        this.height = height;
        this.cellsize = cellsize;
        this.originPos = originPos;

        //uses the grid width and height to create an array of objects, will make this into a generic so it can be used to create the tilemap and for the pathfinding
        gridArray = new TGridObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = gridObject(this,x,y);
            }
        }

        //Debugging
        //creates an array of text mesh objects to display values for testing
        testTextArray = new TextMesh[width, height];
        //fills in the 
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                testTextArray[x,y] = CreateWorldText(null, gridArray[x, y]?.ToString(), GetWorldPosition(x, y) + new Vector3(cellsize,0), 20, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center, 500);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

        OnGridValueChanged += (object eventsender, OnGridValueChangedEvent eventargs) =>
        {
            testTextArray[eventargs.x, eventargs.y].text = gridArray[eventargs.x, eventargs.y].ToString();
        };
    }

    //converts the cartesian xy coordinates that are used in the code to isometric positions they will be displayed in the world
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x + y, 0.5f * (y - x), 0) * cellsize + originPos;
    }

    //converts the world position isometric coordinate to get the cartesian xy coordinates that are used in the code
    public void GetXYCoord(Vector3 worldPosition, out int x, out int y)
    {
        float gridx = worldPosition.x - originPos.x;
        float gridy = worldPosition.y - originPos.y;
        x = Mathf.FloorToInt((0.5f* gridx - gridy)/cellsize);
        y = Mathf.FloorToInt((0.5f * gridx + gridy) /cellsize);
    }

    //uses the private set method and the converstion method to directly set the value of the grid tile that was clicked on 
    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        int x;
        int y;
        GetXYCoord(worldPosition,out x,out y);
        SetGridObject(x, y, value);
    }
    
    //uses the private get method and the conversion method to directly read the value of the grid tile that was clicked on
    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x;
        int y;
        GetXYCoord(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

    //if the x,y value is in the int array the this method will find it and change the value to the value that was put into the method
    //private method as it will only be used internally, needs to be converted from isometric first
    void SetGridObject(int x, int y, TGridObject value)
    {
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            testTextArray[x, y].text = gridArray[x, y].ToString();
            if(OnGridValueChanged != null)
            {
                OnGridValueChanged(this, new OnGridValueChangedEvent { x = x, y = y });
            }
        } 
    }

    public void TriggerGridObjectChanged(int x, int y)
    {
        if (OnGridValueChanged != null)
        {
            OnGridValueChanged(this, new OnGridValueChangedEvent { x = x, y = y });
        }
    }

    //gets the int stored in the x,y of the intarray, if the x,y values are outside of the array it returns -1 
    //private as it will only be used internally by the grid class, as needs to be converted to isometric before use in game
    TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(TGridObject);
        }
    }

    //converts the mouse input position on the screen to the unity world position
    //need to put this in a seperate utility class at some point
    /*public static Vector3 ScreenToWorldPoint(Vector3 screenPos, Camera camera)
    {
        Vector3 worldPosition = camera.ScreenToWorldPoint(screenPos);
        return worldPosition;
    }*/

    //method used to instantiate a text mesh object for testing the grid to be removed later down the line when tiles will be displayed. 
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textanchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform tr = gameObject.transform;
        tr.SetParent(parent, false);
        tr.localPosition = localPosition;
        TextMesh txtM = gameObject.GetComponent<TextMesh>();
        txtM.anchor = textanchor;
        txtM.alignment = textAlignment;
        txtM.text = text;
        txtM.fontSize = fontSize;
        txtM.color = color;
        txtM.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return txtM;
    }
}
