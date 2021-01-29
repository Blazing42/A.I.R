using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingNode
{
    private Grid<PathfindingNode> pathfindingGrid;
    int x;
    public int X { get { return x; } }
    int y;
    public int Y { get { return y; } }

    public int gCost;
    public int hCost;
    public int fCost;

    public bool walkable;

    public PathfindingNode previousNode;

    public PathfindingNode(Grid<PathfindingNode> grid, int x, int y)
    {
        pathfindingGrid = grid;
        this.x = x;
        this.y = y;
        walkable = true;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
}
